using System.Runtime.InteropServices;
using System.Windows;


namespace OpenBBot.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public uint X;
        public uint Y;

        public static implicit operator Point(POINT point)
        {
            return new Point(point.X, point.Y);
        }
    }
}
