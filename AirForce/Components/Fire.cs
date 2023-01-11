namespace AirForce.Components;

public class Fire : Component
{
    private readonly int reloadedTime;
    private readonly Action<int, int> createAction;
    private int currentReloadedTime;
    private readonly GameObject gameObject;

    public Fire(GameObject gameObject, Action<int, int> createAction, int reloadedTime) : base(gameObject)
    {
        this.gameObject = gameObject;
        this.reloadedTime = reloadedTime;
        this.createAction = createAction;
    }

    public override void Update(List<GameObject> gameObjects)
    {
        UpdateReloadingTime();
    }

    public void Shoot()
    {
        if (currentReloadedTime < reloadedTime)
            return;

        currentReloadedTime = 0;

        createAction(gameObject.X, gameObject.Y);
    }

    private void UpdateReloadingTime()
    {
        if (currentReloadedTime < reloadedTime)
            currentReloadedTime++;
    }
}