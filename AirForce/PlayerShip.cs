namespace AirForce;

public class PlayerShip : GameObject
{
    private readonly int startedX;
    private readonly int startedY;

    public PlayerShip(int startedX, int startedY)
    {       
        this.startedX = startedX;
        this.startedY = startedY;

        Tag = CollisionTags.Player;
        Sprite = Resource.player_ship;
        FrameNumber = 3;
        Size = Sprite.Height;

        CalculateFramesRectangles();
        Pull.Enqueue(this);
        GameObjects.Add(this);

        MaxVerticalSpeed = 12;
        Acceleration = 4;
        ReloadedTime = 10;

        Reset();
    }

    public sealed override void Reset()
    {
        X = startedX;
        Y = startedY;
        Health = 10;
        CurrentReloadedTime = 0;
        CurrentVerticalSpeed = 0;
        CurrentFrameNumber = 0;
    }

    public override void Update()
    {
        base.Update();

        if (CurrentReloadedTime < ReloadedTime)
            CurrentReloadedTime++;
    }

    public void MoveUp()
    {
        CurrentVerticalSpeed -= Acceleration;
    }

    public void MoveDown()
    {
        CurrentVerticalSpeed += Acceleration;
    }

    public override void Fire()
    {
        if (CurrentReloadedTime < ReloadedTime)
            return;

        Create(typeof(PlayerBullet), X + Size / 2, Y);
        CurrentReloadedTime = 0;
    }
}