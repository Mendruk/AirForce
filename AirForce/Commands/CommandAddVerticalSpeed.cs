using AirForce.Components;

namespace AirForce.Commands;

public class CommandAddVerticalSpeed : ICommand
{
    private readonly int deltaSpeed;

    private readonly Dodge receiver;


    public CommandAddVerticalSpeed(int deltaSpeed, Dodge receiver)
    {
        this.deltaSpeed = deltaSpeed;
        this.receiver = receiver;
    }

    public void Execute()
    {
        receiver.currentVerticalSpeed += deltaSpeed;
    }

    public void Undo()
    {
        receiver.currentVerticalSpeed -= deltaSpeed;
    }
}