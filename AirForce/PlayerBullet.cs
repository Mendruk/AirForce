namespace AirForce;

public class PlayerBullet : GameObject
{
    public PlayerBullet()
    {
        Tag = CollisionTags.PlayerBullet;
        Sprite = Resource.player_shot;
        FrameNumber = 3;
        Size = Sprite.Height;

        CalculateFramesRectangles();
        Pull.Enqueue(this);
        GameObjects.Add(this);

        HorizontalSpeed = 15;

        Reset();
    }

    public sealed override void Reset()
    {
        Health = 1;
        CurrentFrameNumber = 0;
    }
}