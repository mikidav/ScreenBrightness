using System.Diagnostics;
using Windows.Graphics.Display;

namespace ScreenBrightness
{    public class BrightnessUwp : IBrightnessUwp
    {
        private BrightnessOverride brightnessOverride = null;

        public bool IsSupported { get; private set; }

        public uint MaxBrightness { get; private set; }

        public uint MinBrightness { get; private set; }

        public uint CurrentBrightness { get;  set; }

        public void Initialize()
        {
            try
            {
                IsSupported = true;

                brightnessOverride = BrightnessOverride.GetForCurrentView();

                MaxBrightness = (uint)brightnessOverride.GetLevelForScenario(DisplayBrightnessScenario.FullBrightness);
                MinBrightness = (uint)brightnessOverride.GetLevelForScenario(DisplayBrightnessScenario.IdleBrightness);
                CurrentBrightness = (uint)brightnessOverride.BrightnessLevel;
                //brightnessOverride.IsOverrideActiveChanged += overrideActiveChangedHandler;
                brightnessOverride.BrightnessLevelChanged += brightnessLevelChangedHandler;

            }
            catch (System.Exception)
            {
                IsSupported = false;

            }
        }

        public void ApplyBrightness(uint level)
        {
            if (level < MinBrightness)
            {
                level = MinBrightness;
            }

            if (level > MaxBrightness)
            {
                level = MaxBrightness;
            }

            brightnessOverride.SetBrightnessLevel(level, DisplayBrightnessOverrideOptions.None);

            brightnessOverride.StartOverride();
        }

        public void Dispose()
        {
            //brightnessOverride.IsOverrideActiveChanged += overrideActiveChangedHandler;
            brightnessOverride.BrightnessLevelChanged += brightnessLevelChangedHandler;
        }

        private void brightnessLevelChangedHandler(BrightnessOverride sender, object e)
        {
            CurrentBrightness = (uint)sender.BrightnessLevel;

            Debug.WriteLine($"BrightnessOverride BrightnessLevel changed to {sender.BrightnessLevel}");
        }

        //void overrideActiveChangedHandler(BrightnessOverride sender, object e)
        //{
        //    sender.IsOverrideActiveChanged -= overrideActiveChangedHandler;
        //    Debug.WriteLine($"BrightnessOverride IsOverrideActive changed to {sender.IsOverrideActive}");
        //}
    }
}
