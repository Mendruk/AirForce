namespace AirForce.Components;

public class Dodge : Component
{
    private readonly int maxVerticalSpeed;
    private readonly int verticalAcceleration;
    private int currentVerticalSpeed;

    public Dodge(GameObject gameObject, int maxVerticalSpeed, int verticalAcceleration) : base(gameObject)
    {
        this.maxVerticalSpeed = maxVerticalSpeed;
        this.verticalAcceleration = verticalAcceleration;
    }

    public override void Update(List<GameObject> gameObjects)
    {
        UpdateDodge();
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

        GameObject.Y += currentVerticalSpeed;

        if (GameObject.Y < GameObject.Size / 2)
            GameObject.Y = GameObject.Size / 2;
    }
}