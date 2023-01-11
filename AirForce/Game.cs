using AirForce;
using AirForce.Components;
using System.Drawing.Drawing2D;

public class Game
{
    private readonly LinearGradientBrush backgroundGradientBrush;
    private readonly Font font = new(FontFamily.GenericSansSerif, 20, FontStyle.Bold);

    private readonly int gameFieldHeight;
    private readonly int gameFieldWidth;
    private readonly List<GameObject> gameObjects = new();
    private readonly List<GameObject> gameObjectsToAdd = new();
    private readonly List<GameObject> gameObjectsToDelete = new();

    private readonly Rectangle ground;
    private readonly LinearGradientBrush groundGradientBrush;
    private readonly Dodge playerDodgeComponent;
    private readonly Fire playerFireComponent;
    private readonly GameObject playerShip;

    private readonly Random random = new();
    private int currentTimeToCreateEnemy;
    public bool IsMoveUp, IsMoveDown, IsFire;

    public int Score;

    public Game(int gameFieldWidth, int gameFieldHeight)
    {
        this.gameFieldWidth = gameFieldWidth;
        this.gameFieldHeight = gameFieldHeight;

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

        playerShip = new GameObject(100, 100, Resource.player_ship, GameObjectType.Player, 10, new List<Component>());
        playerShip.Components.Add(new LoopAnimation(playerShip));
        playerDodgeComponent = new Dodge(playerShip, 10, 3);
        playerShip.Components.Add(playerDodgeComponent);
        playerFireComponent = new Fire(playerShip, CreatePlayerBullet, 10);
        playerShip.Components.Add(playerFireComponent);
        gameObjects.Add(playerShip);
    }

    public void Update()
    {
        foreach ((GameObject? gameObject1, GameObject? gameObject2) in CollisionManager.Collision(gameObjects))
            CollideGameObjects(gameObject1, gameObject2);

        foreach (GameObject gameObject in gameObjects) gameObject.Update(gameObjects);

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

        //if (playerShip.Y + playerShip.Size / 2 >= ground.Y &&
        //    playerShip.Type != GameObjectType.Bird)
        //    TryDestroyByDamage(playerShip.Health, playerShip);

        if (IsMoveUp)
            playerDodgeComponent.DodgeUp();

        if (IsMoveDown)
            playerDodgeComponent.DodgeDown();

        if (IsFire)
            playerFireComponent.Shoot();
    }

    public void Draw(Graphics graphics)
    {
        graphics.FillRectangle(backgroundGradientBrush, 0, 0, gameFieldWidth, gameFieldHeight);
        graphics.FillRectangle(groundGradientBrush, ground);

        foreach (GameObject gameObject in gameObjects) gameObject.Draw(graphics);

        graphics.DrawString("Health: " + playerShip.Health, font, Brushes.CadetBlue, gameFieldWidth / 15,
            gameFieldHeight * 4 / 5);
        graphics.DrawString("Score: " + Score, font, Brushes.CadetBlue, gameFieldWidth / 5, gameFieldHeight * 4 / 5);
    }

    private void Create(GameObject gameObject)
    {
        gameObjectsToAdd.Add(gameObject);
    }

    private void Delete(GameObject gameObject)
    {
        gameObjectsToDelete.Add(gameObject);
    }

    private void CreateRandomEnemy()
    {
        int randomNumber = random.Next(1, 10);

        switch (randomNumber)
        {
            case 1:
                GameObject meteor = new(gameFieldWidth, random.Next(30, gameFieldHeight - 30), Resource.asteroid,
                    GameObjectType.Meteor, 10, new List<Component>());
                meteor.Components.Add(new MoveHorizontal(meteor, -5));
                meteor.Components.Add(new MoveVertical(meteor, 5));
                Create(meteor);
                break;
            case 2:
            case 3:
                GameObject bomberShip = new(gameFieldWidth, random.Next(30, gameFieldHeight - 30), Resource.bomber_ship,
                    GameObjectType.Enemy, 3, new List<Component>());
                bomberShip.Components.Add(new LoopAnimation(bomberShip));
                bomberShip.Components.Add(new MoveHorizontal(bomberShip, -3));
                bomberShip.Components.Add(new EnemyFire(bomberShip, CreateEnemyBullet, 20));
                Create(bomberShip);
                break;
            case 4:
            case 5:
            case 6:
                GameObject chaserShip = new(gameFieldWidth, random.Next(30, gameFieldHeight - 30), Resource.chaser_ship,
                    GameObjectType.Enemy, 1, new List<Component>());
                chaserShip.Components.Add(new LoopAnimation(chaserShip));
                chaserShip.Components.Add(new MoveHorizontal(chaserShip, -5));
                chaserShip.Components.Add(new EnemyDodge(chaserShip, 10, 3));
                Create(chaserShip);
                break;
            case 7:
            case 8:
            case 9:
            case 10:
                GameObject bird = new(gameFieldWidth, random.Next(30, gameFieldHeight - 30), Resource.bird,
                    GameObjectType.Bird, 1, new List<Component>());
                bird.Components.Add(new LoopAnimation(bird));
                bird.Components.Add(new MoveHorizontal(bird, -5));
                bird.Components.Add(new EnemyDodge(bird, 10, 3));
                Create(bird);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(randomNumber), randomNumber, null);
        }
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

    public bool TryDestroyByDamage(int damage, GameObject gameObject)
    {
        gameObject.Health -= damage;

        if (gameObject.Health > 0)
            return false;

        GameObject explosion = new(gameObject.X, gameObject.Y, Resource.explosion, GameObjectType.Effect, 1,
            new List<Component>());

        explosion.Components.Add(new LoopAnimationFollowedByDeletion(explosion, Delete));
        Create(explosion);

        Delete(gameObject);

        return true;
    }

    //todo. !!!crutch!!!
    private void CreatePlayerBullet(int x, int y)
    {
        GameObject playerBullet =
            new(x, y, Resource.player_shot, GameObjectType.PlayerBullet, 1, new List<Component>());
        playerBullet.Components.Add(new MoveHorizontal(playerBullet, 10));

        Create(playerBullet);
    }

    private void CreateEnemyBullet(int x, int y)
    {
        GameObject enemyBullet =
            new(x, y, Resource.enemy_shot, GameObjectType.EnemyBullet, 1, new List<Component>());
        enemyBullet.Components.Add(new MoveHorizontal(enemyBullet, -10));

        Create(enemyBullet);
    }
}