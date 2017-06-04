using System.Threading;
using System.Runtime.InteropServices;
using OpenBBot.Models;
using System.Windows;

namespace OpenBBot.Services
{
    public class ClickingThreadService
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        public ClickingThreadService() : base()
        {
            IsAlive = false;
            this.Interval = 20;
        }

        public ClickingThreadService(int interval) : base()
        {
            IsAlive = false;
            this.Interval = interval;    
        }

        private void StartClicking()
        {
            ClickingThread = new Thread(this.Run);
            this.IsAlive = true;
            ClickingThread.Start();
        }

        private void Stop()
        {
            this.IsAlive = false;
            this.ClickingThread.Join();
        }

        public void Switch()
        {
            if (this.IsAlive)
            {
                Stop();
            }
            else
            {
                StartClicking();
            }
        }

        private void Run()
        {
            while (this.IsAlive)
            {
                Point currentCursorPosition = GetCursorPosition();

                uint X = (uint) currentCursorPosition.X;
                uint Y = (uint)currentCursorPosition.Y;

                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
                Thread.Sleep(Interval);
            }
        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        private static Point GetCursorPosition()
        {
            GetCursorPos(out POINT lpPoint);
            return lpPoint;
        }

        private bool IsAlive { get; set; }
        private Thread ClickingThread { get; set; }
        private int Interval { get; set; }
    }
}
