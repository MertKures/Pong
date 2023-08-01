using System;
using System.Drawing;
using System.Windows.Forms;

namespace PingPong
{
    public class Ball
    {
        public PointF ConstantSpeed = new PointF(15, 15);
        public RectangleF Rect = new RectangleF(0, 0, 10, 10);
        public PointF Speed = new PointF(15, 15);
        public bool IsVisible { get => Utils.IntersectsWith(scenePB.DisplayRectangle, Rect); }
        public Player lastTouchedPlayer = null;
        private PictureBox scenePB;

        public Ball(PictureBox scenePB, int x, int y)
        {
            if (scenePB == null)
                throw new ArgumentNullException("scenePB", "Scene picture box cannot be null.");
            Rect.X = x;
            Rect.Y = y;
        }

        public Ball(PictureBox scenePB)
        {
            if (scenePB == null)
                throw new ArgumentNullException("scenePB", "Scene picture box cannot be null.");
            this.scenePB = scenePB;
        }

        public void SetLocation(float x, float y)
        {
            if (x > scenePB.DisplayRectangle.Right + Rect.Width * 2)
                throw new ArgumentOutOfRangeException("x", "X coordinate cannot be greater than scene picture box's width.");
            else if (x < scenePB.DisplayRectangle.Left - Rect.Width * 2)
                throw new ArgumentOutOfRangeException("x", "X coordinate cannot be less than scene picture box's left.");
            else if (y > scenePB.DisplayRectangle.Bottom + Rect.Height * 2)
                throw new ArgumentOutOfRangeException("y", "Y coordinate cannot be greater than scene picture box's height.");
            else if (y < scenePB.DisplayRectangle.Top - Rect.Height * 2)
                throw new ArgumentOutOfRangeException("y", "Y coordinate cannot be less than scene picture box's ball.");

            Rect.X = x;
            Rect.Y = y;
        }

        public void SetSize(int width, int height)
        {
            if (width == 0)
                throw new ArgumentOutOfRangeException("width", "Width cannot be zero.");
            else if (width < 0)
                throw new ArgumentOutOfRangeException("width", "Width cannot be less than zero.");
            else if (width > scenePB.DisplayRectangle.Width * 0.2)
                throw new ArgumentOutOfRangeException("width", "Width cannot be greater than 20% of the scene picture box's width.");

            else if (height == 0)
                throw new ArgumentOutOfRangeException("height", "Height cannot be zero.");
            else if (height < 0)
                throw new ArgumentOutOfRangeException("height", "Height cannot be less than zero.");
            else if (height > scenePB.DisplayRectangle.Height * 0.4)
                throw new ArgumentOutOfRangeException("height", "Height cannot be greater than 40% of the scene picture box's height.");

            Rect.Width = width;
            Rect.Height = height;
        }

        public void SetSpeed(float x, float y)
        {
            Speed.X = x;
            Speed.Y = y;
        }

        public void Update(float deltaTime)
        {
            Rect.X += (int)(Speed.X * deltaTime);
            Rect.Y += (int)(Speed.Y * deltaTime);
        }

        public void Draw(Graphics g, Brush brush, float deltaTime)
        {
            //KontrolEt(g);

            if (!IsVisible)
                return;

            Update(deltaTime);

            g.FillRectangle(brush, Rect);
        }

        public void Draw(Graphics g, float deltaTime)
        {
            Draw(g, Brushes.Green, deltaTime);
        }

        //private void KontrolEt(Graphics g)
        //{
        //    //!(g.VisibleClipBounds.Contains(new PointF(X, Y - Genislik / 2)) && g.VisibleClipBounds.Contains(new PointF(X, Y + Genislik / 2)))
        //    if (Rect.X - Rect.Width >= g.VisibleClipBounds.X + g.VisibleClipBounds.Width || Rect.X <= g.VisibleClipBounds.X)
        //        IsVisible = true;
        //    else
        //        IsVisible = false;
        //}
    }
}