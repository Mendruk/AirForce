using AirForce.Commands;

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

    public override void Update(List<GameObject> gameObjects, Queue<ICommand> commands)
    {
        UpdateDodge(commands);
    }

    public void DodgeUp()
    {
        currentVerticalSpeed -= verticalAcceleration;
    }

    public void DodgeDown()
    {
        currentVerticalSpeed += verticalAcceleration;
    }

    public void UpdateDodge(Queue<ICommand> commands)
    {
        if (currentVerticalSpeed > maxVerticalSpeed)
            currentVerticalSpeed = maxVerticalSpeed;

        if (currentVerticalSpeed < -maxVerticalSpeed)
            currentVerticalSpeed = -maxVerticalSpeed;

        if (currentVerticalSpeed > 0)
            currentVerticalSpeed -= 1;

        if (currentVerticalSpeed < 0)
            currentVerticalSpeed += 1;

        ICommand command = new CommandMove(0, currentVerticalSpeed, GameObject);
        commands.Enqueue(command);
        command.Execute();

        if (GameObject.Y < GameObject.Size / 2)
            GameObject.Y = GameObject.Size / 2;
    }
}