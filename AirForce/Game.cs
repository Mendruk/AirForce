using AirForce;

public class Game
{    
    public static List<GameObject> GameObjectList = new();

    public static int GameFieldWidth;
    public static int GameFieldHeight;
    
    private Rectangle ground;
    private PlayerShip playerShip;
    private ChaserShip chaserShip;

    public bool isMoveUp, isMoveDown, isFire;

    public Game(int gameFieldWidth, int gameFieldHeight)
    {
        GameFieldHeight=gameFieldHeight;
        GameFieldWidth=gameFieldWidth;

        ground = new Rectangle(0, gameFieldHeight / 5 * 4, gameFieldWidth, gameFieldHeight);

        playerShip = new PlayerShip();
        chaserShip = new ChaserShip();

        GameObjectList.Add(playerShip);
        GameObjectList.Add(chaserShip);
    }

    public void Update()
    {
        for (int i = 0; i < GameObjectList.Count; i++)
            GameObjectList[i].Update();

        if (isMoveUp)
            playerShip.MoveUp();
        
        if (isMoveDown)
            playerShip.MoveDown();

        if (isFire)
            playerShip.Fire();
    }

    public void Draw(Graphics graphics)
    {
        for (int i = 0; i < GameObjectList.Count; i++)
            GameObjectList[i].Draw(graphics);

        graphics.FillRectangle(Brushes.DarkSlateGray, ground);
    }
}