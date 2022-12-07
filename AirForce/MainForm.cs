using System.Drawing.Drawing2D;

namespace AirForce;

public partial class MainForm : Form
{
    private readonly Font font = new(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
    private readonly LinearGradientBrush gradientBrush;
    private readonly Game game;
    private readonly Random random = new();
    private readonly Point[] stars;

    public MainForm()
    {
        InitializeComponent();

        SetStyle(ControlStyles.DoubleBuffer |
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint, true);

        gradientBrush = new LinearGradientBrush(
            new Point(0, 0),
            new Point(0, Height),
            Color.DarkSlateGray,
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
            stars[i].X -= 1;
            if (stars[i].X <= 0)
                stars[i].X = Width;
        }

        game.Update();
        Refresh();
    }

    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
        //e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

        e.Graphics.FillRectangle(gradientBrush, 0, 0, Width, Height);

        foreach (Point point in stars) 
            e.Graphics.FillEllipse(Brushes.CadetBlue, point.X, point.Y, 3, 3);

        game.Draw(e.Graphics);

        e.Graphics.DrawString("Health: " + game.Health, font, Brushes.CadetBlue, Width / 15, Height * 4 / 5);
        e.Graphics.DrawString("Score: " + game.Score, font, Brushes.CadetBlue, Width / 5, Height * 4 / 5);
    }

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.W)
        {
            game.IsMoveUp = true;
            game.IsMoveDown = false;
        }

        if (e.KeyCode == Keys.S)
        {
            game.IsMoveUp = false;
            game.IsMoveDown = true;
        }

        if (e.KeyCode == Keys.Space)
            game.IsFire = true;
    }

    private void MainForm_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.W)
            game.IsMoveUp = false;

        if (e.KeyCode == Keys.S)
            game.IsMoveDown = false;

        if (e.KeyCode == Keys.Space)
            game.IsFire = false;
    }

    private void OnDefeat(object? sender, EventArgs e)
    {
        MessageBox.Show("You LOSE!", "Defeat");
        game.Restart();
    }
}