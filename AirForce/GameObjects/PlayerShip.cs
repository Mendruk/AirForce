namespace AirForce;

public class PlayerShip : GameObject, IShootable, IDodgeble
{
    //IShootable
    private readonly Type bulletType = typeof(PlayerBullet);
    private readonly int maxVerticalSpeed = 10;
    private readonly int reloadedTime = 10;
    private readonly int startedX;
    private readonly int startedY;

    //IDodgeble
    private readonly int verticalAcceleration = 3;
    private int currentReloadedTime;
    private int currentVerticalSpeed;

    public PlayerShip(int startedX, int startedY) : base(Resource.player_ship)
    {
        Type = GameObjectType.Player;

        X = this.startedX = startedX;
        Y = this.startedY = startedY;

        maxHealth = Health = 10;
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

    public void Shoot(Action<Type, int, int> creationAction)
    {
        if (currentReloadedTime < reloadedTime)
            return;

        currentReloadedTime = 0;
        creationAction(bulletType, X + Size / 2, Y);
    }

    public override void Reset()
    {
        base.Reset();

        X = startedX;
        Y = startedY;
    }
}