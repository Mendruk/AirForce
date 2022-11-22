namespace AirForce
{
    public class PlayerBullet : GameObject
    {
        public static Queue<PlayerBullet> Pull;
        private static int pullSize = 100;

        private int speed = 15;

        static PlayerBullet()
        {
            Pull = new Queue<PlayerBullet>();

            for (int i = 0; i < pullSize; i++)
                Pull.Enqueue(new PlayerBullet(0, 0));
        }

        public PlayerBullet(int x, int y)
        {
            sprite = Resource.player_shot;
            size = sprite.Height;

            X = x;
            Y = y;
            frameNumber = 3;

            Start();
        }

        public override void Update()
        {
            base.Update();

            if (X >= Game.GameFieldWidth)
            {
                Pull.Enqueue(this);
                isEnable = false;
                return;
            }

            Move();
        }

        private void Move()
        {
            X += speed;
        }
    }
}


