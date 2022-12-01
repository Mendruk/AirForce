namespace AirForce
{
    public class Explosion : GameObject
    {
        public Explosion(int x, int y)
        {
            Tag = CollisionTags.Effect;
            X = x;
            Y = y;
            sprite = Resource.explosion;
            frameNumber = 4;
            size = sprite.Height;
            
            CalculateFramesRectangles();
            Pull.Enqueue(this);
            GameObjects.Add(this);

            Reset();

        }
        protected override void Reset()
        {
            currentRealodedTime = 0;
            currentVerticalSpeed = 0;
            currnetFrameNumber = 0;
        }

        protected override void ChangeAnimationFrame()
        {
            currnetFrameNumber++;

            if (currnetFrameNumber >= frameNumber)
                Delite(this);
        }

        public override void TakeDamage(int damage)
        {
        }
    }
}
