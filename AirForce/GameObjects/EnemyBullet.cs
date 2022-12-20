namespace AirForce;

public class EnemyBullet : GameObject
{
    public EnemyBullet(int x, int y) : base(x, y, Resource.enemy_shot)
    {
        Type = GameObjectType.EnemyBullet;
        ConstHorizontalSpeed = -15;

        Health = 1;
    }
}