namespace AirForce
{
    public class PlayerBullet : GameObject
    {
        public PlayerBullet(int x, int y)
        {
            Tag = CollisionTags.PlayerBullet;
            X = x;
            Y = y;
            Tag = CollisionTags.PlayerBullet;
            sprite = Resource.player_shot;
            size = sprite.Height;
            frameNumber = 3;
            CalculateFramesRectangles();

            Health = 1;
            horizontalSpeed = 15;
            maxVerticalSpeed = 0;
            currentVerticalSpeed = 0;
            acceleration = 0;
        }
    }
}


