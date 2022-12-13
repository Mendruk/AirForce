using AirForce;
using AirForce.GameObjects;
using System.Drawing.Drawing2D;

public class Game
{
    private const int MaxEnemyNumber = 20;
    private int currentTimeToCreateEnemy;

    private readonly Random random = new();

    private readonly LinearGradientBrush backgroundGradientBrush;
    private readonly LinearGradientBrush groundGradientBrush;
    private readonly Font font = new(FontFamily.GenericSansSerif, 20, FontStyle.Bold);

    private readonly CollisionManager collisionManager;
    private readonly List<GameObject> gameObjects = new();
    private readonly PlayerShip playerShip;
    private readonly Rectangle ground;

    private readonly int gameFieldHeight;
    private readonly int gameFieldWidth;

    public bool IsMoveUp, IsMoveDown, IsFire;

    public Game(int gameFieldWidth, int gameFieldHeight)
    {
        backgroundGradientBrush = new LinearGradientBrush(
            new Point(0, 0),
            new Point(0, gameFieldHeight),
            Color.DarkSlateGray,
            Color.CadetBlue);

        ground = new Rectangle(0, gameFieldHeight * 4 / 5, gameFieldWidth, gameFieldHeight);

        groundGradientBrush = new LinearGradientBrush(
            new Point(0, ground.Y),
            new Point(0, gameFieldHeight),
            Color.DarkSlateGray,
            Color.CadetBlue);

        this.gameFieldHeight = gameFieldHeight;
        this.gameFieldWidth = gameFieldWidth;

        int playerStartX = gameFieldWidth / 8;
        int playerStartY = ground.Y / 2;

        playerShip = new PlayerShip(playerStartX, playerStartY) { IsEnable = true };
        gameObjects.Add(playerShip);

        for (int i = 0; i < MaxEnemyNumber; i++)
        {
            gameObjects.Add(new PlayerBullet());
            gameObjects.Add(new EnemyBullet());
            gameObjects.Add(new Explosion());
            gameObjects.Add(new ChaserShip());
            gameObjects.Add(new BomberShip());
            gameObjects.Add(new Meteor());
            gameObjects.Add(new Bird());
        }

        for (int i = 0; i < 50; i++)
            gameObjects.Add(new Star(random.Next(0, gameFieldWidth), random.Next(0, ground.Y)));

        collisionManager = new CollisionManager(gameObjects);
    }

    public int Score { get; private set; }

    public event EventHandler Defeat = delegate { };

    public void Update()
    {
        foreach ((GameObject? gameObject1, GameObject? gameObject2) in collisionManager.Collision())
            CollideGameObjects(gameObject1, gameObject2);

        int listCount = gameObjects.Count;

        for (int i = 1; i < listCount; i++)
        {
            GameObject gameObject = gameObjects[i];

            if (!gameObject.IsEnable)
                continue;

            gameObject.Move();

            if (gameObject.X < 0 ||
                gameObject.X > gameFieldWidth)
            {
                if (gameObject.Type == GameObjectType.Effect)
                    gameObject.X = gameFieldWidth;
                else
                    gameObject.IsEnable = false;
            }

            if (gameObject.Y + gameObject.Size / 2 >= ground.Y &&
                gameObject.Type != GameObjectType.Bird)
                TryDestroyByDamage(gameObject.Health, gameObject);

            if (gameObject is IDodgeble movable)
            {
                foreach (GameObject bullet in gameObjects
                             .Where(gameObject => gameObject.Type == GameObjectType.PlayerBullet)
                             .Where(bullet => bullet.IsEnable))
                    if (collisionManager.HasDodge(gameObject, bullet, out DodgeDirection direction))
                    {
                        if (direction == DodgeDirection.Up)
                            movable.DodgeUp();
                        else if (direction == DodgeDirection.Down)
                            movable.DodgeDown();
                        break;
                    }

                movable.UpdateDodge();
            }

            if (gameObject is IShootable shootable)
            {
                if (collisionManager.HasShoot(gameObject, playerShip) && playerShip.IsEnable)
                    shootable.Shoot(Create);

                shootable.UpdateReloadingTime();
            }
        }

        if(playerShip.IsEnable) 
            PlayerAct();

        currentTimeToCreateEnemy++;

        if (currentTimeToCreateEnemy < 30)
            return;

        CreateEnemy();
        currentTimeToCreateEnemy = 0;
    }
    //2//
    private void PlayerAct()
    {
        if (playerShip.Y + playerShip.Size / 2 >= ground.Y &&
            playerShip.Type != GameObjectType.Bird)
            TryDestroyByDamage(playerShip.Health, playerShip);

        playerShip.UpdateDodge();
        playerShip.UpdateReloadingTime();
        //

        if (IsMoveUp)
            playerShip.DodgeUp();

        if (IsMoveDown)
            playerShip.DodgeDown();

        if (IsFire)
            playerShip.Shoot(Create);
    }

