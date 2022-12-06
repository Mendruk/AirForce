using System.Drawing.Drawing2D;

namespace AirForce
{
    public partial class MainForm : Form
    {
        private readonly LinearGradientBrush gradientBrush;
        private Game game;
        private Random random=new Random();
        private Point[] stars;

        public MainForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.DoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            gradientBrush = new(
                new Point(0, 0),
                new Point(0, Height),
                Color.Black,
                Color.CadetBlue);

            stars = new Point[100];
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].X = random.Next(Width);
                stars[i].Y = random.Next(Height);
            }

            game = new Game(Width, Height);
            game.Defeat += OnDefeat;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].X-=1;
                if (stars[i].X <= 0)
                    stars[i].X = Width;
            }
            game.Update();
            Refresh();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.FillRectangle(gradientBrush, 0, 0, Width, Height);
            foreach (Point point in stars)
            {
                e.Graphics.FillEllipse(Brushes.AliceBlue,point.X,point.Y,3,3);
            }

            game.Draw(e.Graphics);

            e.Graphics.DrawString("Health: " + game.Health, DefaultFont, Brushes.Black, Width / 4, Height * 5 / 6);
            e.Graphics.DrawString("Score: " + game.Score, DefaultFont, Brushes.Black, Width / 4, Height * 6 / 7);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                game.isMoveUp = true;
                game.isMoveDown = false;
            }

            if (e.KeyCode == Keys.S)
            {
                game.isMoveUp = false;
                game.isMoveDown = true;
            }

            if (e.KeyCode == Keys.Space)
                game.isFire = true;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
                game.isMoveUp = false;

            if (e.KeyCode == Keys.S)
                game.isMoveDown = false;

            if (e.KeyCode == Keys.Space)
                game.isFire = false;

        }

        private void OnDefeat(object? sender, EventArgs e)
        {
            MessageBox.Show("You LOSE!", "Defeat");
            game.Restart();
        }

    }
}
