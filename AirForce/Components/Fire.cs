using AirForce.Commands;
using System;

namespace AirForce.Components;

public class Fire : Component
{
    protected readonly int ReloadedTime;
    protected int CurrentReloadedTime;

    protected List<GameObject> gameObjectsToAdd;
    protected List<GameObject> gameObjectsToRemove;
    protected Func<int,int,GameObject> getBullet;

    public Fire(List<GameObject> gameObjectsToAdd , List<GameObject> gameObjectsToRemove, GameObject gameObject, int reloadedTime, Func<int,int,GameObject> getBullet) 
        : base(gameObject)
    {
        ReloadedTime = reloadedTime;
        this.gameObjectsToAdd = gameObjectsToAdd;
        this.gameObjectsToRemove = gameObjectsToRemove;
        this.getBullet = getBullet;
    }

    public override void Update(List<GameObject> gameObjects, Queue<ICommand> commands)
    {
        UpdateReloadingTime();
    }

    public virtual void Shoot(Queue<ICommand> commands)
    {
        if (CurrentReloadedTime < ReloadedTime)
            return;

        CurrentReloadedTime = 0;

        commands.Enqueue(new CommandCreate(gameObjectsToAdd, gameObjectsToRemove,
            getBullet(GameObject.X + GameObject.Size / 2,GameObject.Y)));
    }

    private void UpdateReloadingTime()
    {
        if (CurrentReloadedTime < ReloadedTime)
            CurrentReloadedTime++;
    }
}