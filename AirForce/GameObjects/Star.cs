namespace AirForce.GameObjects;

public class Star : GameObject
{
    public Star(int x, int y) : base(Resource.star)
    {
        Type = GameObjectType.Effect;
        IsEnable = true;
        ConstHorizontalSpeed = -3;
        CurrentFrameNumber = random.Next(FrameNumber);

        X = x;
        Y = y;
    }
}