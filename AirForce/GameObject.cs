namespace AirForce
{
    public abstract class GameObject
    {
        public CollisionTags Tag;
        public int Health;

        public int X, Y;
        protected int horizontalSpeed;
        protected int maxVerticalSpeed;
        protected int currentVerticalSpeed;
        protected int acceleration;
        protected bool canDodge;

        protected int reloadedTime;
        protected int currentRealodedTime;
        protected bool canFire;

        protected Bitmap sprite;
        protected int size;

        protected int frameNumber = 1;
        protected int currnetFrameNumber;
        protected List<Rectangle> frameRectangles;


        public void Draw(Graphics graphics)
        {
            //graphics.DrawEllipse(Pens.Blue, X - size / 2, Y - size / 2, size, size);

            graphics.DrawImage(sprite, new Rectangle(X - size / 2, Y - size / 2, size, size),
                frameRectangles[currnetFrameNumber], GraphicsUnit.Pixel);
        }

        public virtual void Update()
        {
            ChangeAnimationFrame();

            Move();
        }

        public int DistanceToObject(GameObject gameObject)
        {
            return (int)Math.Sqrt(Math.Pow(X - gameObject.X, 2) + Math.Pow(Y - gameObject.Y, 2));
        }

        protected virtual void ChangeAnimationFrame()
        {
            currnetFrameNumber++;

            if (currnetFrameNumber >= frameNumber)
                currnetFrameNumber = 0;
        }

        protected void CalculateFramesRectangles()
        {
            frameRectangles = new List<Rectangle>();

            for (int i = 0; i < frameNumber; i++)
            {
                frameRectangles.Add(new Rectangle(i * sprite.Width / frameNumber, 0,
                    sprite.Width / frameNumber, sprite.Height));
            }
        }

        private void Move()
        {
            if (currentVerticalSpeed > maxVerticalSpeed)
                currentVerticalSpeed = maxVerticalSpeed;

            if (currentVerticalSpeed < -maxVerticalSpeed)
                currentVerticalSpeed = -maxVerticalSpeed;

            if (currentVerticalSpeed > 0)
                currentVerticalSpeed -= 1;

            if (currentVerticalSpeed < 0)
                currentVerticalSpeed += 1;

            X += horizontalSpeed;
            Y += currentVerticalSpeed;
        }

        public void Dodge(Directions direction)
        {
            switch (direction)
            {
                case Directions.Up:
                    acceleration = 2;
                    break;
                case Directions.Down:
                    acceleration = -2;
                    break;
                default://todo
                    break;
            }
        }

        public void TakeDamage(int damage)
        {
            Health-=damage;
            if (Health <= 0)
            {
               // Game.GameObjects[Game.GameObjects.IndexOf(this)] = new Explosion(X, Y);
            }
        }
    }
}
