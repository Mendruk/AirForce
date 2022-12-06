namespace AirForce;

public abstract class GameObject
{
    public static Queue<GameObject> Pull = new();
    public static List<GameObject> GameObjects = new();

    protected static readonly Random Random = new();

    public bool IsEnable;

    public int Health { get; protected set; }
    public int X { get; protected set; }
    public int Y { get; protected set; }
    public CollisionTags Tag { get; protected set; }
    public int Size { get; protected set; }

    public bool CanDodge { get; protected set; }
    public bool CanFire { get; protected set; }

    protected int ReloadedTime;
    protected int CurrentReloadedTime;

    protected int CurrentVerticalSpeed;
    protected int MaxVerticalSpeed;
    protected int HorizontalSpeed;
    protected int Acceleration;

    protected List<Rectangle> FrameRectangles;
    protected int CurrentFrameNumber;
    protected int FrameNumber = 1;

    protected Bitmap Sprite;

    /////////////////////////////////////TODO////////////////////////////////////////
    public static GameObject Create(Type type, int x, int y)
    {
        while (true)
        {
            GameObject obj = Pull.Dequeue();

            if (obj.GetType() == type)
            {
                obj.IsEnable = true;
                obj.X = x;
                obj.Y = y;
                obj.Reset();
                return obj;
            }

            Pull.Enqueue(obj);
        }
    }

    public static GameObject Create(Type type, int x, int y, int size)
    {
        while (true)
        {
            GameObject obj = Pull.Dequeue();

            if (obj.GetType() == type)
            {
                obj.IsEnable = true;
                obj.X = x;
                obj.Y = y;
                obj.Size = size;
                obj.Reset();
                return obj;
            }

            Pull.Enqueue(obj);
        }
    }
    /////////////////////////////////////////////////////////////////////////////

    public static void Delete(GameObject gameObject)
    {
        gameObject.IsEnable = false;
        Pull.Enqueue(gameObject);
    }

    public void Draw(Graphics graphics)
    {
        Rectangle rectangle = new(X - Size / 2, Y - Size / 2, Size, Size);
        graphics.DrawImage(Sprite, rectangle, FrameRectangles[CurrentFrameNumber], GraphicsUnit.Pixel);

        ChangeAnimationFrame();
    }

    public virtual void Update()
    {
        Move();
    }

    public int DistanceToObject(GameObject gameObject)
    {
        return (int)Math.Sqrt(Math.Pow(X - gameObject.X, 2) + Math.Pow(Y - gameObject.Y, 2));
    }

    protected virtual void ChangeAnimationFrame()
    {
        CurrentFrameNumber += 1;

        if (CurrentFrameNumber >= FrameNumber)
            CurrentFrameNumber = 0;
    }


    protected void CalculateFramesRectangles()
    {
        FrameRectangles = new List<Rectangle>();

        for (int i = 0; i < FrameNumber; i++)
            FrameRectangles.Add(new Rectangle(i * Sprite.Width / FrameNumber, 0,
                Sprite.Width / FrameNumber, Sprite.Height));
    }

    private void Move()
    {
        if (CurrentVerticalSpeed > MaxVerticalSpeed)
            CurrentVerticalSpeed = MaxVerticalSpeed;

        if (CurrentVerticalSpeed < -MaxVerticalSpeed)
            CurrentVerticalSpeed = -MaxVerticalSpeed;

        if (CurrentVerticalSpeed > 0)
            CurrentVerticalSpeed -= 1;

        if (CurrentVerticalSpeed < 0)
            CurrentVerticalSpeed += 1;

        X += HorizontalSpeed;
        Y += CurrentVerticalSpeed;

        if (Y < Size / 2)
            Y = Size / 2;
    }

    public void Dodge(Directions direction)
    {
        switch (direction)
        {
            case Directions.Up:
                CurrentVerticalSpeed += Acceleration;
                break;
            case Directions.Down:
                CurrentVerticalSpeed -= Acceleration;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    public virtual void Fire()
    {
        if (CurrentReloadedTime < ReloadedTime)
            return;

        Create(typeof(EnemyBullet), X - Size / 2, Y - Size / 4);
        Create(typeof(EnemyBullet), X - Size / 2, Y + Size / 4);
        CurrentReloadedTime = 0;
    }

    //todo try rename
    public virtual void IsDestroyedIfTakeDamage(int damage, out bool isDestroyed)
    {
        Health -= damage;
        isDestroyed = false;

        if (Health > 0)
            return;

        isDestroyed = true;
        Delete(this);
        Create(typeof(Explosion), X, Y, Size);
    }

    public abstract void Reset();
}