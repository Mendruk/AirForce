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
        ICommand command = new CommandMove(0, speed, GameObject);
        commands.Enqueue(command);
        command.Execute();

    }
}