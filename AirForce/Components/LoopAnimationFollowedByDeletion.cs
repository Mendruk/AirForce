using AirForce.Commands;

namespace AirForce.Components;

public class LoopAnimationFollowedByDeletion : Component
{
    protected List<GameObject> gameObjectsToAdd;
    protected List<GameObject> gameObjectsToRemove;

    public LoopAnimationFollowedByDeletion(GameObject gameObject, List<GameObject> gameObjectsToAdd,List<GameObject>gameObjectsToRemove) : base(gameObject)
    {
        this.gameObjectsToAdd = gameObjectsToAdd;
        this.gameObjectsToRemove = gameObjectsToRemove;
    }
     
    public override void Update(List<GameObject> gameObjects, Queue<ICommand> commands)
    {
        ICommand commandNextFrame = new CommandAnimationNextFrame(GameObject);
        commands.Enqueue(commandNextFrame);

        commandNextFrame.Execute();

        if (GameObject.CurrentFrameNumber >= GameObject.FrameNumber)
        {
            GameObject.CurrentFrameNumber = 0;

            ICommand command = new CommandDestroy(gameObjectsToAdd, gameObjectsToRemove, GameObject);
            commands.Enqueue(command);
            command.Execute();
        }
    }
}