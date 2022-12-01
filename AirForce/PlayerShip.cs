namespace AirForce
{
    public class PlayerShip : GameObject
    {
        public PlayerShip(int x, int y)
        {
            Tag = CollisionTags.Player;
            X = x;
            Y = y;
            sprite = Resource.player_ship;
            size = sprite.Height;
            frameNumber = 3;
            CalculateFramesRectangles();

            Health = 10;
            maxVerticalSpeed = 12;
            acceleration = 4;
            reloadedTime = 10;
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

        public void Fire(List<GameObject> gameObjects)
        {
            if (currentRealodedTime >= reloadedTime)
            {
                gameObjects.Add(new PlayerBullet(X + size / 2, Y));
                currentRealodedTime = 0;
            }
        }
    }
}