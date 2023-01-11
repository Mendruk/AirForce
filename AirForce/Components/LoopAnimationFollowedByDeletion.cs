namespace AirForce.Components;

public class LoopAnimationFollowedByDeletion : Component
{
    private readonly Action<GameObject> delete;

    public LoopAnimationFollowedByDeletion(GameObject gameObject, Action<GameObject> delete) : base(gameObject)
    {
        this.delete = delete;
    }

    public override void Update(List<GameObject> gameObjects)
    {
        GameObject.CurrentFrameNumber += 1;

        if (GameObject.CurrentFrameNumber >= GameObject.FrameNumber)
            delete(GameObject);
    }
}