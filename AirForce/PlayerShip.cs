namespace AirForce
{
    public class PlayerShip : GameObject
    {
        public event EventHandler PlayerShipDestroyed = delegate { };

        public PlayerShip(int x, int y)
        {
            Tag = CollisionTags.Player;
            X = x;
            Y = y;
            sprite = Resource.player_ship;
            size = sprite.Height;
            frameNumber = 3;
            CalculateFramesRectangles();
            Pull.Enqueue(this);
            GameObjects.Add(this);

            maxVerticalSpeed = 12;
            acceleration = 4;
            reloadedTime = 10;

            Reset();
        }

        protected override void Reset()
        {
            Health = 10;
            currentRealodedTime = 0;
            currentVerticalSpeed = 0;
            currnetFrameNumber = 0;
        }

        public override void Update()
        {
            base.Update();

            if (currentRealodedTime < reloadedTime)
                currentRealodedTime++;
        }

        public void MoveUp()
        {
            currentVerticalSpeed -= acceleration;
        }

        public void MoveDown()
        {
            currentVerticalSpeed += acceleration;
        }

        public override void Fire()
        {
            if (currentRealodedTime >= reloadedTime)
            {
                Create(typeof(PlayerBullet), X + size / 2, Y);
                currentRealodedTime = 0;
            }
        }
    }
}