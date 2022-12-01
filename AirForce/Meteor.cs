namespace AirForce
{
    public class Meteor : GameObject
    {
        public Meteor(int x, int y)
        {
            Tag = CollisionTags.Meteor;
            X = x;
            Y = y;
            sprite = Resource.asteroid;
            size = sprite.Height;
            frameNumber = 4;
            CalculateFramesRectangles();
            Pull.Enqueue(this);
            GameObjects.Add(this);

            horizontalSpeed = -15;
            maxVerticalSpeed = 10 ;
            acceleration = 3;

            Reset();
        }
        protected override void Reset()
        {
            Health = 10;
            currentVerticalSpeed = 3;
            currnetFrameNumber = 0;
        }

        public override void Update()
        {
            base.Update();

            currentVerticalSpeed += acceleration;
        }
    }
}