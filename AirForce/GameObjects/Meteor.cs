namespace AirForce;

public sealed class Meteor : GameObject
{
    public Meteor(int x, int y) : base(x, y, Resource.asteroid)
    {
        Type = GameObjectType.Meteor;

        ConstHorizontalSpeed = random.Next(-10, -5);
        ConstVerticalSpeed = random.Next(5, 10);

        Health = random.Next(4, 8);
        Size = Size * Health / 8;
    }
}