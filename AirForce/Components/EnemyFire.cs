﻿namespace AirForce.Components;

public class EnemyFire : Fire
{
    public EnemyFire(GameObject gameObject, Action<int, int> createBullet, int reloadedTime) : base(gameObject,
        createBullet, reloadedTime)
    {
    }

    public override void Update(List<GameObject> gameObjects)
    {
        base.Update(gameObjects);

        foreach (GameObject obj in gameObjects)
        {
            if (obj.Type != GameObjectType.Player)
                continue;

            if (HasShoot( obj))
                Shoot();
        }
    }

    public override void Shoot()
    {
        if (CurrentReloadedTime < ReloadedTime)
            return;

        CurrentReloadedTime = 0;

        CreateBullet(GameObject.X - GameObject.Size / 2, GameObject.Y - GameObject.Size / 3);
        CreateBullet(GameObject.X - GameObject.Size / 2, GameObject.Y + GameObject.Size / 3);
    }

    private bool HasShoot(GameObject gameObjectOther)
    {
        return GameObject.Y < gameObjectOther.Y + gameObjectOther.Size / 2 &&
               GameObject.Y > gameObjectOther.Y - gameObjectOther.Size / 2;
    }
}