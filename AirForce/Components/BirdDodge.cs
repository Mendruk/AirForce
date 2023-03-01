using AirForce.Commands;

namespace AirForce.Components;

public class BirdDodge : Dodge
{
    private readonly Random random = new();

    public BirdDodge(GameObject gameObject, int maxVerticalSpeed, int verticalAcceleration) : base(gameObject, maxVerticalSpeed, verticalAcceleration)
    {
    }

    public override void Update(List<GameObject> gameObjects, Queue<ICommand> commands)
    {
        if (random.Next(0, 2) == 0)
        {
            DodgeDown(commands);
        }
        else
        {
            DodgeUp(commands);
        }

        UpdateDodge(commands);
    }
}