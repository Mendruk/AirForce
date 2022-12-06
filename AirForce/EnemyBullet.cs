namespace AirForce;

public class EnemyBullet : GameObject
{
    public EnemyBullet()
    {
        Tag = CollisionTags.EnemyBullet;
        Sprite = Resource.enemy_shot;
        FrameNumber = 3;
        Size = Sprite.Height;

        CalculateFramesRectangles();
        Pull.Enqueue(this);
        GameObjects.Add(this);

        HorizontalSpeed = -15;

        Reset();
    }

    public sealed override void Reset()
    {
        Health = 1;
        CurrentFrameNumber = 0;
    }
}