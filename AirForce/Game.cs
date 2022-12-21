using AirForce;
using System.Drawing.Drawing2D;

public class Game
{
    private int currentTimeToCreateEnemy;

    private readonly Random random = new();

    private readonly LinearGradientBrush backgroundGradientBrush;
    private readonly LinearGradientBrush groundGradientBrush;
    private readonly Font font = new(FontFamily.GenericSansSerif, 20, FontStyle.Bold);

    private readonly CollisionManager collisionManager;
    private readonly PlayerShip playerShip;
    private readonly Rectangle ground;

    private readonly List<GameObject> gameObjects = new();
    private readonly List<GameObject> gameObjectsToAdd = new();
    private readonly List<GameObject> gameObjectsToDelete = new();

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

        playerShip = new PlayerShip(playerStartX, playerStartY);
        gameObjects.Add(playerShip);

        collisionManager = new CollisionManager();
    }

    public int Score { get; private set; }

    public event EventHandler Defeat = delegate { };

    public void Update()
    {
        foreach ((GameObject? gameObject1, GameObject? gameObject2) in collisionManager.Collision(gameObjects))
            CollideGameObjects(gameObject1, gameObject2);

        int listCount = gameObjects.Count;

        for (int i = 1; i < listCount; i++)
        {
            GameObject gameObject = gameObjects[i];

            gameObject.Move();

            if (gameObject.X < 0 ||
                gameObject.X > gameFieldWidth)
            {
                if (gameObject.Type == GameObjectType.Effect)
                    gameObject.X = gameFieldWidth;
                else
                    gameObjectsToDelete.Add(gameObject);
            }

            if (gameObject.Y + gameObject.Size / 2 >= ground.Y &&
                gameObject.Type != GameObjectType.Bird)
                TryDestroyByDamage(gameObject.Health, gameObject);

            if (gameObject is IDodgeble movable)
            {
                foreach (GameObject bullet in gameObjects)
                {
                    if (bullet.Type != GameObjectType.PlayerBullet ||
                        !HasDodge(gameObject, bullet, out DodgeDirection direction))
                        continue;

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
                if (HasShoot(gameObject, playerShip))
                    shootable.Shoot(Create);

                shootable.UpdateReloadingTime();
            }
        }

        PlayerAct();

        currentTimeToCreateEnemy++;

        if (currentTimeToCreateEnemy >= 30)
        {
            CreateRandomEnemy();
            currentTimeToCreateEnemy = 0;
        }

        gameObjects.AddRange(gameObjectsToAdd);
        gameObjectsToAdd.Clear();

        foreach (GameObject gameObjectToDelete in gameObjectsToDelete)
            gameObjects.Remove(gameObjectToDelete);

        gameObjectsToDelete.Clear();
    }

    private void PlayerAct()
    {
        if (playerShip.Y + playerShip.Size / 2 >= ground.Y &&
            playerShip.Type != GameObjectType.Bird)
            TryDestroyByDamage(playerShip.Health, playerShip);

        playerShip.UpdateDodge();
        playerShip.UpdateReloadingTime();

        if (IsMoveUp)
            playerShip.DodgeUp();

        if (IsMoveDown)
            playerShip.DodgeDown();

        //todo
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

        foreach (GameObject gameObject in gameObjects)
            gameObject.Draw(graphics);

        graphics.DrawString("Health: " + playerShip.Health, font, Brushes.CadetBlue, gameFieldWidth / 15,
            gameFieldHeight * 4 / 5);
        graphics.DrawString("Score: " + Score, font, Brushes.CadetBlue, gameFieldWidth / 5, gameFieldHeight * 4 / 5);
    }

    public void Restart()
    {
        gameObjectsToDelete.AddRange(gameObjects);

        IsFire = false;
        IsMoveDown = false;
        IsMoveUp = false;
        Score = 0;
    }

    private void CreateRandomEnemy()
    {
        int randomNumber = random.Next(1, 10);

        switch (randomNumber)
        {
            case 1:
                Create(new Meteor(random.Next(gameFieldWidth / 4, gameFieldWidth), 0));
                break;
            case 2:
            case 3:
                Create(new BomberShip(gameFieldWidth, random.Next(80, ground.Y - 80)));
                break;
            case 4:
            case 5:
            case 6:
                Create(new ChaserShip(gameFieldWidth, random.Next(30, ground.Y - 30)));
                break;
            case 7:
            case 8:
            case 9:
            case 10:
                Create(new Bird(gameFieldWidth, random.Next(gameFieldHeight / 2, ground.Y)));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(randomNumber), randomNumber, null);
        }
    }

    public void Create(GameObject gameObject)
    {
        gameObjectsToAdd.Add(gameObject);
    }

    public bool TryDestroyByDamage(int damage, GameObject gameObject)
    {
        gameObject.Health -= damage;

        if (gameObject.Health > 0)
            return false;

        gameObjectsToDelete.Add(gameObject);
        //Create(new Explosion( gameObject.X, gameObject.Y));

        //if (gameObject == playerShip)
        //    Defeat(this, EventArgs.Empty);

        return true;
    }

    private bool HasShoot(GameObject gameObject1, GameObject gameObject2)
    {
        return gameObject1.Y < gameObject2.Y + gameObject2.Size / 2 &&
               gameObject1.Y > gameObject2.Y - gameObject2.Size / 2;
    }

    private bool HasDodge(GameObject gameObject1, GameObject gameObject2, out DodgeDirection direction)
    {
        if (gameObject1.Y < gameObject2.Y)
            direction = DodgeDirection.Up;
        else
            direction = DodgeDirection.Down;

        return gameObject1.GetDistanceTo(gameObject2)<= gameObject1.Size + gameObject2.Size;
    }
}