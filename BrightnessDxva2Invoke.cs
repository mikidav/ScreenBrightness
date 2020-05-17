using System;
using System.Runtime.InteropServices;

public class BrightnessDxva2Invoke : IBrightnessDxva2Invoke
{
    private IntPtr m_hmonitor;
    private int monitorNum;
    NativeMethods.PHYSICAL_MONITOR m_physicalMonitor;
    public bool IsSupported { get; private set; }

    public uint MaxBrightness { get; private set; }

    public uint MinBrightness { get; private set; }

    public uint CurrentBrightness { get; set; }

    public void Initialize()
    {
        try
        {
            IsSupported = true;

            // Querying the Windows service to get the Brightness API.
            // get handle to primary display
            m_hmonitor = NativeMethods.MonitorFromWindow(IntPtr.Zero, NativeMethods.MONITOR_DEFAULTTO.PRIMARY);

            // get number of physical displays (assume only one for simplicity)
            bool success = NativeMethods.GetNumberOfPhysicalMonitorsFromHMONITOR(m_hmonitor, out monitorNum);
            m_physicalMonitor = new NativeMethods.PHYSICAL_MONITOR();

            success = NativeMethods.GetPhysicalMonitorsFromHMONITOR(m_hmonitor, monitorNum, ref m_physicalMonitor);

            uint min, max, current;
            // commonly min and max are 0-100 which represents a percentage brightness
            success = NativeMethods.GetMonitorBrightness(m_physicalMonitor.hPhysicalMonitor, out min, out current, out max);

            MinBrightness = min;
            MaxBrightness = max;
            CurrentBrightness = current;
            IsSupported = success;

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
        bool res;
        if (IsSupported)
            res = NativeMethods.SetMonitorBrightness(m_hmonitor, level);

    }

    public void Dispose()
    {
        if (IsSupported)
            NativeMethods.DestroyPhysicalMonitors(monitorNum, ref m_physicalMonitor);
    }


    private static class NativeMethods
    {
        [DllImport("Dxva2")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetMonitorBrightness(IntPtr hMonitor,
            out uint pdwMinimumBrightness,
            out uint pdwCurrentBrightness,
            out uint pdwMaximumBrightness);

        [DllImport("Dxva2")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetMonitorBrightness(IntPtr hMonitor, uint newBrightness);

        [DllImport("Dxva2")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, out int numberOfPhysicalMonitors);

        [DllImport("Dxva2", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, int physicalMonitorArraySize, ref PHYSICAL_MONITOR physicalMonitorArray);

        [DllImport("Dxva2", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DestroyPhysicalMonitors(int physicalMonitorArraySize, ref PHYSICAL_MONITOR physicalMonitorArray);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct PHYSICAL_MONITOR
        {
            internal IntPtr hPhysicalMonitor;
            //[PHYSICAL_MONITOR_DESCRIPTION_SIZE]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            internal string szPhysicalMonitorDescription;
        }

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr hwnd, MONITOR_DEFAULTTO dwFlags);

        internal enum MONITOR_DEFAULTTO
        {
            NULL = 0x00000000,
            PRIMARY = 0x00000001,
            NEAREST = 0x00000002,
        }
    }

}

//public static class BrightnessDxva2Invoke
//{
//    public static bool SetBrightness(uint brightness)
//    {
//        if (brightness > 100)
//        {
//            brightness = 100;
//        }

//        // get handle to primary display
//        IntPtr hmon = NativeMethods.MonitorFromWindow(IntPtr.Zero, NativeMethods.MONITOR_DEFAULTTO.PRIMARY);

//        // get number of physical displays (assume only one for simplicity)
//        int num;
//        bool success = NativeMethods.GetNumberOfPhysicalMonitorsFromHMONITOR(hmon, out num);
//        NativeMethods.PHYSICAL_MONITOR pmon = new NativeMethods.PHYSICAL_MONITOR();

//        success = NativeMethods.GetPhysicalMonitorsFromHMONITOR(hmon, num, ref pmon);

//        uint min, max, current;
//        // commonly min and max are 0-100 which represents a percentage brightness
//        success = NativeMethods.GetMonitorBrightness(pmon.hPhysicalMonitor, out min, out current, out max);

//        // set to full brightness
//        success = NativeMethods.SetMonitorBrightness(pmon.hPhysicalMonitor, brightness);

//        success = NativeMethods.DestroyPhysicalMonitors(num, ref pmon);

//        return success;
//    }

//    public static bool GetBrightness(out uint min, out uint current, out uint max)
//    {
//        min = 0; max = 100; current = 100;
//        // get handle to primary display
//        IntPtr hmon = NativeMethods.MonitorFromWindow(IntPtr.Zero, NativeMethods.MONITOR_DEFAULTTO.PRIMARY);

//        // get number of physical displays (assume only one for simplicity)
//        int num;
//        bool success = NativeMethods.GetNumberOfPhysicalMonitorsFromHMONITOR(hmon, out num);
//        NativeMethods.PHYSICAL_MONITOR pmon = new NativeMethods.PHYSICAL_MONITOR();

//        success = NativeMethods.GetPhysicalMonitorsFromHMONITOR(hmon, num, ref pmon);

//        // commonly min and max are 0-100 which represents a percentage brightness
//        success = NativeMethods.GetMonitorBrightness(pmon.hPhysicalMonitor, out min, out current, out max);

//        success = NativeMethods.DestroyPhysicalMonitors(num, ref pmon);
//        return success;
//    }




//}