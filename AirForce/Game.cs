using AirForce;
using System.Diagnostics.Metrics;
using System;
using System.Drawing.Drawing2D;
using static System.Formats.Asn1.AsnWriter;

public class Game
{
    private int currentTimeToCreateEnemy;
    public bool IsMoveUp, IsMoveDown, IsFire;

    private readonly Random random=new Random();

    private readonly int gameFieldHeight;
    private readonly int gameFieldWidth;

    private GameObject playerShip;
    private List<GameObject> gameObjects=new ();
    private List<GameObject> gameObjectsToAdd=new ();
    private List<GameObject> gameObjectsToDelete=new ();

    public int Score;

    public Game(int gameFieldWidth, int gameFieldHeight)
    {
        this.gameFieldWidth = gameFieldWidth;
        this.gameFieldHeight = gameFieldHeight;

        playerShip = new GameObject(100, 100, Resource.player_ship,GameObjectType.Player,10,new List<Component>());
        playerShip.Components.Add(new LoopAnimation(playerShip));
        gameObjects.Add(playerShip);
    }

    public void Update()
    {
        foreach ((GameObject? gameObject1, GameObject? gameObject2) in CollisionManager.Collision(gameObjects))
            CollideGameObjects(gameObject1, gameObject2);

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.Update(gameObjects);
        }

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

        //playerShip.UpdateDodge();
        //playerShip.UpdateReloadingTime();

        //if (IsMoveUp)
        //    playerShip.DodgeUp();

        //if (IsMoveDown)
        //    playerShip.DodgeDown();

        ////todo
        //if (IsFire)
        //    playerShip.Shoot(Create);
    }

    public void Draw(Graphics graphics)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.Draw(graphics);
        }
    }

    public void Create(GameObject gameObject)
    {
       gameObjectsToAdd.Add(gameObject);
    }

    public void Delete(GameObject gameObject)
    {
        gameObjectsToDelete.Add(gameObject);
    }

    private void CreateRandomEnemy()
    {
        int randomNumber = random.Next(1, 10);

        switch (randomNumber)
        {
            case 1:
                GameObject meteor = new(gameFieldWidth, random.Next(30, gameFieldHeight - 30), Resource.asteroid, GameObjectType.Meteor, 10, new List<Component>());
                meteor.Components.Add(new MoveHorizontal(meteor, -5));
                meteor.Components.Add(new MoveVertical(meteor, 5));
                Create(meteor);
                break;
            case 2:
            case 3:
                GameObject bomberShip = new(gameFieldWidth, random.Next(30, gameFieldHeight - 30), Resource.bomber_ship, GameObjectType.Enemy, 3, new List<Component>());
                bomberShip.Components.Add(new LoopAnimation(bomberShip));
                bomberShip.Components.Add(new MoveHorizontal(bomberShip, -3));
                Create(bomberShip);
                break;
            case 4:
            case 5:
            case 6: GameObject chaserShip = new (gameFieldWidth, random.Next(30, gameFieldHeight - 30), Resource.chaser_ship,GameObjectType.Enemy,1, new List<Component>());
                chaserShip.Components.Add(new LoopAnimation(chaserShip));
                chaserShip.Components.Add(new MoveHorizontal(chaserShip, -5));
                Create(chaserShip);
                break;
            case 7:
            case 8:
            case 9:
            case 10:
                //Create(new Bird(gameFieldWidth, random.Next(gameFieldHeight / 2, ground.Y)));
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

        GameObject explosion = new(gameObject.X, gameObject.Y, Resource.explosion, GameObjectType.Effect, 1,new List<Component>());

        explosion.Components.Add(new LoopAnimationFollowedByDeletion(explosion, Delete));
        Create(explosion);

        Delete(gameObject);

        return true;
    }
}