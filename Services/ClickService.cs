using Services.Models;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Services
{
    public class ClickService
	{
		private Thread ClickingThread { get; set; }

		private bool IsAlive { get; set; }
		private int Interval { get; set; }

		private const int MOUSEEVENTF_LEFTDOWN = 0x02;
		private const int MOUSEEVENTF_LEFTUP = 0x04;
		//private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
		//private const int MOUSEEVENTF_RIGHTUP = 0x10;

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern bool GetCursorPos(out POINT lpPoint);

		public ClickService() : base()
		{
			IsAlive = false;
			this.Interval = 20;
			ClickingThread = new Thread(this.Run);
			ClickingThread.Start();
		}

		public ClickService(int interval) : base()
		{
			IsAlive = false;
			this.Interval = interval;
			ClickingThread = new Thread(this.Run);
			ClickingThread.Start();
		}

		~ClickService()
		{
			ClickingThread.Join();
		}

		private void StartClicking()
		{
			Interval = 20;
			IsAlive = true;
		}

		private void Stop()
		{
			Interval = 1000;
			IsAlive = false;
		}

		public void Switch()
		{
			if (IsAlive)
			{
				Stop();
				return;
			}

			StartClicking();
		}

		private void Run()
		{
			while (true)
			{
				if (IsAlive)
				{

					Point currentCursorPosition = GetCursorPosition();

					uint X = (uint)currentCursorPosition.X;
					uint Y = (uint)currentCursorPosition.Y;

					mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
				}
				Thread.Sleep(Interval);
			}
		}

		private static Point GetCursorPosition()
		{
			GetCursorPos(out POINT lpPoint);
			return lpPoint;
		}
	}
}
