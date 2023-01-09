namespace AirForce;

public static class CollisionManager
{
    private static readonly Dictionary<GameObjectType, GameObjectType[]> collisions = new()
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

    public static List<(GameObject gameObject1, GameObject gameObject2)> Collision(List<GameObject> gameObjects)
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
                    HasCollisionBetweenTypes(gameObject1.Type, gameObject2.Type))
                    collisionList.Add((gameObject1, gameObject2));
            }
        }

        return collisionList;
    }

    private static bool HasCollisionBetweenTypes(GameObjectType tag1, GameObjectType tag2)
    {
        return collisions.TryGetValue(tag1, out GameObjectType[] tags) &&
               tags.Any(tag => tag2 == tag);
    }

    private static bool HasTouch(GameObject gameObject1, GameObject gameObject2)
    {
        return gameObject1.GetDistanceTo(gameObject2) <= gameObject1.Size / 2 + gameObject2.Size / 2;
    }
}