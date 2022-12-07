using AirForce;

public class Game
{
    public bool IsMoveUp, IsMoveDown, IsFire;
    public static Random Random = new();
    private const int MaxEnemyNumber = 20;

    private readonly Dictionary<CollisionTags, CollisionTags[]> collisions = new()
    {
        { CollisionTags.Meteor,       new[] { CollisionTags.EnemyBullet, CollisionTags.Enemy, CollisionTags.Player, CollisionTags.PlayerBullet } },
        { CollisionTags.Player,       new[] { CollisionTags.Bird, CollisionTags.EnemyBullet, CollisionTags.Enemy, CollisionTags.Meteor } },
        { CollisionTags.PlayerBullet, new[] { CollisionTags.Enemy, CollisionTags.Meteor } },
        { CollisionTags.Enemy,        new[] { CollisionTags.PlayerBullet, CollisionTags.Meteor } },
        { CollisionTags.EnemyBullet,  new[] { CollisionTags.Player, CollisionTags.Meteor } },
        { CollisionTags.Bird,         new[] { CollisionTags.Player } }
    };

    private int currentTimeToCreateEnemy;

    private readonly int gameFieldWidth;
    private readonly int gameFieldHeight;

    private readonly Ground ground;
    private readonly PlayerShip playerShip;

    public Game(int gameFieldWidth, int gameFieldHeight)
    {
        this.gameFieldHeight = gameFieldHeight;
        this.gameFieldWidth = gameFieldWidth;

        ground = new Ground(gameFieldWidth, gameFieldHeight);

        int playerStartX = gameFieldWidth / 7;
        int playerStartY = (gameFieldHeight - ground.Y) / 2;

        playerShip = new PlayerShip(playerStartX, playerStartY) { IsEnable = true };

        for (int i = 0; i < MaxEnemyNumber; i++)
        {
            new PlayerBullet();
            new EnemyBullet();
            new Explosion();
            new ChaserShip();
            new BomberShip();
            new Meteor();
            new Bird();
        }
    }

    public int Score { get; private set; }
    public int Health => playerShip.Health;

    public event EventHandler Defeat = delegate { };

    public void Update()
    {
        int listCount = GameObject.GameObjects.Count;

        for (int i = 0; i < listCount; i++)
        {
            GameObject gameObject1 = GameObject.GameObjects[i];

            if (!gameObject1.IsEnable)
                continue;

            if (gameObject1.X < 0 || gameObject1.X > gameFieldWidth)
                GameObject.Delete(gameObject1);

            if (gameObject1.Y > ground.Y - gameObject1.Size / 2 &&
                gameObject1.Tag != CollisionTags.Bird)
            {
                if (gameObject1.TryDestroyByDamage(gameObject1.Health) &&
                    gameObject1 == playerShip)
                    Defeat(this, EventArgs.Empty);

                continue;
            }

            if (gameObject1.CanFire &&
                gameObject1.Y < playerShip.Y + playerShip.Size &&
                gameObject1.Y > playerShip.Y - playerShip.Size)
                gameObject1.Fire();

            for (int j = i + 1; j < listCount; j++)
            {
                GameObject gameObject2 = GameObject.GameObjects[j];

                if (!gameObject2.IsEnable)
                    continue;

                if (gameObject1.CanDodge && gameObject2.Tag == CollisionTags.PlayerBullet &&
                    gameObject1.DistanceToObject(gameObject2) < gameObject1.Size * 3)
                    gameObject1.Dodge(gameObject1.Y <= gameObject2.Y ? Directions.Down : Directions.Up);

                if (gameObject1.DistanceToObject(gameObject2) <= gameObject1.Size / 2 + gameObject2.Size / 2 &&
                    HasCollision(gameObject1.Tag, gameObject2.Tag))
                    CollideGameObjects(gameObject1, gameObject2);
            }

            gameObject1.Update();
        }

        PlayerAct();

        currentTimeToCreateEnemy++;

        if (currentTimeToCreateEnemy < 30)
            return;

        CreateEnemy();
        currentTimeToCreateEnemy = 0;
    }

    private void PlayerAct()
    {
        if (IsMoveUp)
            playerShip.MoveUp();

        if (IsMoveDown)
            playerShip.MoveDown();

        if (IsFire) playerShip.Fire();

    }

    private void CollideGameObjects(GameObject gameObject1, GameObject gameObject2)
    {
        int gameObject1Health = gameObject1.Health;

        bool isDestroyedGameObject1 = gameObject1.TryDestroyByDamage(gameObject2.Health);
        bool isDestroyedGameObject2 = gameObject2.TryDestroyByDamage(gameObject1Health);

        if (isDestroyedGameObject1 && gameObject1 == playerShip)
            Defeat(this, EventArgs.Empty);

        if ((isDestroyedGameObject1 && gameObject1.Tag == CollisionTags.Enemy) ||
            (isDestroyedGameObject2 && gameObject2.Tag == CollisionTags.Enemy))
            Score++;
    }

    public void Draw(Graphics graphics)
    {
        foreach (GameObject gameObject in GameObject.GameObjects.Where(gameObject => gameObject.IsEnable))
            gameObject.Draw(graphics);

        ground.Draw(graphics);
    }

    public void Restart()
    {
        foreach (GameObject gameObject in GameObject.GameObjects.Where(gameObject => gameObject.IsEnable))
            GameObject.Delete(gameObject);

        playerShip.Reset();
        playerShip.IsEnable = true;
        IsFire = false;
        IsMoveDown = IsMoveUp = false;
        Score = 0;
    }

    private void CreateEnemy()
    {
        int randomNumber = Random.Next(1, 10);

        switch (randomNumber)
        {
            case 1:
                GameObject.Create(typeof(Meteor), Random.Next(gameFieldWidth / 4, gameFieldWidth), 0);
                break;
            case 2:
            case 3:
                GameObject.Create(typeof(BomberShip), gameFieldWidth, Random.Next(0, ground.Y));
                break;
            case 4:
            case 5:
            case 6:
                GameObject.Create(typeof(ChaserShip), gameFieldWidth, Random.Next(0, ground.Y));
                break;
            case 7:
            case 8:
            case 9:
            case 10:
                GameObject.Create(typeof(Bird), gameFieldWidth, Random.Next(gameFieldHeight / 2, ground.Y));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(randomNumber), randomNumber, null);
        }
    }

    private bool HasCollision(CollisionTags tag1, CollisionTags tag2)
    {
        return collisions.TryGetValue(tag1, out CollisionTags[] tags) &&
               tags.Any(tag => tag2 == tag);
    }
}