namespace AirForce
{
    public class ChaserShip : GameObject
    {
        public ChaserShip(int x, int y)
        {
            Tag = CollisionTags.Enemy;
            X = x;
            Y = y;
            Tag = CollisionTags.Enemy;
            sprite = Resource.chaser_ship;
            size = sprite.Height;
            CalculateFramesRectangles();

            //

            Health = 1;
            horizontalSpeed = Game.Random.Next(-15,-10);
            maxVerticalSpeed = 10;
            currentVerticalSpeed = 1;
            acceleration = 3;
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
