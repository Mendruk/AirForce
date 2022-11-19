namespace AirForce
{
    public class PlayerBullet : GameObject
    {
        private static int playerBulletNumber = 20;
        
        private int speed = 15;
        
        public static Queue<PlayerBullet> Pull;

        static PlayerBullet()
        {
            Pull = new Queue<PlayerBullet>();

            for (int i = 0; i < playerBulletNumber; i++)
                Pull.Enqueue(new PlayerBullet(0,0));
        }

        public PlayerBullet(int x, int y)
        {
            sprite = Resource.player_shot;
            X = x;
            Y = y;
        }

        public override void Update()
        {
            if (X > Game.GameFieldWidth)
                Pull.Enqueue(new PlayerBullet(0,0));

            Move();
        }

        private void Move()
        {
            X += speed;
        }
    }
}


