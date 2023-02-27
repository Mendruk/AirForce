using AirForce.Commands;

namespace AirForce.Components;

public class LoopAnimation : Component
{
    public LoopAnimation(GameObject gameObject) : base(gameObject)
    {
    }

    public override void Update(List<GameObject> gameObjects, Queue<ICommand> commands)
    {
        if (GameObject.CurrentFrameNumber >= GameObject.FrameNumber-1)
            GameObject.CurrentFrameNumber = 0;

        GameObject.CurrentFrameNumber++;
    }
}