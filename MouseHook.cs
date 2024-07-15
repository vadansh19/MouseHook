using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Automation;
using System.Windows.Forms;

namespace HookApp
{
    public class MouseHook : IHook
    {
        #region Private Fields

        private readonly win32api.LowLevelMouseProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        #endregion Private Fields

        #region Public Constructors

        public MouseHook()
        {
            _proc = HookCallback;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Init()
        {
            _hookID = SetHook(_proc);
        }

        public void Stop()
        {
            win32api.UnhookWindowsHookEx(_hookID);
        }

        #endregion Public Methods

        #region Private Methods

        private string GetActiveProcessFileName(win32api.POINT point)
        {
            IntPtr hWnd = win32api.WindowFromPoint(point);
            win32api.GetWindowThreadProcessId(hWnd, out int processId);
            IntPtr hProcess = win32api.OpenProcess(0x0410, false, processId);
            var buffer = new StringBuilder(1024);
            win32api.GetModuleFileNameEx(hProcess, IntPtr.Zero, buffer, buffer.Capacity);
            win32api.CloseHandle(hProcess);
            return buffer.ToString().Substring(buffer.ToString().LastIndexOf('\\') + 1);
        }

        private string GetElementType(win32api.POINT point)
        {
            AutomationElement element = AutomationElement.FromPoint(new System.Windows.Point(point.x, point.y));
            if (element != null)
            {
                return element.Current.ControlType.ProgrammaticName.Split('.')[1];
            }
            return "Unknown";
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)win32api.WM_LBUTTONDOWN)
            {
                var hookStruct = (win32api.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(win32api.MSLLHOOKSTRUCT));
                var processName = GetActiveProcessFileName(hookStruct.pt);
                var elementType = GetElementType(hookStruct.pt);
                Console.WriteLine($"{processName}:" + " {X = " + $"{hookStruct.pt.x}" + ", Y = " + $"{hookStruct.pt.y}" + "}" + $": {elementType}");

            }
            return win32api.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private IntPtr SetHook(win32api.LowLevelMouseProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return win32api.SetWindowsHookEx(win32api.WH_MOUSE_LL, proc, win32api.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        #endregion Private Methods
    }
}
