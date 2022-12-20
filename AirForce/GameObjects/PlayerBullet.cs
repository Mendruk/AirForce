namespace AirForce;

public class PlayerBullet : GameObject
{
    public PlayerBullet(int x, int y) : base(x, y, Resource.player_shot)
    {
        Type = GameObjectType.PlayerBullet;
        ConstHorizontalSpeed = 10;

        MaxHealth = Health = 1;
    }
}