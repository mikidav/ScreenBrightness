using Caliburn.Micro;
using Windows.UI.Xaml.Controls;

namespace ScreenBrightness.ViewModels
{
    public class MainViewModel : Conductor<IScreen>.Collection.AllActive
    {
        public MainViewModel(IBrightnessDxva2Invoke brightnessDxva2Invoke, IBrightnessUwp brightnessUwp, IBrightnessWmi brightnessWmi)
        {
            BrightnessDxva2Invoke = brightnessDxva2Invoke;
            BrightnessUwp = brightnessUwp;
            BrightnessWmi = brightnessWmi;
        }

        private IBrightnessDxva2Invoke BrightnessDxva2Invoke { get; }
        private IBrightnessUwp BrightnessUwp { get; }
        private IBrightnessWmi BrightnessWmi { get; }

        public void Initialize()
        {
            BrightnessDxva2Invoke.Initialize();
            m_brightnessLevelDxva2 = BrightnessDxva2Invoke.CurrentBrightness;
            NotifyOfPropertyChange(() => MaxBrightnessLevelDxva2);
            NotifyOfPropertyChange(() => MinBrightnessLevelDxva2);
            NotifyOfPropertyChange(() => BrightnessLevelDxva2);
            NotifyOfPropertyChange(() => IsSupportedsDxva2Invoke);


            BrightnessUwp.Initialize();
            NotifyOfPropertyChange(() => MaxBrightnessLevelUwp);
            NotifyOfPropertyChange(() => MinBrightnessLevelUwp);
            NotifyOfPropertyChange(() => BrightnessLevelUwp);
            NotifyOfPropertyChange(() => IsSupportedUwp);

            BrightnessWmi.Initialize();
            m_brightnessLevelWmi = BrightnessWmi.CurrentBrightness;
            NotifyOfPropertyChange(() => MaxBrightnessLevelWmi);
            NotifyOfPropertyChange(() => MinBrightnessLevelWmi);
            NotifyOfPropertyChange(() => BrightnessLevelWmi);
            NotifyOfPropertyChange(() => IsSupportedWmi);
        }

        private uint m_brightnessLevelWmi;
        private uint m_brightnessLevelDxva2;

        public uint BrightnessLevelWmi
        {
            get { return m_brightnessLevelWmi; }
            set
            {
                m_brightnessLevelWmi = value;
                BrightnessWmi.ApplyBrightness(value);
                NotifyOfPropertyChange(() => BrightnessLevelWmi);
            }
        }

        public uint MaxBrightnessLevelWmi
        {
            get { return BrightnessWmi.MaxBrightness; }

        }

        public uint MinBrightnessLevelWmi
        {
            get { return BrightnessWmi.MinBrightness; }
        }

        public bool IsSupportedWmi
        {
            get
            {
                return BrightnessWmi.IsSupported;
            }
        }


        public void OnSliderMouseUpWmi(Slider slider)
        {

        }
        public void OnSliderMouseUpDXva2(Slider slider)
        {

        }
        public void OnSliderMouseUpUwp(Slider slider)
        {

        }


        public uint BrightnessLevelUwp
        {
            get { return BrightnessUwp.CurrentBrightness; }
            set
            {
                BrightnessUwp.ApplyBrightness(value);
                NotifyOfPropertyChange(() => BrightnessLevelUwp);
            }
        }

        public uint MaxBrightnessLevelUwp
        {
            get { return BrightnessUwp.MaxBrightness; }

        }

        public uint MinBrightnessLevelUwp
        {
            get { return BrightnessUwp.MinBrightness; }
        }

        public bool IsSupportedUwp
        {
            get
            {
                return BrightnessUwp.IsSupported;
            }
        }

        public uint BrightnessLevelDxva2
        {
            get { return m_brightnessLevelDxva2; }
            set
            {
                m_brightnessLevelDxva2 = value;
                BrightnessDxva2Invoke.ApplyBrightness(value);
                NotifyOfPropertyChange(() => BrightnessLevelDxva2);
            }
        }

        public uint MaxBrightnessLevelDxva2
        {
            get { return BrightnessDxva2Invoke.MaxBrightness; }

        }

        public uint MinBrightnessLevelDxva2
        {
            get { return BrightnessDxva2Invoke.MinBrightness; }
        }
        public bool IsSupportedsDxva2Invoke
        {
            get
            {
                return BrightnessDxva2Invoke.IsSupported;
            }
        }

    }
}
