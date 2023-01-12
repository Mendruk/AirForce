namespace AirForce.Components;

public class LoopAnimationFollowedByDeletion : Component
{
    private readonly Action<GameObject> delete;
    private readonly GameObject gameObject;

    public LoopAnimationFollowedByDeletion(GameObject gameObject, Action<GameObject> delete) : base(gameObject)
    {
        this.delete = delete;
        this.gameObject = gameObject;
    }

    public override void Update(List<GameObject> gameObjects)
    {
        gameObject.CurrentFrameNumber += 1;

        if (GameObject.CurrentFrameNumber >= GameObject.FrameNumber)
            delete(GameObject);
    }
}