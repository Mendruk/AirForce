namespace AirForce;

public class BomberShip : GameObject, IShootable
{
    private readonly int reloadedTime = 20;
    private int currentReloadedTime;

    public BomberShip(int x, int y) : base(x, y, Resource.bomber_ship)
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

    public void Shoot(Action<GameObject> creationAction)
    {
        if (currentReloadedTime < reloadedTime)
            return;

        currentReloadedTime = 0;
        creationAction(new EnemyBullet(X - Size / 2, Y - Size / 3));
        creationAction(new EnemyBullet(X - Size / 2, Y + Size / 3));

    }
}