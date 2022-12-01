namespace AirForce
{
    public class BomberShip : GameObject
    {
        public BomberShip(int x, int y)
        {
            X = x;
            Y = y;
            Tag = CollisionTags.Enemy;
            frameNumber = 2;
            sprite = Resource.bomber_ship;
            size = sprite.Height;
            CalculateFramesRectangles();
            Pull.Enqueue(this);
            GameObjects.Add(this);

            //
            horizontalSpeed = Game.Random.Next(-6, -4);
            maxVerticalSpeed = 5;
            currentVerticalSpeed = 1;
            acceleration = 2;

            canFire = true;
            reloadedTime = 20;

            Reset();
        }

        protected override void Reset()
        {
            Health = 4;
            currentRealodedTime = 0;
            currnetFrameNumber = 0;
        }

        public override void Update()
        {
            base.Update();

            if (currentRealodedTime < reloadedTime)
                currentRealodedTime++;
        }
    }

}
