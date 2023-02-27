namespace AirForce.Commands;

public class CommandAddScore : ICommand
{
    private int score;
    private readonly Game receiver;

    public CommandAddScore(int score, Game receiver)
    {
        this.receiver = receiver;
        this.score = score;
    }

    public void Execute()
    {
        receiver.Score += score;
    }

    public void Undo()
    {
        receiver.Score-=score;
    }
}