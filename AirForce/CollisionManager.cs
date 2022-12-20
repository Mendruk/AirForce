namespace AirForce;

public class CollisionManager
{
    private readonly Dictionary<GameObjectType, GameObjectType[]> collisions = new()
    {
        {
            GameObjectType.Meteor,
            new[] { GameObjectType.EnemyBullet, GameObjectType.Enemy, GameObjectType.Player, GameObjectType.PlayerBullet }
        },
        {
            GameObjectType.Player,
            new[] { GameObjectType.Bird, GameObjectType.EnemyBullet, GameObjectType.Enemy, GameObjectType.Meteor }
        },
        {
            GameObjectType.PlayerBullet,
            new[] { GameObjectType.Enemy, GameObjectType.Meteor }
        },
        {
            GameObjectType.Enemy,
            new[] { GameObjectType.PlayerBullet, GameObjectType.Meteor }
        },
        {
            GameObjectType.EnemyBullet,
            new[]
                { GameObjectType.Player, GameObjectType.Meteor }
        },
        {
            GameObjectType.Bird,
            new[] { GameObjectType.Player }
        }
    };

    private readonly List<GameObject> gameObjects;

    public CollisionManager(List<GameObject> gameObjects)
    {
        this.gameObjects = gameObjects;
    }

    public List<(GameObject gameObject1, GameObject gameObject2)> Collision()
    {
        List<(GameObject gameObject1, GameObject gameObject2)> collisionList = new();

        int listCount = gameObjects.Count;

        for (int i = 0; i < listCount; i++)
        {
            GameObject gameObject1 = gameObjects[i];

            for (int j = i + 1; j < listCount; j++)
            {
                GameObject gameObject2 = gameObjects[j];

                if (HasTouch(gameObject1, gameObject2) &&
                    HasCollision(gameObject1.Type, gameObject2.Type))
                    collisionList.Add((gameObject1, gameObject2));
            }
        }

        return collisionList;
    }

    private bool HasCollision(GameObjectType tag1, GameObjectType tag2)
    {
        return collisions.TryGetValue(tag1, out GameObjectType[] tags) &&
               tags.Any(tag => tag2 == tag);
    }

    private bool HasTouch(GameObject gameObject1, GameObject gameObject2)
    {
        return (int)Math.Sqrt(Math.Pow(gameObject1.X - gameObject2.X, 2) +
                              Math.Pow(gameObject1.Y - gameObject2.Y, 2)) <=
               gameObject1.Size / 2 + gameObject2.Size / 2;
    }

    public bool HasShoot(GameObject gameObject1, GameObject gameObject2)
    {
        return gameObject1.Y < gameObject2.Y + gameObject2.Size / 2 &&
               gameObject1.Y > gameObject2.Y - gameObject2.Size / 2;
    }

    public bool HasDodge(GameObject gameObject1, GameObject gameObject2, out DodgeDirection direction)
    {
        if (gameObject1.Y < gameObject2.Y)
            direction = DodgeDirection.Up;
        else
            direction = DodgeDirection.Down;

        return (int)Math.Sqrt(Math.Pow(gameObject1.X - gameObject2.X, 2) +
                              Math.Pow(gameObject1.Y - gameObject2.Y, 2)) <=
               gameObject1.Size + gameObject2.Size;
    }
}