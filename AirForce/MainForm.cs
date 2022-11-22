using System.Drawing.Drawing2D;

namespace AirForce
{
    public partial class MainForm : Form
    {
        private readonly LinearGradientBrush gradientBrush;
        private Game game;

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

            game = new Game(Width, Height);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            game.Update();
            Refresh();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillRectangle(gradientBrush, 0, 0, Width, Height);
            game.Draw(e.Graphics);
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
    }
}
