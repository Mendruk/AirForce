namespace AirForce
{
    public abstract class GameObject
    {
        public static Queue<GameObject> Pull = new();
        public static List<GameObject> GameObjects = new();

        public bool isEnable;
        public CollisionTags Tag;
        public int Health;

        public int X, Y;
        protected int horizontalSpeed;
        protected int maxVerticalSpeed;
        protected int currentVerticalSpeed;
        protected int acceleration;
        public bool canDodge;

        protected int reloadedTime;
        protected int currentRealodedTime;
        public bool canFire;


        protected Bitmap sprite;
        public int size;

        protected int frameNumber = 1;
        protected int currnetFrameNumber;
        protected List<Rectangle> frameRectangles;

        /////////////////////////////////////TODO////////////////////////////////////////
        public static GameObject Create(Type type, int x, int y)
        {
            while (true)
            {
                GameObject obj = Pull.Dequeue();

                if (obj.GetType() == type)
                {
                    obj.isEnable = true;
                    obj.X = x;
                    obj.Y = y;
                    obj.Reset();
                    return obj;
                }

                Pull.Enqueue(obj);
            }
        }

        public static GameObject Create(Type type, int x, int y, int size)
        {
            while (true)
            {
                GameObject obj = Pull.Dequeue();

                if (obj.GetType() == type)
                {
                    obj.isEnable = true;
                    obj.X = x;
                    obj.Y = y;
                    obj.size = size;
                    obj.Reset();
                    return obj;
                }

                Pull.Enqueue(obj);
            }
        }
        /////////////////////////////////////////////////////////////////////////////

        public static void Delete(GameObject gameObject)
        {
            gameObject.isEnable = false;
            Pull.Enqueue(gameObject);
        }

        public void Draw(Graphics graphics)
        {
            //graphics.DrawEllipse(Pens.Blue, X - size / 2, Y - size / 2, size, size);

            graphics.DrawImage(sprite, new Rectangle(X - size / 2, Y - size / 2, size, size),
                frameRectangles[currnetFrameNumber], GraphicsUnit.Pixel);
            ChangeAnimationFrame();
        }

        public virtual void Update()
        {
            Move();
        }

        public int DistanceToObject(GameObject gameObject)
        {
            return (int)Math.Sqrt(Math.Pow(X - gameObject.X, 2) + Math.Pow(Y - gameObject.Y, 2));
        }

        protected virtual void ChangeAnimationFrame()
        {
            currnetFrameNumber += 1;

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

            if (Y < size / 2)
                Y = size / 2;
        }

        public void Dodge(Directions direction)
        {
            switch (direction)
            {
                case Directions.Up:
                    currentVerticalSpeed += acceleration;
                    break;
                case Directions.Down:
                    currentVerticalSpeed -= acceleration;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public virtual void Fire()
        {
            if (currentRealodedTime < reloadedTime) 
                return;

            Create(typeof(EnemyBullet), X, Y);
            currentRealodedTime = 0;
        }

        public virtual void IsDestroyedIfTakeDamage(int damage, out bool isDestroyed)
        {
            Health -= damage;
            isDestroyed = false;

            if (Health > 0)
                return;

            isDestroyed=true;
            Delete(this);
            Create(typeof(Explosion), X, Y, size);
        }

        protected abstract void Reset();
    }
}
