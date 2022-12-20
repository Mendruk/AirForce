namespace AirForce.GameObjects;

public class Star : GameObject
{
    public Star(int x, int y) : base(x, y,Resource.star)
    {
        Type = GameObjectType.Effect;
        ConstHorizontalSpeed = -3;
        CurrentFrameNumber = random.Next(FrameNumber);
    }
}