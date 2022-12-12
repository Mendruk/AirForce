using System.Drawing.Drawing2D;

namespace AirForce;

public partial class MainForm : Form
{
    private readonly Game game;

    public MainForm()
    {
        InitializeComponent();

        SetStyle(ControlStyles.DoubleBuffer |
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint, true);

        game = new Game(Width, Height);
        game.Defeat += OnDefeat;
    }

    private void timer_Tick(object sender, EventArgs e)
    {
        game.Update();
        Refresh();
    }

    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        game.Draw(e.Graphics);
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