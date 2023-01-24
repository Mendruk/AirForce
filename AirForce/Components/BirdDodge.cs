namespace AirForce.Components;

public class BirdDodge : Dodge
{
    private readonly Random random = new();

    public BirdDodge(GameObject gameObject, int maxVerticalSpeed, int verticalAcceleration) : base(gameObject, maxVerticalSpeed, verticalAcceleration)
    {
    }

    public override void Update(List<GameObject> gameObjects)
    {
        if (random.Next(0, 2) == 0)
        {
            DodgeDown();
        }
        else
        {
            DodgeUp();
        }

        UpdateDodge();
    }
}