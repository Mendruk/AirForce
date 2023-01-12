namespace AirForce.Components;

public class EnemyFire : Fire
{
    private readonly GameObject gameObject;

    public EnemyFire(GameObject gameObject, Action<int, int> createAction, int reloadedTime) : base(gameObject,
        createAction, reloadedTime)
    {
        this.gameObject = gameObject;
    }

    public override void Update(List<GameObject> gameObjects)
    {
        base.Update(gameObjects);

        foreach (GameObject obj in gameObjects)
        {
            if (obj.Type != GameObjectType.Player)
                continue;

            if (HasShoot(gameObject, obj))
                Shoot();
        }
    }

    public override void Shoot()
    {
        if (CurrentReloadedTime < ReloadedTime)
            return;

        CurrentReloadedTime = 0;

        CreateAction(gameObject.X - gameObject.Size / 2, gameObject.Y - gameObject.Size / 3);
        CreateAction(gameObject.X - gameObject.Size / 2, gameObject.Y + gameObject.Size / 3);
    }

    private bool HasShoot(GameObject gameObject1, GameObject gameObject2)
    {
        return gameObject1.Y < gameObject2.Y + gameObject2.Size / 2 &&
               gameObject1.Y > gameObject2.Y - gameObject2.Size / 2;
    }
}