    private void CollideGameObjects(GameObject gameObject1, GameObject gameObject2)
    {
        int gameObject1Health = gameObject1.Health;

        if ((TryDestroyByDamage(gameObject2.Health, gameObject1)
             && gameObject1.Type == GameObjectType.Enemy) ||
            (TryDestroyByDamage(gameObject1Health, gameObject2)
             && gameObject2.Type == GameObjectType.Enemy))
            Score++;
    }

    public void Draw(Graphics graphics)
    {
        graphics.FillRectangle(backgroundGradientBrush, 0, 0, gameFieldWidth, gameFieldHeight);
        graphics.FillRectangle(groundGradientBrush, ground);

        foreach (GameObject gameObject in gameObjects.Where(gameObject => gameObject.IsEnable &&
                                                                          gameObject.Type == GameObjectType.Effect))
            gameObject.Draw(graphics);

        foreach (GameObject gameObject in gameObjects.Where(gameObject => gameObject.IsEnable &&
                                                                          gameObject.Type != GameObjectType.Effect))
            gameObject.Draw(graphics);

        graphics.DrawString("Health: " + playerShip.Health, font, Brushes.CadetBlue, gameFieldWidth / 15,
            gameFieldHeight * 4 / 5);
        graphics.DrawString("Score: " + Score, font, Brushes.CadetBlue, gameFieldWidth / 5, gameFieldHeight * 4 / 5);
    }

    public void Restart()
    {
        foreach (GameObject gameObject in gameObjects.Where(gameObject => gameObject.IsEnable &&
                                                                          gameObject.Type != GameObjectType.Effect))
            gameObject.IsEnable = false;

        playerShip.Reset();
        playerShip.IsEnable = true;
        IsFire = false;
        IsMoveDown = IsMoveUp = false;
        Score = 0;
    }

    private void CreateEnemy()
    {
        int randomNumber = random.Next(1, 10);

        switch (randomNumber)
        {
            case 1:
                Create(typeof(Meteor), random.Next(gameFieldWidth / 4, gameFieldWidth), 0);
                break;
            case 2:
            case 3:
                Create(typeof(BomberShip), gameFieldWidth, random.Next(80, ground.Y - 80));
                break;
            case 4:
            case 5:
            case 6:
                Create(typeof(ChaserShip), gameFieldWidth, random.Next(30, ground.Y - 30));
                break;
            case 7:
            case 8:
            case 9:
            case 10:
                Create(typeof(Bird), gameFieldWidth, random.Next(gameFieldHeight / 2, ground.Y));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(randomNumber), randomNumber, null);
        }
    }

    //1//
    private void Create(Type type, int x, int y)
    {
        GameObject? createdGameObject = gameObjects.FirstOrDefault(gameObject => !gameObject.IsEnable &&
            gameObject.GetType() == type, null);
        if (createdGameObject == null)
            return;

        createdGameObject.IsEnable = true;
        createdGameObject.X = x;
        createdGameObject.Y = y;
    }

    private void Create(Type type, int x, int y, int size)
    {
        GameObject? createdGameObject = gameObjects.FirstOrDefault(gameObject => !gameObject.IsEnable &&
            gameObject.GetType() == type, null);
        if (createdGameObject == null)
            return;
        createdGameObject.Size = size;
        createdGameObject.IsEnable = true;
        createdGameObject.X = x;
        createdGameObject.Y = y;
    }
    //

    public bool TryDestroyByDamage(int damage, GameObject gameObject)
    {
        gameObject.Health -= damage;

        if (gameObject.Health > 0)
            return false;

        gameObject.IsEnable = false;
        gameObject.Reset();
        Create(typeof(Explosion), gameObject.X, gameObject.Y, gameObject.Size);

        if (gameObject == playerShip)
            Defeat(this, EventArgs.Empty);

        return true;
    }
}