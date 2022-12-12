namespace AirForce;

public class ChaserShip : GameObject, IDodgeble
{
    private readonly int maxVerticalSpeed = 10;

    //IDodgeble
    private readonly int verticalAcceleration = 3;
    private int currentVerticalSpeed;

    public ChaserShip() : base(Resource.chaser_ship)
    {
        Type = GameObjectType.Enemy;
        ConstHorizontalSpeed = random.Next(-12, -8);

        maxHealth = Health = 1;
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
}