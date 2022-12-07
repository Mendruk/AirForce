namespace AirForce
{
    public class Ground
    {
        public readonly int Y;
        private readonly Rectangle rectangle;

        public Ground(int gameFieldWidth, int gameFieldHeight)
        {
            Y = gameFieldHeight / 5 * 4;
            rectangle = new Rectangle(0, Y, gameFieldWidth, gameFieldHeight);
        }

        public void Draw(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.DarkSlateGray, rectangle);
        }
    }
}
