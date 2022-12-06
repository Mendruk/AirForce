namespace AirForce;

public class ChaserShip : GameObject
{
    public ChaserShip()
    {
        Tag = CollisionTags.Enemy;
        Sprite = Resource.chaser_ship;
        FrameNumber = 3;
        Size = Sprite.Height;

        CalculateFramesRectangles();
        Pull.Enqueue(this);
        GameObjects.Add(this);

        CanDodge = true;
        MaxVerticalSpeed = 5;
        CurrentVerticalSpeed = 1;
        Acceleration = 2;

        Reset();
    }

    public sealed override void Reset()
    {
        Health = 1;
        CurrentVerticalSpeed = 0;
        CurrentFrameNumber = 0;
        HorizontalSpeed = Random.Next(-10, -5);
    }
}