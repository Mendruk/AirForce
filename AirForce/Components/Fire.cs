namespace AirForce.Components;

public class Fire : Component
{
    protected readonly Action<int, int> CreateBullet;
    protected readonly int ReloadedTime;
    protected int CurrentReloadedTime;

    public Fire(GameObject gameObject, Action<int, int> createBullet, int reloadedTime) : base(gameObject)
    {
        ReloadedTime = reloadedTime;
        CreateBullet = createBullet;
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

        CreateBullet(GameObject.X + GameObject.Size / 2 , GameObject.Y);
    }

    private void UpdateReloadingTime()
    {
        if (CurrentReloadedTime < ReloadedTime)
            CurrentReloadedTime++;
    }
}