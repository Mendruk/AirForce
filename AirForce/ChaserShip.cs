namespace AirForce
{
    public class ChaserShip : Ship
    {
        public static Queue<ChaserShip> Pull;
        private static int pullSize = 20;

        private int horizontalSpeed = 6;

        static ChaserShip()
        {
            Pull = new Queue<ChaserShip>();

            for (int i = 0; i < pullSize; i++)
                Pull.Enqueue(new ChaserShip());
        }

        public ChaserShip()
        {
            sprite = Resource.chaser_ship;
            size = sprite.Height;

            X = Game.GameFieldWidth;
            Y = Game.GameFieldHeight / 2 - sprite.Height / 2;

            Start();

            acceleration = 3;
            maxVerticalSpeed = 8;

            currnetHp = 1;
        }

        public override void Update()
        {
            base.Update();

            X -= horizontalSpeed;

            if (X <= 0)
                Reset();

            foreach (GameObject gameObject in Game.GameObjectList.Where(obj => obj.isEnable))
            {
                if (!(gameObject is PlayerBullet or PlayerShip))
                    continue;

                if (DistanceToObject(gameObject) > sprite.Width * 2)
                    continue;

                if (DistanceToObject(gameObject) < sprite.Width / 2)
                {
                    TakeDamage(1);
                    gameObject.isEnable = false;
                }

                if (gameObject.X > X)
                    continue;

                if (gameObject.Y > Y - size / 2 &&
                    gameObject.Y < Y + size / 2)
                {
                    if (gameObject.Y > Y)
                        currentVerticalSpeed -= acceleration;
                    else
                        currentVerticalSpeed += acceleration;

                    return;
                }
            }
        }

        protected override void Destroy()
        {
            Effect.CreateEffect(X, Y);

            Reset();
        }

        private void Reset()
        {
            Pull.Enqueue(this);
            isEnable = false;
            maxVerticalSpeed = 4;
            currentVerticalSpeed = 0;
            currnetHp = 1;
        }
    }
}
