﻿namespace AirForce;

public class Bird : GameObject
{
    public Bird() : base(Resource.bird)
    {
        Type = GameObjectType.Bird;
        Size = 50;
        ConstHorizontalSpeed = random.Next(-5, -3);

        MaxHealth = Health = 1;
    }

    public override int ConstVerticalSpeed
    {
        get => random.Next(-5, 5);
        protected set { }
    }
}