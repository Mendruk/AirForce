namespace AirForce
{
    public abstract class GameObject
    {
        public bool isEnable;

        public int X, Y;

        protected Bitmap sprite;
        protected int size;

        protected int frameNumber = 1;
        protected int currnetFrameNumber;
        protected List<Rectangle> frameRectangles;

        protected void Start()
        {
            Game.GameObjectList.Add(this);

            frameRectangles = new List<Rectangle>();

            for (int i = 0; i < frameNumber; i++)
            {
                frameRectangles.Add(new Rectangle(i * sprite.Width / frameNumber, 0,
                    sprite.Width / frameNumber, sprite.Height));
            }
        }

        public void Draw(Graphics graphics)
        {
            graphics.DrawEllipse(Pens.Blue, X - size / 2, Y - size / 2, size, size);

            graphics.DrawImage(sprite, new Rectangle(X - size / 2, Y - size / 2, size, size), 
                frameRectangles[currnetFrameNumber], GraphicsUnit.Pixel);
        }

        protected int DistanceToObject(GameObject gameObject)
        {
            return (int)Math.Sqrt(Math.Pow(X - gameObject.X, 2) + Math.Pow(Y - gameObject.Y, 2));
        }

        public virtual void Update()
        {
            currnetFrameNumber++;

            if (currnetFrameNumber >= frameNumber)
                currnetFrameNumber = 0;

        }
    }
}
