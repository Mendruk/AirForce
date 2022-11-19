namespace AirForce
{
    public abstract class GameObject
    {
        public int X, Y;
        public Bitmap sprite;

        public void Draw(Graphics graphics)
        {
            graphics.DrawImage(sprite, X, Y, sprite.Width, sprite.Height);
        }

        public abstract void Update();
    }
}
