using AirForce;

public class Game
{
    public static Random Random = new Random();

    public int Health => playerShip.Health;
    public int Score;

    private int maxEnemyNumber = 20;

    private Dictionary<CollisionTags, CollisionTags[]> collisions = new Dictionary<CollisionTags, CollisionTags[]>()
    {
        { CollisionTags.Player, new[] {CollisionTags.Bird, CollisionTags.EnemyBullet, CollisionTags.Enemy, CollisionTags.Meteor} },
        { CollisionTags.PlayerBullet, new[]{ CollisionTags.Enemy, CollisionTags.Meteor} },
        { CollisionTags.Enemy, new[] { CollisionTags.PlayerBullet, CollisionTags.Meteor } },
        { CollisionTags.Meteor, new[] { CollisionTags.EnemyBullet, CollisionTags.Enemy, CollisionTags.Ground ,CollisionTags.Player,CollisionTags.PlayerBullet} },
        { CollisionTags.Bird, new[] { CollisionTags.Player } }
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

        playerShip = new PlayerShip(200, 500) { isEnable = true };

        for (int i = 0; i < maxEnemyNumber; i++)
        {
            new PlayerBullet(0, 0);
            new EnemyBullet(0, 0);
            new Explosion(0, 0);
            new ChaserShip(0, 0);
            new BomberShip(0, 0);
            new Meteor(0, 0);
            new Bird(0, 0);
        }
    }

    public void Update()
    {
        //todo
        if (Random.Next(50) == 25)
            CreateEnemy();

        int listCount = GameObject.GameObjects.Count;

        GameObject gameObject1;
        GameObject gameObject2;

        for (int i = 0; i < listCount; i++)
        {
            gameObject1 = GameObject.GameObjects[i];

            if (!gameObject1.isEnable)
                continue;

            if (gameObject1.X < 0 || gameObject1.X > gameFieldWidth)
                GameObject.Delite(gameObject1);

            if (gameObject1.Y > groundY - gameObject1.size / 2)
                gameObject1.TakeDamage(gameObject1.Health);

            if (gameObject1.canFire &&
                gameObject1.Y < playerShip.Y + playerShip.size &&
                gameObject1.Y > playerShip.Y - playerShip.size)
                gameObject1.Fire();

            for (int j = i + 1; j < listCount; j++)
            {
                gameObject2 = GameObject.GameObjects[j];

                if (!gameObject2.isEnable)
                    continue;

                if (gameObject1.canDodge && gameObject2.Tag == CollisionTags.PlayerBullet &&
                    gameObject1.DistanceToObject(gameObject2) < gameObject1.size * 3)
                {
                    if (gameObject1.Y <= gameObject2.Y)
                        gameObject1.Dodge(Directions.Down);
                    else
                        gameObject1.Dodge(Directions.Up);
                }

                if (gameObject1.DistanceToObject(gameObject2) < gameObject1.size / 2 + gameObject2.size / 2)
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
            playerShip.Fire();
        }
    }

    public void Draw(Graphics graphics)
    {
        for (int i = 0; i < GameObject.GameObjects.Count; i++)
        {

            if (!GameObject.GameObjects[i].isEnable)
                continue;
            GameObject.GameObjects[i].Draw(graphics);
        }

        graphics.FillRectangle(Brushes.DarkSlateGray, ground);

    }

    private void CreateEnemy()
    {
        GameObject.Create(typeof(ChaserShip), gameFieldWidth, Random.Next(gameFieldHeight - groundY, groundY));
        GameObject.Create(typeof(BomberShip), gameFieldWidth, Random.Next(gameFieldHeight - groundY, groundY));
        GameObject.Create(typeof(Meteor), Random.Next(gameFieldWidth / 2, gameFieldWidth), 0);
        GameObject.Create(typeof(Bird),gameFieldWidth, Random.Next(gameFieldHeight/2, groundY-100));
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