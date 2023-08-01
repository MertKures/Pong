using System;
using System.Drawing;
using System.Windows.Forms;

namespace PingPong
{
    public class Player
    {
        private static int ID_COUNTER = 0;

        private readonly int PLAYER_ID;

        public RectangleF Rect = new RectangleF(0, 0, 5, 50);

        public PointF Speed = new PointF(0, 5);

        private PictureBox scenePB;

        public int Score = 0;

        public Player(PictureBox scenePB)
        {
            if (scenePB == null)
                throw new ArgumentNullException("scenePB", "Scene picture box cannot be null.");
            this.scenePB = scenePB;
            if (ID_COUNTER >= 2)
                throw new InvalidOperationException("There can be only two players.");
            PLAYER_ID = ++ID_COUNTER;

            if (PLAYER_ID == 1)
                SetLocation(0, scenePB.DisplayRectangle.Height / 2 - Rect.Height / 2);
            else
                SetLocation(scenePB.DisplayRectangle.Width - Rect.Width, scenePB.DisplayRectangle.Height / 2 - Rect.Height / 2);
        }

        public void Update(float deltaTime)
        {
            if (PLAYER_ID == 1)
            {
                if (Keyboard.IsKeyDown(Keys.W) && Rect.Y - Speed.Y * deltaTime >= scenePB.DisplayRectangle.Top)
                    SetLocation(int.MinValue, Rect.Y - Speed.Y * deltaTime);
                if (Keyboard.IsKeyDown(Keys.S) && Rect.Y + Speed.Y * deltaTime <= scenePB.DisplayRectangle.Height - Rect.Height)
                    SetLocation(int.MinValue, Rect.Y + Speed.Y * deltaTime);
            }
            else
            {
                if (Keyboard.IsKeyDown(Keys.Up) && Rect.Y - Speed.Y * deltaTime >= scenePB.DisplayRectangle.Top)
                    SetLocation(int.MinValue, Rect.Y - Speed.Y * deltaTime);
                if (Keyboard.IsKeyDown(Keys.Down) && Rect.Y + Speed.Y * deltaTime <= scenePB.DisplayRectangle.Height - Rect.Height)
                    SetLocation(int.MinValue, Rect.Y + Speed.Y * deltaTime);
            }
        }

        public void Draw(Graphics g, float deltaTime)
        {
            Update(deltaTime);

            Draw(g, Brushes.Black, deltaTime);
        }

        public void Draw(Graphics g, Brush brush, float deltaTime)
        {
            g.FillRectangle(brush, Rect);
        }

        public void SetLocation(float x, float y)
        {
            if (x > scenePB.DisplayRectangle.Right)
                throw new ArgumentOutOfRangeException("x", "X coordinate cannot be greater than scene picture box's width.");
            else if (y > scenePB.DisplayRectangle.Bottom)
                throw new ArgumentOutOfRangeException("y", "Y coordinate cannot be greater than scene picture box's height.");
            else if (y < scenePB.DisplayRectangle.Top)
                throw new ArgumentOutOfRangeException("y", "Y coordinate cannot be less than scene picture box's ball.");
            else if (x == int.MinValue)
            {
                Rect.Y = y;
                return;
            }
            else if (x < scenePB.DisplayRectangle.Left)
                throw new ArgumentOutOfRangeException("x", "X coordinate cannot be less than scene picture box's left.");

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
            Speed.X = 0;
            Speed.Y = y;
        }
    }
}