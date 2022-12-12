namespace AirForce;

public sealed class Meteor : GameObject
{
    public Meteor() : base(Resource.asteroid)
    {
        Type = GameObjectType.Meteor;

        ConstHorizontalSpeed = random.Next(-10,-5);
        ConstVerticalSpeed = random.Next(5,10);

        maxHealth = Health = random.Next(4,8);
        Size = Size * maxHealth / 8;
    }
}