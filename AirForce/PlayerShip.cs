namespace AirForce
{
    public class PlayerShip : Ship
    {
        public PlayerShip()
        {
            sprite = Resource.player_ship;

            X = 100;
            Y = Game.GameFieldHeight / 2 - sprite.Height / 2;

            acceleration = 2;
            maxVerticalSpeed = 12;

            reloadingTime = 5;
        }

        public override void Fire()
        {
            if (currentReloadingTime < reloadingTime)
                return;

            currentReloadingTime = 0;

            PlayerBullet bullet = PlayerBullet.Pull.Dequeue();

            bullet.X = X + sprite.Width;
            bullet.Y = Y + sprite.Height / 2;

            Game.GameObjectList.Add(bullet);
        }

        public void MoveUp()
        {
            currentVerticalSpeed -= acceleration;
        }

        public void MoveDown()
        {
            currentVerticalSpeed += acceleration;
        }
    }
}