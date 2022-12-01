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
            frameNumber = 3;
            sprite = Resource.chaser_ship;
            size = sprite.Height;
            CalculateFramesRectangles();
            Pull.Enqueue(this);
            GameObjects.Add(this);

            horizontalSpeed = Game.Random.Next(-10,-5);
            maxVerticalSpeed = 5;
            currentVerticalSpeed = 1;
            acceleration = 2;

            canDodge = true;

            Reset();

        }

        protected override void Reset()
        {
            Health = 1;
            currentVerticalSpeed = 0;
            currnetFrameNumber = 0;
        }
    }
}
