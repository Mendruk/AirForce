using AirForce.Commands;

namespace AirForce.Components;

public class EnemyDodge : Dodge
{
    public EnemyDodge(GameObject gameObject, int maxVerticalSpeed, int verticalAcceleration) : base(gameObject, maxVerticalSpeed, verticalAcceleration)
    {
    }

    public override void Update(List<GameObject> gameObjects, Queue<ICommand> commands)
    {
        foreach (GameObject obj in gameObjects)
        {
            if (obj.Type != GameObjectType.PlayerBullet ||
                !HasDodge(GameObject, obj, out DodgeDirection direction))
                continue;

            if (direction == DodgeDirection.Up)
                DodgeUp(commands);
            else if (direction == DodgeDirection.Down)
                DodgeDown(commands);
            break;
        }
        
        UpdateDodge(commands);
    }

    private bool HasDodge(GameObject gameObject1, GameObject gameObject2, out DodgeDirection direction)
    {
        if (gameObject1.Y < gameObject2.Y)
            direction = DodgeDirection.Up;
        else
            direction = DodgeDirection.Down;

        return gameObject1.GetDistanceTo(gameObject2) <= gameObject1.Size + gameObject2.Size;
    }
}