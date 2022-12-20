namespace AirForce;

public class Explosion : GameObject
{
    public Explosion(int x, int y) : base(x, y, Resource.explosion)
    {
        Type = GameObjectType.Effect;
    }

    protected override void ChangeAnimationFrame()
    {
        CurrentFrameNumber += 1;

        if (CurrentFrameNumber >= FrameNumber)
        {
            CurrentFrameNumber = 0;
        }
    }
}