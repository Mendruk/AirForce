﻿using System.Drawing.Drawing2D;
using AirForce;
using AirForce.Commands;
using AirForce.Components;
public class Game
{   
    private readonly Random random = new();

    private readonly LinearGradientBrush backgroundGradientBrush;
    private readonly LinearGradientBrush groundGradientBrush;
    private readonly Font font = new(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
    private static readonly StringFormat Format = new();

    private readonly int gameFieldHeight;
    private readonly int gameFieldWidth;

    private readonly List<GameObject> gameObjects = new();
    private readonly List<GameObject> gameObjectsToAdd = new();
    private readonly List<GameObject> gameObjectsToRemove = new();
    private readonly CollisionManager collisionManager;
    private readonly GameObjectBuilder gameObjectBuilder;
    private readonly Rectangle ground;

    private readonly Queue<ICommand> commandsToExecute=new();
    private readonly Stack<List<ICommand>> commandsToUndo=new();

    private Dodge playerDodgeComponent;
    private Fire playerFireComponent;
    private GameObject playerShip;
    private int currentTimeToCreateEnemy;

    public GameState GameState = GameState.Play;
    public bool IsMoveUp, IsMoveDown, IsFire;
    public int Score;

    public Game(int gameFieldWidth, int gameFieldHeight)
    {
        Format.Alignment = StringAlignment.Center;

        this.gameFieldWidth = gameFieldWidth;
        this.gameFieldHeight = gameFieldHeight;
        ground = new Rectangle(0, gameFieldHeight * 4 / 5, gameFieldWidth, gameFieldHeight);

        backgroundGradientBrush = new LinearGradientBrush(

            new Point(0, 0),
            new Point(0, gameFieldHeight),
            Color.DarkSlateGray,
            Color.CadetBlue);

        groundGradientBrush = new LinearGradientBrush(
            new Point(0, ground.Y),
            new Point(0, gameFieldHeight),
            Color.DarkSlateGray,
            Color.CadetBlue);

        collisionManager = new CollisionManager();
        //Fragility
        gameObjectBuilder = new GameObjectBuilder(gameObjectsToAdd,gameObjectsToRemove);

        Restart();
    }

    public void Update()
    {
        switch (GameState)
        {
            case GameState.Play:
                PlayUpdate();
                break;
            case GameState.Rewind:
                RewindUpdate();
                break;
            case GameState.Defeat:
                RewindUpdate();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        EndOfUpdate();
    }

    //todo rename
    private void PlayUpdate()
    {
        foreach ((GameObject? gameObject1, GameObject? gameObject2) in collisionManager.Collision(gameObjects))
            CollideGameObjects(gameObject1, gameObject2);

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.Update(gameObjects, commandsToExecute);

            if (gameObject.Y + gameObject.Size / 2 >= ground.Y
                && gameObject.Type != GameObjectType.Bird
                && gameObject.Type != GameObjectType.Effect)
                TryDestroyByDamage(gameObject.Health, gameObject);
        }

        currentTimeToCreateEnemy++;

        if (currentTimeToCreateEnemy >= 30)
        {
            CreateRandomEnemy();
            currentTimeToCreateEnemy = 0;
        }

        if (IsMoveUp)
            playerDodgeComponent.DodgeUp();

        if (IsMoveDown)
            playerDodgeComponent.DodgeDown();

        if (IsFire)
            playerFireComponent.Shoot(commandsToExecute);

        List<ICommand> commandsOnFrame = new();

        for (int i = 0; i < commandsToExecute.Count; i++)
        {
            ICommand command = commandsToExecute.Dequeue();
            command.Execute();
            commandsOnFrame.Add(command);
        }

        commandsToUndo.Push(commandsOnFrame);
    }

    private void RewindUpdate()
    {
        if (commandsToUndo.Count == 0)
        {
            GameState = GameState.Play;
            Restart();
            return;
        }

        List<ICommand> commandsToUndoOnFrame = commandsToUndo.Pop();

        foreach (ICommand command in commandsToUndoOnFrame)
        {
            command.Undo();
        }
    }

    //todo rename
    private void EndOfUpdate()
    {
        gameObjects.AddRange(gameObjectsToAdd);
        gameObjectsToAdd.Clear();

        foreach (GameObject gameObjectToDelete in gameObjectsToRemove)
            gameObjects.Remove(gameObjectToDelete);

        gameObjectsToRemove.Clear();
    }

    public void Draw(Graphics graphics)
    {
        graphics.FillRectangle(backgroundGradientBrush, 0, 0, gameFieldWidth, gameFieldHeight);
        graphics.FillRectangle(groundGradientBrush, ground);

        foreach (GameObject gameObject in gameObjects) gameObject.Draw(graphics);

        switch (GameState )
        {
            case GameState.Play:        
                graphics.DrawString("Health: " + playerShip.Health, font, Brushes.CadetBlue, gameFieldWidth / 15, gameFieldHeight * 4 / 5); 
                graphics.DrawString("Score: " + Score, font, Brushes.CadetBlue, gameFieldWidth / 5, gameFieldHeight * 4 / 5);
                break;
            case GameState.Defeat:
                graphics.DrawString("You Lose.\n You`re score " + Score +"\nPress SPACE to start new game", font, Brushes.CadetBlue, gameFieldWidth /2, gameFieldHeight /2,Format);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(GameState), GameState, null);
        }
    }

