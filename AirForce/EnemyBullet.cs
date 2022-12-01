namespace AirForce
{
    public class EnemyBullet : GameObject
    {
        public EnemyBullet(int x, int y)
        {
            Tag = CollisionTags.EnemyBullet;
            X = x;
            Y = y;
            sprite = Resource.enemy_shot;
            size = sprite.Height;
            frameNumber = 3;
            CalculateFramesRectangles();
            Pull.Enqueue(this);
            GameObjects.Add(this);

            horizontalSpeed = -15;
            maxVerticalSpeed = 0;
            currentVerticalSpeed = 0;
            acceleration = 0;

            Reset();
        }

        protected override void Reset()
        {
            Health = 1;;
            currnetFrameNumber = 0;
        }

    }
}