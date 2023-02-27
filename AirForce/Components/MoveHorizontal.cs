using AirForce.Commands;

namespace AirForce.Components;

public class MoveHorizontal : Component
{
    private readonly int speed;

    public MoveHorizontal(GameObject gameObject, int speed) : base(gameObject)
    {
        this.speed = speed;
    }

    public override void Update(List<GameObject> gameObjects, Queue<ICommand> commands)
    {
        ICommand command = new CommandMove(speed, 0, GameObject);
        commands.Enqueue(command);
        command.Execute();
    }
}