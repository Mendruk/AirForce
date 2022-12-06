namespace AirForce;

public class BomberShip : GameObject
{
    public BomberShip()
    {
        Tag = CollisionTags.Enemy;
        Sprite = Resource.bomber_ship;
        FrameNumber = 2;
        Size = Sprite.Height;

        CalculateFramesRectangles();
        Pull.Enqueue(this);
        GameObjects.Add(this);

        MaxVerticalSpeed = 5;
        CurrentVerticalSpeed = 1;
        Acceleration = 2;

        CanFire = true;
        ReloadedTime = 20;

        Reset();
    }

    public sealed override void Reset()
    {
        Health = 3;
        CurrentReloadedTime = 0;
        CurrentFrameNumber = 0;
        HorizontalSpeed = Random.Next(-6, -4);
    }

    public override void Update()
    {
        base.Update();

        if (CurrentReloadedTime < ReloadedTime)
            CurrentReloadedTime++;
    }
}