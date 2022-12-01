using AirForce;

public class Game
{
    public static Random Random = new Random();
    public static List<GameObject> GameObjects = new();

    public int Health => playerShip.Health;
    public int Score;

    private Dictionary<CollisionTags, CollisionTags[]> collisions = new Dictionary<CollisionTags, CollisionTags[]>()
    {
        { CollisionTags.Player, new[] {CollisionTags.Bird, CollisionTags.EnemyBullet, CollisionTags.Enemy, CollisionTags.Ground } },
        { CollisionTags.PlayerBullet, new[]{ CollisionTags.Enemy, CollisionTags.Ground } },
        { CollisionTags.Enemy, new[] { CollisionTags.PlayerBullet, CollisionTags.Ground } }
    };

    private int gameFieldWidth;
    private int gameFieldHeight;

    private Rectangle ground;
    private int groundY;

    private PlayerShip playerShip;

    public bool isMoveUp, isMoveDown, isFire;

    public Game(int gameFieldWidth, int gameFieldHeight)
    {
        this.gameFieldHeight = gameFieldHeight;
        this.gameFieldWidth = gameFieldWidth;

        groundY = gameFieldHeight / 5 * 4;
        ground = new Rectangle(0, groundY, gameFieldWidth, gameFieldHeight);

        playerShip = new PlayerShip(100, 500);

        GameObjects.Add(playerShip);
    }

    public void Update()
    {
        if (Random.Next(10) == 5)
            CreateEnemy();

        int listCount = GameObjects.Count;

        GameObject gameObject1;
        GameObject gameObject2;

        for (int i = 0; i < listCount; i++)
        {
            gameObject1 = GameObjects[i];

            for (int j = i + 1; j < listCount; j++)
            {
                gameObject2 = GameObjects[j];

                if (gameObject1.DistanceToObject(gameObject2) < 100)
                {
                    if (!IsCollision(gameObject1.Tag, gameObject2.Tag))
                        continue;

                    int health1 = gameObject1.Health;
                    gameObject1.TakeDamage(gameObject2.Health);
                    gameObject2.TakeDamage(health1);
                }
            }

            gameObject1.Update();
        }

        if (isMoveUp)
            playerShip.MoveUp();

        if (isMoveDown)
            playerShip.MoveDown();

        if (isFire)
        {
            playerShip.Fire(GameObjects);
        }
    }

    public void Draw(Graphics graphics)
    {
        foreach (GameObject gameObject in GameObjects)
            gameObject.Draw(graphics);

        graphics.FillRectangle(Brushes.DarkSlateGray, ground);
    }

    private void CreateEnemy()
    {
        GameObjects.Add(new ChaserShip(gameFieldWidth, Random.Next(0, groundY)));
    }

    private bool IsCollision(CollisionTags tag1, CollisionTags tag2)
    {
        if (collisions.TryGetValue(tag1, out CollisionTags[] tags))
        {
            foreach (CollisionTags tag in tags)
                if (tag2 == tag)
                    return true;
        }
        return false;
    }
}