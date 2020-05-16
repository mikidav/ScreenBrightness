using System;

namespace ScreenBrightness
{
    public interface IBrightnessProvider : IDisposable
    {
        bool IsSupported { get; }

        uint MaxBrightness { get; }

        uint MinBrightness { get; }

        uint CurrentBrightness { get; set; }

        void ApplyBrightness(uint level);

        void Initialize();
    }
}