using AirForce.Commands;

namespace AirForce.Components;

public class Dodge : Component
{
    private readonly int maxVerticalSpeed;
    private readonly int verticalAcceleration;
    public int currentVerticalSpeed;

    public Dodge(GameObject gameObject, int maxVerticalSpeed, int verticalAcceleration) : base(gameObject)
    {
        this.maxVerticalSpeed = maxVerticalSpeed;
        this.verticalAcceleration = verticalAcceleration;
    }

    public override void Update(List<GameObject> gameObjects, Queue<ICommand> commands)
    {
        UpdateDodge(commands);
    }

    public void DodgeUp(Queue<ICommand> commands)
    {
        ICommand command = new CommandAddVerticalSpeed(-verticalAcceleration, this);
        commands.Enqueue(command);
        command.Execute();
    }

    public void DodgeDown(Queue<ICommand> commands)
    {
        ICommand command = new CommandAddVerticalSpeed(verticalAcceleration, this);
        commands.Enqueue(command);
        command.Execute(); 
    }

    public void UpdateDodge(Queue<ICommand> commands)
    {
        if (currentVerticalSpeed > maxVerticalSpeed)
            currentVerticalSpeed = maxVerticalSpeed;

        if (currentVerticalSpeed < -maxVerticalSpeed)
            currentVerticalSpeed = -maxVerticalSpeed;

        if (currentVerticalSpeed > 0)
        {
            ICommand commandAddVertical = new CommandAddVerticalSpeed(-1, this);
            commands.Enqueue(commandAddVertical);
            commandAddVertical.Execute();
        }

        if (currentVerticalSpeed < 0)
        {
            ICommand commandAddVertical = new CommandAddVerticalSpeed(1, this);
            commands.Enqueue(commandAddVertical);
            commandAddVertical.Execute();
        }

        ICommand command = new CommandMove(0, currentVerticalSpeed, GameObject);
        command.Execute();

        if (GameObject.Y < GameObject.Size / 2)
        {
            GameObject.Y = GameObject.Size / 2;
            return;
        }

        commands.Enqueue(command);

    }
}