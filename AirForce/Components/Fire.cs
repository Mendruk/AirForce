namespace AirForce.Components;

public class Fire : Component
{
    protected readonly Action<int, int> CreateAction;
    protected readonly GameObject GameObject;
    protected readonly int ReloadedTime;
    protected int CurrentReloadedTime;

    public Fire(GameObject gameObject, Action<int, int> createAction, int reloadedTime) : base(gameObject)
    {
        this.GameObject = gameObject;
        this.ReloadedTime = reloadedTime;
        this.CreateAction = createAction;
    }

    public override void Update(List<GameObject> gameObjects)
    {
        UpdateReloadingTime();
    }

    public virtual void Shoot()
    {
        if (CurrentReloadedTime < ReloadedTime)
            return;

        CurrentReloadedTime = 0;

        CreateAction(GameObject.X + GameObject.Size / 2, GameObject.Y);
    }

    private void UpdateReloadingTime()
    {
        if (CurrentReloadedTime < ReloadedTime)
            CurrentReloadedTime++;
    }
}