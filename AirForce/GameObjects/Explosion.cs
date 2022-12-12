namespace AirForce;

public class Explosion : GameObject
{
    public Explosion() : base(Resource.explosion)
    {
        Type = GameObjectType.Effect;
    }

    protected override void ChangeAnimationFrame()
    {

        CurrentFrameNumber += 1;

        if (CurrentFrameNumber >= FrameNumber)
        {
            CurrentFrameNumber = 0;
            IsEnable = false;
        }
    }
}