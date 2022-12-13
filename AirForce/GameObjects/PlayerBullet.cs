namespace AirForce;

public class PlayerBullet : GameObject
{
    public PlayerBullet() : base(Resource.player_shot)
    {
        Type = GameObjectType.PlayerBullet;
        ConstHorizontalSpeed = 10;

        MaxHealth = Health = 1;
    }
}