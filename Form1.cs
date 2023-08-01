using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PingPong
{
    // SORUN: Top çerçevenin aşağısından gidip yok olunca oyun bitiyor.
    // SORUN: Bazen top, oyuncunun çubuğunun alt kısmına çarpınca oyun bitiyor.

    public partial class Form1 : Form
    {
        float deltaTime = 1;

        Random rnd = new Random();

        Stopwatch timer = new Stopwatch();

        Player player1;
        Player player2;
        Ball ball;

        bool IsFinished = false;

        public Form1()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            Load += Form1_Load;

            Resize += Form1_Resize;
            Frame.Paint += Frame_Paint;

            player1 = new Player(Frame);
            player2 = new Player(Frame);
            ball = new Ball(Frame);

            player1.Rect.Height = 100;
            player1.Rect = new RectangleF(5, Frame.Height / 2 - 25, player1.Rect.Width, player1.Rect.Height);

            player2.Rect.Height = 100;
            player2.Rect = new RectangleF(Frame.Width - 30, Frame.Height / 2 - 25, player2.Rect.Width, player2.Rect.Height);

            ball.SetLocation(Frame.Width / 2 - ball.Rect.Width / 2, Frame.Height / 2 - ball.Rect.Height / 2);

            player1.SetSpeed(0, 500 * Frame.Width / Frame.Height);
            player2.SetSpeed(0, 500 * Frame.Width / Frame.Height);

            ball.ConstantSpeed.X = 250;
            ball.ConstantSpeed.Y = 250;
            ball.SetSpeed(250, 250);

            if (rnd.Next(-1, 2) == -1)
                ball.SetSpeed(-ball.Speed.X, ball.Speed.Y);

            RenderTimer.Tick += RenderTimer_Tick;
            RenderTimer.Start();

            timer.Start();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            player1.Rect.X = 5;
            player2.Rect.X = Frame.Width - player2.Rect.Width - 5;

            player1.Rect.Y = Frame.Height / 2 - player1.Rect.Height / 2;
            player2.Rect.Y = Frame.Height / 2 - player2.Rect.Height / 2;

            player1.SetSpeed(0, 500 * Frame.Width / Frame.Height);
            player2.SetSpeed(0, 500 * Frame.Width / Frame.Height);

            ball.SetSpeed(ball.ConstantSpeed.X * Width / Height, ball.ConstantSpeed.Y * Width / Height);
        }

        private void Frame_Paint(object sender, PaintEventArgs e)
        {
            GrafigiAyarla(e.Graphics);

            Render(e.Graphics);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Rectangle rectangle1 = Frame.DisplayRectangle;
            Rectangle rectangle2 = new Rectangle(10, Frame.DisplayRectangle.Bottom - 5, 10, 10);

            Console.WriteLine(rectangle2.IntersectsWith(rectangle1));
            Console.WriteLine(rectangle1.IntersectsWith(rectangle2));
        }

        private void GrafigiAyarla(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
        }

        private void Render(Graphics g)
        {
            deltaTime = timer.Elapsed.Milliseconds / 1000.0f;
            timer.Restart();

            if (Keyboard.IsKeyDown(Keys.R))
            {
                ball.lastTouchedPlayer = null;

                player1.SetLocation(5, Frame.Height / 2 - player1.Rect.Height / 2);
                player2.SetLocation(Frame.Width - player2.Rect.Width - 5, Frame.Height / 2 - player2.Rect.Height / 2);

                if (rnd.Next(-1, 2) == -1)
                    ball.SetSpeed(-ball.Speed.X, ball.Speed.Y);
                else
                    ball.SetSpeed(ball.Speed.X, ball.Speed.Y);

                ball.SetLocation(Frame.Width / 2 - ball.Rect.Width / 2, Frame.Height / 2 - ball.Rect.Height / 2);

                ball.Draw(g, deltaTime);
                player1.Draw(g, deltaTime);
                player2.Draw(g, deltaTime);

                IsFinished = false;

                return;
            }

            g.Clear(BackColor);

            g.DrawString("Oyuncu 1 : " + player1.Score.ToString(), Font, Brushes.Blue, new Point(1, 1));

            g.DrawString("Oyuncu 2 : " + player2.Score.ToString(), Font, Brushes.Blue, new Point(Frame.Width - 35 - g.MeasureString("Oyuncu 2 : " + player2.Score.ToString(), Font).ToSize().Width, 1));

            if (ball.Rect.Y >= Frame.DisplayRectangle.Bottom - ball.Rect.Height)
            {
                ball.Speed.Y *= -1;
                ball.SetLocation(ball.Rect.X, Frame.DisplayRectangle.Bottom - ball.Rect.Height);
            }
            else if (ball.Rect.Y <= Frame.DisplayRectangle.Top)
            {
                ball.Speed.Y *= -1;
                ball.SetLocation(ball.Rect.X, Frame.DisplayRectangle.Top);
            }

            if (player1.Rect.IntersectsWith(ball.Rect))
            {
                ball.Speed.X *= -1;
                ball.lastTouchedPlayer = player1;
                ball.SetLocation(player1.Rect.Right, ball.Rect.Y);
            }
            else if (player2.Rect.IntersectsWith(ball.Rect))
            {
                ball.Speed.X *= -1;
                ball.lastTouchedPlayer = player2;
                ball.SetLocation(player2.Rect.Left - ball.Rect.Width, ball.Rect.Y);
            }

            if (!ball.IsVisible && !IsFinished)
            {
                IsFinished = true;

                if (ball.Rect.X >= Frame.DisplayRectangle.Right)
                    player1.Score++;
                else if (ball.Rect.X <= Frame.DisplayRectangle.Left)
                    player2.Score++;
            }

            if (IsFinished)
            {
                Size yazi = g.MeasureString("Tekrar başlamak için 'R'ye basın.", Font).ToSize();

                g.DrawString("Tekrar başlamak için 'R'ye basın.", Font, Brushes.Green, new Point(Frame.Width / 2 - yazi.Width / 2, Frame.Height / 2 - yazi.Height / 2));

                return;
            }

            //ball.HiziniArttir((float)rnd.NextDouble() / 50, (float)rnd.NextDouble() / 50);

            ball.Draw(g, deltaTime);
            player1.Draw(g, deltaTime);
            player2.Draw(g, deltaTime);
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            Frame.Invalidate();
        }
    }
}
