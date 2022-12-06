using AirForce;

public class Game
{
    public static Random Random = new Random();

    public bool isMoveUp, isMoveDown, isFire;

    public int Health => playerShip.Health;
    public int Score;

    private int maxEnemyNumber = 20;

    //try rename
    private int currentTimeToCreateEnemy;

    private readonly Dictionary<CollisionTags, CollisionTags[]> collisions = new()
    {
        { CollisionTags.Player, new[] { CollisionTags.Bird, CollisionTags.EnemyBullet, CollisionTags.Enemy, CollisionTags.Meteor } },
        { CollisionTags.PlayerBullet, new[] { CollisionTags.Enemy, CollisionTags.Meteor } },
        { CollisionTags.Enemy, new[] { CollisionTags.PlayerBullet, CollisionTags.Meteor } },
        { CollisionTags.Meteor, new[] { CollisionTags.EnemyBullet, CollisionTags.Enemy, CollisionTags.Player, CollisionTags.PlayerBullet } },
        { CollisionTags.Bird, new[] { CollisionTags.Player } }
    };

    private int gameFieldWidth, gameFieldHeight;

    private int playerStartX, playerStartY;

    private Rectangle ground;
    private int groundY;

    private PlayerShip playerShip;

    public event EventHandler Defeat = delegate { };

    public Game(int gameFieldWidth, int gameFieldHeight)
    {
        this.gameFieldHeight = gameFieldHeight;
        this.gameFieldWidth = gameFieldWidth;

        playerStartX = gameFieldWidth / 7;
        playerStartY = (gameFieldHeight - groundY) / 2;

        groundY = gameFieldHeight / 5 * 4;
        ground = new Rectangle(0, groundY, gameFieldWidth, gameFieldHeight);

        playerShip = new PlayerShip(playerStartX, playerStartY) { isEnable = true };

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
        currentTimeToCreateEnemy++;

        if (currentTimeToCreateEnemy >= 30)
        {
            CreateEnemy();
            currentTimeToCreateEnemy = 0;
        }


        int listCount = GameObject.GameObjects.Count;

        GameObject gameObject1;
        GameObject gameObject2;

        for (int i = 0; i < listCount; i++)
        {
            gameObject1 = GameObject.GameObjects[i];

            if (!gameObject1.isEnable)
                continue;

            if (gameObject1.X < 0 || gameObject1.X > gameFieldWidth)
                GameObject.Delete(gameObject1);

            if (gameObject1.Y > groundY - gameObject1.size / 2)
            {
                gameObject1.IsDestroyedIfTakeDamage(gameObject1.Health, out bool isDestroyed);
                if (isDestroyed && gameObject1 == playerShip)
                    Defeat(this, EventArgs.Empty);
            }

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

                if (gameObject1.DistanceToObject(gameObject2) >= gameObject1.size / 2 + gameObject2.size / 2)
                    continue;

                if (!IsCollision(gameObject1.Tag, gameObject2.Tag))
                    continue;

                int health1 = gameObject1.Health;

                gameObject1.IsDestroyedIfTakeDamage(gameObject2.Health, out bool isDestroyed1);
                gameObject2.IsDestroyedIfTakeDamage(health1, out bool isDestroyed2);

                if (isDestroyed1 && gameObject1 == playerShip)
                    Defeat(this, EventArgs.Empty);

                if ((isDestroyed1 && gameObject1.Tag == CollisionTags.Enemy) ||
                    isDestroyed2 && gameObject2.Tag == CollisionTags.Enemy)
                    Score++;
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
        foreach (GameObject gameObject in GameObject.GameObjects.Where(gameObject => gameObject.isEnable))
        {
            gameObject.Draw(graphics);
        }

        graphics.FillRectangle(Brushes.DarkSlateGray, ground);
    }

    public void Restart()
    {
        foreach (GameObject gameObject in GameObject.GameObjects.Where(gameObject => gameObject.isEnable))
            gameObject.IsDestroyedIfTakeDamage(gameObject.Health, out bool isDestroyed); 

        playerShip.Health = 10;
        playerShip.isEnable = true;
        playerShip.X = playerStartX;
        playerShip.Y = playerStartY;
    }

    private void CreateEnemy()
    {
        int randomNumber = Random.Next(1, 10);

        switch (randomNumber)
        {
            case 1:
                GameObject.Create(typeof(Meteor), Random.Next(gameFieldWidth / 2, gameFieldWidth), 0);
                break;
            case 2:
            case 3:
                GameObject.Create(typeof(BomberShip), gameFieldWidth, Random.Next(gameFieldHeight - groundY, groundY));
                break;
            case 4:
            case 5:
            case 6:
                GameObject.Create(typeof(ChaserShip), gameFieldWidth, Random.Next(gameFieldHeight - groundY, groundY));
                break;
            case 7:
            case 8:
            case 9:
            case 10:
                GameObject.Create(typeof(Bird), gameFieldWidth, Random.Next(gameFieldHeight / 2, groundY - 100));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(randomNumber), randomNumber, null);
        }
    }

    private bool IsCollision(CollisionTags tag1, CollisionTags tag2)
    {
        return collisions.TryGetValue(tag1, out CollisionTags[] tags) && tags.Any(tag => tag2 == tag);
    }
}