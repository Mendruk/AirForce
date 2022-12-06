namespace AirForce;

public class Bird : GameObject
{
    public Bird()
    {
        Tag = CollisionTags.Bird;
        Sprite = Resource.bird;
        FrameNumber = 12;
        Size = 50;

        CalculateFramesRectangles();
        Pull.Enqueue(this);
        GameObjects.Add(this);

        CurrentVerticalSpeed = 5;
        MaxVerticalSpeed = 8;
        Acceleration = 4;

        Reset();
    }

    public sealed override void Reset()
    {
        Health = 1;
        CurrentVerticalSpeed = -3;
        CurrentFrameNumber = 0;
        HorizontalSpeed = -Random.Next(5, 10);
    }

    public override void Update()
    {
        base.Update();

        CurrentVerticalSpeed += Game.Random.Next(-Acceleration, Acceleration);
    }
}