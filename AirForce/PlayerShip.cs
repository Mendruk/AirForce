namespace AirForce
{
    public class PlayerShip : Ship
    {
        public PlayerShip()
        {
            sprite = Resource.player_ship;
            size = sprite.Height;

            X = 100;
            Y = Game.GameFieldHeight / 2 - sprite.Height / 2;

            frameNumber = 2;

            Start();

            acceleration = 3;
            maxVerticalSpeed = 8;

            reloadingTime =10;
        }

        public void Fire()
        {
            if (currentReloadingTime < reloadingTime)
                return;

            currentReloadingTime = 0;

            PlayerBullet bullet = PlayerBullet.Pull.Dequeue();
            bullet.isEnable = true;

            bullet.X = X+size/2;
            bullet.Y = Y;
        }

        public void MoveUp()
        {
            currentVerticalSpeed -= acceleration;
        }

        public void MoveDown()
        {
            currentVerticalSpeed += acceleration;
        }

        protected override void Destroy()
        {
            throw new NotImplementedException();
        }
    }
}