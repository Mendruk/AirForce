namespace AirForce;

public class BomberShip : GameObject, IShootable
{
    private readonly Type bulletType = typeof(EnemyBullet);
    private readonly int reloadedTime = 20;
    private int currentReloadedTime;

    public BomberShip() : base(Resource.bomber_ship)
    {
        Type = GameObjectType.Enemy;
        ConstHorizontalSpeed = random.Next(-8, -5);

        MaxHealth = Health = 3;
    }

    public void UpdateReloadingTime()
    {
        if (currentReloadedTime < reloadedTime)
            currentReloadedTime++;
    }

    public void Shoot(Action<Type, int, int> creationAction)
    {
        if (currentReloadedTime < reloadedTime)
            return;

        currentReloadedTime = 0;
        creationAction(bulletType, X - Size / 2, Y - Size / 3);
        creationAction(bulletType, X - Size / 2, Y + Size / 3);
    }
}