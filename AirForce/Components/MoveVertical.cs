using AirForce.Commands;

namespace AirForce.Components;

public class MoveVertical : Component
{
    private readonly int speed;

    public MoveVertical(GameObject gameObject, int speed) : base(gameObject)
    {
        this.speed = speed;
    }

    public override void Update(List<GameObject> gameObjects, Queue<ICommand> commands)
    {
        commands.Enqueue(new CommandMove(0, speed, GameObject));
    }
}