    private void Create(GameObject gameObject)
    {
        gameObjectsToAdd.Add(gameObject);
    }

    private void Delete(GameObject gameObject)
    {
        gameObjectsToRemove.Add(gameObject);
    }

    private void CreateRandomEnemy()
    {
        int randomNumber = random.Next(1, 10);

        switch (randomNumber)
        {
            case 1:
                commandsToExecute.Enqueue(new CommandCreate(gameObjectsToAdd,gameObjectsToRemove,
                    gameObjectBuilder.GetMeteor(random.Next(0, gameFieldWidth / 2), 0)));
                break;
            case 2:
            case 3:
                commandsToExecute.Enqueue(new CommandCreate(gameObjectsToAdd, gameObjectsToRemove,
                    gameObjectBuilder.GetBomberShip(gameFieldWidth, random.Next(30, ground.Y))));
                break;
            case 4:
            case 5:
            case 6:
                commandsToExecute.Enqueue(new CommandCreate(gameObjectsToAdd, gameObjectsToRemove,
                    gameObjectBuilder.GetChaserShip(gameFieldWidth, random.Next(30, ground.Y))));
                break;
            case 7:
            case 8:
            case 9:
            case 10:
                commandsToExecute.Enqueue(new CommandCreate(gameObjectsToAdd, gameObjectsToRemove,
                    gameObjectBuilder.GetBird(gameFieldWidth, random.Next(ground.Y - 100, ground.Y))));
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
            commandsToExecute.Enqueue(new CommandAddScore(1,this));
    }

    public bool TryDestroyByDamage(int damage, GameObject gameObject)
    {

        commandsToExecute.Enqueue(new CommandTakeDamage(damage,gameObject));

        if (gameObject.Health > 0)
            return false;

        GameObject explosion = new(gameObject.X, gameObject.Y, Resource.explosion, GameObjectType.Effect, 1, new List<Component>());
        explosion.Components.Add(new LoopAnimationFollowedByDeletion(explosion, gameObjectsToAdd,gameObjectsToRemove));
        explosion.Size = gameObject.Size;
        commandsToExecute.Enqueue(new CommandCreate(gameObjectsToAdd,gameObjectsToRemove,explosion));

        commandsToExecute.Enqueue(new CommandDestroy(gameObjectsToAdd,gameObjectsToRemove,gameObject));

        if (gameObject.Type == GameObjectType.Player)
            GameState = GameState.Defeat;

        return true;
    }

    public void Restart()
    {
        gameObjects.Clear();
        gameObjectsToAdd.Clear();
        gameObjectsToRemove.Clear();
        Score = 0;

        //If you add it to the GameObjectBuilder, the playerShip control will break.
        playerShip = new GameObject(200, ground.Y/2, Resource.player_ship, GameObjectType.Player, 10, new List<Component>());
        playerShip.Components.Add(new LoopAnimation(playerShip));
        playerDodgeComponent = new Dodge(playerShip, 10, 3);
        playerShip.Components.Add(playerDodgeComponent);
        playerFireComponent = new Fire(gameObjectsToAdd,gameObjectsToRemove,playerShip,10,gameObjectBuilder.GetPlayerBullet);
        playerShip.Components.Add(playerFireComponent);
        gameObjects.Add(playerShip);
    }
}