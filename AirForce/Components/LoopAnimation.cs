using AirForce.Commands;

namespace AirForce.Components;

public class LoopAnimation : Component
{
    public LoopAnimation(GameObject gameObject) : base(gameObject)
    {
    }

    public override void Update(List<GameObject> gameObjects, Queue<ICommand> commands)
    {
        ICommand command = new CommandAnimationNextFrame(GameObject);
        commands.Enqueue(command);

        command.Execute();

        if (GameObject.CurrentFrameNumber == GameObject.FrameNumber)
            GameObject.CurrentFrameNumber = 0;

        
    }
}