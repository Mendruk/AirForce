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

        }

        protected override void ChangeAnimationFrame()
        {
            currnetFrameNumber++;

            if (currnetFrameNumber >= frameNumber)
                Game.GameObjects.Remove(this);
        }
    }
}
