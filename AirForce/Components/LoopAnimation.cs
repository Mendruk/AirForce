namespace AirForce.Components;

public class LoopAnimation : Component
{
    public LoopAnimation(GameObject gameObject) : base(gameObject)
    {
    }

    public override void Update(List<GameObject> gameObjects)
    {
        GameObject.CurrentFrameNumber += 1;

        if (GameObject.CurrentFrameNumber >= GameObject.FrameNumber)
            GameObject.CurrentFrameNumber = 0;
    }
}