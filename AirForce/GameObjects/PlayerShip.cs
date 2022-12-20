namespace AirForce;

public class PlayerShip : GameObject, IShootable, IDodgeble
{
    private readonly int startedX;
    private readonly int startedY;

    private readonly int maxVerticalSpeed = 10;
    private readonly int reloadedTime = 10;

    private readonly int verticalAcceleration = 3;
    private int currentReloadedTime;
    private int currentVerticalSpeed;

    public PlayerShip(int startedX, int startedY) : base(startedX, startedY, Resource.player_ship)
    {
        Type = GameObjectType.Player;

        X = this.startedX = startedX;
        Y = this.startedY = startedY;

        MaxHealth = Health = 10;
    }

    public void DodgeUp()
    {
        currentVerticalSpeed -= verticalAcceleration;
    }

    public void DodgeDown()
    {
        currentVerticalSpeed += verticalAcceleration;
    }

    public void UpdateDodge()
    {
        if (currentVerticalSpeed > maxVerticalSpeed)
            currentVerticalSpeed = maxVerticalSpeed;

        if (currentVerticalSpeed < -maxVerticalSpeed)
            currentVerticalSpeed = -maxVerticalSpeed;

        if (currentVerticalSpeed > 0)
            currentVerticalSpeed -= 1;

        if (currentVerticalSpeed < 0)
            currentVerticalSpeed += 1;

        Y += currentVerticalSpeed;

        if (Y < Size / 2)
            Y = Size / 2;
    }

    public void UpdateReloadingTime()
    {
        if (currentReloadedTime < reloadedTime)
            currentReloadedTime++;
    }

    public void Shoot(Action<GameObject> creationAction)
    {
        if (currentReloadedTime < reloadedTime)
            return;

        currentReloadedTime = 0;

        creationAction(new PlayerBullet(X + Size / 2, Y));
    }


    public override void Reset()
    {
        base.Reset();

        X = startedX;
        Y = startedY;
    }
}