using System.Drawing;

namespace PingPong
{
    public static class Utils
    {
        private static Rectangle tempRectangle = new Rectangle(0, 0, 0, 0);

        public static bool IntersectsWith(Rectangle rectangle1, RectangleF rectangle2)
        {
            tempRectangle.X = (int)rectangle2.X;
            tempRectangle.Y = (int)rectangle2.Y;
            tempRectangle.Width = (int)rectangle2.Width;
            tempRectangle.Height = (int)rectangle2.Height;

            return rectangle1.IntersectsWith(tempRectangle);
        }
    }
}
