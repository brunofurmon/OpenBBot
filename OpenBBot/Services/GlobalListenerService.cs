﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;


namespace OpenBBot.Services
{
    public static class GlobalListenerService
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        public static Action ExternalCallback { get; set; }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        // User-Chosen Hotkey
        private static int HotKey { get; set; }

        public static void InstallHook()
        {
            _hookID = SetHook(_proc);
        }

        public static void InstallHook(Action externalCallBack)
        {
            _hookID = SetHook(_proc);
            ExternalCallback = externalCallBack;
        }

        public static void UninstallHook()
        {
            UnhookWindowsHookEx(_hookID);
        }

        public static void SetHotKey(int hotkey)
        {
            HotKey = hotkey;
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                // Test if code is corresponding to hotkey
                if ((Key)vkCode == Key.RightShift)
                {
                    ExternalCallback();
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
