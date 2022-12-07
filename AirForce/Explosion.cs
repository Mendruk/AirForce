namespace AirForce;

public class Explosion : GameObject
{
    public Explosion()
    {
        Tag = CollisionTags.Effect;
        Sprite = Resource.explosion;
        FrameNumber = 4;
        Size = Sprite.Height;

        CalculateFramesRectangles();
        Pull.Enqueue(this);
        GameObjects.Add(this);

        Reset();
    }

   public sealed override void Reset()
    {
        CurrentFrameNumber = 0;
    }

    public override bool TryDestroyByDamage(int damage)
    {
        return true;
    }

    protected override void ChangeAnimationFrame()
    {
        CurrentFrameNumber++;

        if (CurrentFrameNumber >= FrameNumber)
            Delete(this);
    }
}