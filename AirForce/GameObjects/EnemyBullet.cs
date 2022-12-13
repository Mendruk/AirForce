namespace AirForce;

public class EnemyBullet : GameObject
{
    public EnemyBullet() : base(Resource.enemy_shot)
    {
        Type = GameObjectType.EnemyBullet;
        ConstHorizontalSpeed = -15;

        MaxHealth = Health = 1;
    }
}