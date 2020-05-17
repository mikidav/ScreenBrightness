using System.Management;

namespace ScreenBrightness
{
    public class BrightnessWmi : IBrightnessWmi
    {
        private ManagementObject m_brightnessInstance;
        private ManagementBaseObject m_brightnessClass;

        public bool IsSupported { get; private set; }

        public uint MaxBrightness { get; private set; }

        public uint MinBrightness { get; private set; }

        public uint CurrentBrightness { get; set; }

        public void Initialize()
        {
            try
            {
                MaxBrightness = 100;
                MinBrightness = 0;

                IsSupported = true;

                // Querying the Windows service to get the Brightness API.
                var searcher = new ManagementObjectSearcher(
                    "root\\WMI",
                    "SELECT * FROM WmiMonitorBrightness");

                var results = searcher.Get();
                var resultEnum = results.GetEnumerator();
                resultEnum.MoveNext();
                m_brightnessClass = resultEnum.Current;

                // We need to create an instance to use the Set method!
                var instanceName = (string)m_brightnessClass["InstanceName"];
                m_brightnessInstance = new ManagementObject(
                    "root\\WMI",
                    "WmiMonitorBrightnessMethods.InstanceName='" + instanceName + "'",
                    null);


                //// Getting the current value.
                //var maxValue = m_brightnessClass.GetPropertyValue("MaxBrightness");
                //var valueString = maxValue.ToString();
                //MaxBrightness = uint.Parse(valueString); // Direct cast fails.

                //// Getting the current value.
                //var minValue = m_brightnessClass.GetPropertyValue("MinBrightness");
                //var minValueString = minValue.ToString();
                //MinBrightness = uint.Parse(valueString); // Direct cast fails.

                GettingCurrentValue();

            }
            catch (System.Exception ex)
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

           

            var inParams = m_brightnessInstance.GetMethodParameters("WmiSetBrightness");
            inParams["Brightness"] = level;
            inParams["Timeout"] = 0;
            ManagementBaseObject managementBaseObject = m_brightnessInstance.InvokeMethod("WmiSetBrightness", inParams, null);
            
            GettingCurrentValue();
        }

        public void Dispose()
        {
            if (IsSupported)
            {
                m_brightnessClass?.Dispose();
                m_brightnessInstance?.Dispose();
            }
        }

        private void GettingCurrentValue()
        {
            // Getting the current value.
            var currentValue = m_brightnessClass.GetPropertyValue("CurrentBrightness");
            var currentValueString = currentValue.ToString();
            CurrentBrightness = uint.Parse(currentValueString); // Direct cast fails.
        }
    }
}
