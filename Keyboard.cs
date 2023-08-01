using System.Windows.Forms;

namespace PingPong
{
    public static class Keyboard
    {
        public static bool IsKeyDown(Keys key) => GetAsyncKeyState(key) < 0;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys key);
    }
}