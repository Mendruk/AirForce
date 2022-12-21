namespace AirForce;

public abstract class GameObject
{
    protected Random random = new();

    protected int CurrentFrameNumber;
    protected int FrameNumber;
    protected Bitmap Sprite;
    protected List<Rectangle> FrameRectangles;

    public int Health;
    public int Size;

    public int X;
    public int Y;

    public GameObjectType Type;

    protected GameObject(int x, int y, Bitmap sprite)
    {
        X = x;
        Y = y;
        Sprite = sprite;

        FrameNumber = sprite.Width / sprite.Height;
        Size = sprite.Height;
        FrameRectangles = EnumerateFramesRectangles().ToList();
    }

    public int ConstHorizontalSpeed { get; protected set; }
    public virtual int ConstVerticalSpeed { get; protected set; }

    public void Draw(Graphics graphics)
    {
        Rectangle rectangle = new(X - Size / 2, Y - Size / 2, Size, Size);
        graphics.DrawImage(Sprite, rectangle, FrameRectangles[CurrentFrameNumber], GraphicsUnit.Pixel);
    }

    protected virtual void ChangeAnimationFrame(List<GameObject> gameObjectsToDelete)
    {
        CurrentFrameNumber += 1;

        if (CurrentFrameNumber >= FrameNumber)
            CurrentFrameNumber = 0;
    }

    public void Move()
    {
        X += ConstHorizontalSpeed;
        Y += ConstVerticalSpeed;
    }

    public void Update(List<GameObject> gameObjects, List<GameObject> gameObjectsToDelete, Action<GameObject> creationAction)
    {
        Move();

        ChangeAnimationFrame(gameObjectsToDelete);

        if (this is IDodgeble dodgeble)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.Type != GameObjectType.PlayerBullet ||
                    !HasDodge(this, gameObject, out DodgeDirection direction))
                    continue;

                if (direction == DodgeDirection.Up)
                    dodgeble.DodgeUp();
                else if (direction == DodgeDirection.Down)
                    dodgeble.DodgeDown();
                break;
            }

            dodgeble.UpdateDodge();
        }

        if (this is IShootable shootable)
        {
            if (HasShoot(this, gameObjects[0]))
                shootable.Shoot(creationAction);

            shootable.UpdateReloadingTime();
        }
    }

    private IEnumerable<Rectangle> EnumerateFramesRectangles()
    {
        for (int i = 0; i < FrameNumber; i++)
            yield return new Rectangle(i * Sprite.Width / FrameNumber, 0,
                Sprite.Width / FrameNumber, Sprite.Height);
    }

    public int GetDistanceTo(GameObject gameObject)
    {
        return (int)Math.Sqrt(Math.Pow(X - gameObject.X, 2) +
                       Math.Pow(Y - gameObject.Y, 2));
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

        return gameObject1.GetDistanceTo(gameObject2) <= gameObject1.Size + gameObject2.Size;
    }
}