namespace AirForce;

public class Meteor : GameObject
{
    public Meteor()
    {
        Tag = CollisionTags.Meteor;
        Sprite = Resource.asteroid;
        FrameNumber = 1;
        Size = Sprite.Height;

        CalculateFramesRectangles();
        Pull.Enqueue(this);
        GameObjects.Add(this);

        HorizontalSpeed = -10;
        MaxVerticalSpeed = 10;
        Acceleration = 2;

        Reset();
    }

    public sealed override void Reset()
    {
        int randomNumber = Random.Next(4, 8);
        Size = Sprite.Height * randomNumber / 8;
        Health = randomNumber;
        CurrentVerticalSpeed = 3;
        CurrentFrameNumber = 0;
    }

    public override void Update()
    {
        base.Update();

        CurrentVerticalSpeed += Acceleration;
    }
}