using System;
using System.Runtime.InteropServices;

class WindowsDisplayAffinity
{
    [DllImport("user32.dll")]
    private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    [DllImport("user32.dll")]
    private static extern bool SetWindowDisplayAffinity(IntPtr hWnd, DisplayAffinity affinity);

    private enum DisplayAffinity : uint
    {
        WDA_NONE = 0x00000000,
        WDA_MONITOR = 0x00000001
    }

    public static void SetDAForWindows()
    {
        IntPtr windowHandle = IntPtr.Zero;
        do
        {
            windowHandle = FindWindowEx(IntPtr.Zero, windowHandle, null, null);
            SetWindowDisplayAffinity(windowHandle, DisplayAffinity.WDA_MONITOR);
        } while (windowHandle != IntPtr.Zero);
    }

    [DllImport("kernel32.dll")]
    public static extern bool FreeLibrary(IntPtr hModule);

    public static bool DllMain(IntPtr hModule, uint ul_reason_for_call, IntPtr lpReserved)
    {
        switch (ul_reason_for_call)
        {
            case 1: // DLL_PROCESS_ATTACH
                SetDAForWindows();
                break;
            case 2: // DLL_THREAD_ATTACH
            case 3: // DLL_THREAD_DETACH
            case 0: // DLL_PROCESS_DETACH
                break;
        }
        return false;
    }
}

