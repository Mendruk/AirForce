﻿namespace AirForce
{
    public class PlayerBullet : GameObject
    {
        public PlayerBullet(int x, int y)
        {
            Tag = CollisionTags.PlayerBullet;
            X = x;
            Y = y;
            sprite = Resource.player_shot;
            size = sprite.Height;
            frameNumber = 3;
            CalculateFramesRectangles();
            Pull.Enqueue(this);
            GameObjects.Add(this);

            Health = 1;
            horizontalSpeed = 15;
            maxVerticalSpeed = 0;
            currentVerticalSpeed = 0;
            acceleration = 0;
        }

        protected override void Reset()
        {
            Health = 1;
            currnetFrameNumber = 0;
        }
    }
}


