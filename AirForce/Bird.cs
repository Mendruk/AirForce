namespace AirForce
{
    public class Bird : GameObject
    {
        public Bird(int x, int y)
        {
            Tag = CollisionTags.Bird;
            X = x;
            Y = y;
            sprite = Resource.bird;
            size = 50;
            frameNumber = 12;
            CalculateFramesRectangles();
            Pull.Enqueue(this);
            GameObjects.Add(this);

            horizontalSpeed = -10;
            currentVerticalSpeed = 5;
            maxVerticalSpeed = 8;
            acceleration = 4;

            Reset();
        }
        protected override void Reset()
        {
            Health = 1;
            currentVerticalSpeed = -3;
            currnetFrameNumber = 0;
        }

        public override void Update()
        {
            base.Update();

            currentVerticalSpeed += Game.Random.Next(-acceleration, acceleration);
        }
    }
}