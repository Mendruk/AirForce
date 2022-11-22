using AirForce;

public class Game
{
    private Random random = new Random();
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

        playerShip = new PlayerShip
        {
            isEnable = true
        };

        GameObjectList.Add(playerShip);
    }

    public void Update()
    {
        if(random.Next(20)==10)
            CreateEnemy();

        int iMax = GameObjectList.Count;
        for (int i = 0; i < iMax; i++)
        {
            if(GameObjectList[i].isEnable)
                GameObjectList[i].Update();
        }

        if (isMoveUp)
            playerShip.MoveUp();
        
        if (isMoveDown)
            playerShip.MoveDown();

        if (isFire)
            playerShip.Fire();
    }

    public void Draw(Graphics graphics)
    {
        //todo
        int iMax = GameObjectList.Count;

        for (int i = 0; i < iMax; i++)
        {
            if (GameObjectList[i].isEnable)
                GameObjectList[i].Draw(graphics);
        }

        graphics.FillRectangle(Brushes.DarkSlateGray, ground);
    }

    private void CreateEnemy()
    {
        if(ChaserShip.Pull.Count==0)
            return;

        GameObject enemy = ChaserShip.Pull.Dequeue();

        enemy.X = GameFieldWidth+50;
        enemy.Y = random.Next(50, GameFieldHeight / 5 * 4);
        enemy.isEnable = true;
    }
}