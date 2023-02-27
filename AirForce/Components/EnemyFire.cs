using AirForce.Commands;

namespace AirForce.Components;

public class EnemyFire : Fire
{
    public EnemyFire(List<GameObject> gameObjectsToAdd, List<GameObject> gameObjectsToRemove, GameObject gameObject, int reloadedTime, Func<int, int, GameObject> getBullet) 
        : base(gameObjectsToAdd, gameObjectsToRemove, gameObject, reloadedTime,getBullet)
    {
    }

    public override void Update(List<GameObject> gameObjects, Queue<ICommand> commands)
    {
        base.Update(gameObjects,commands);

        foreach (GameObject obj in gameObjects)
        {
            if (obj.Type != GameObjectType.Player)
                continue;

            if (HasShoot( obj))
                Shoot(commands);
        }
    }

    public override void Shoot(Queue<ICommand> commands)
    {
        if (CurrentReloadedTime < ReloadedTime)
            return;

        CurrentReloadedTime = 0;

        ICommand commandFirst = new CommandCreate(gameObjectsToAdd, gameObjectsToRemove,
            getBullet(GameObject.X - GameObject.Size / 2, GameObject.Y - GameObject.Size / 3));

        ICommand commandSecond = new CommandCreate(gameObjectsToAdd, gameObjectsToRemove,
            getBullet(GameObject.X - GameObject.Size / 2, GameObject.Y + GameObject.Size / 3));

        commands.Enqueue(commandFirst);
        commands.Enqueue(commandSecond);

        commandFirst.Execute();
        commandSecond.Execute();
    }

    private bool HasShoot(GameObject gameObjectOther)
    {
        return GameObject.Y < gameObjectOther.Y + gameObjectOther.Size / 2 &&
               GameObject.Y > gameObjectOther.Y - gameObjectOther.Size / 2;
    }
}