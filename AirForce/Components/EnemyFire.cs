namespace AirForce.Components;

public class EnemyFire : Fire
{
    private GameObject gameObject;

    public EnemyFire(GameObject gameObject, Action<int, int> createAction, int reloadedTime) : base(gameObject, createAction, reloadedTime)
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

    private bool HasShoot(GameObject gameObject1, GameObject gameObject2)
    {
        return gameObject1.Y < gameObject2.Y + gameObject2.Size / 2 &&
               gameObject1.Y > gameObject2.Y - gameObject2.Size / 2;
    }
}