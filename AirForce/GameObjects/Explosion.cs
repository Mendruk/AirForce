namespace AirForce;

public class Explosion : GameObject
{
    public Explosion(int x, int y) : base(x, y, Resource.explosion)
    {
        Type = GameObjectType.Effect;
    }

    protected override void ChangeAnimationFrame(List<GameObject> gameObjectsToDelete)
    {
        CurrentFrameNumber += 1;

        if (CurrentFrameNumber >= FrameNumber)
        {
            gameObjectsToDelete.Add(this);
        }
    }
}