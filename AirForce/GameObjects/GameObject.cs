namespace AirForce;

public abstract class GameObject
{
    protected Random random = new();

    protected int CurrentFrameNumber;
    protected int FrameNumber;
    protected Bitmap Sprite;
    protected List<Rectangle> FrameRectangles;

    protected int MaxHealth;
    public int Health;
    public int Size;

    public int X;
    public int Y;

    public bool IsEnable;
    public GameObjectType Type;

    protected GameObject(Bitmap sprite)
    {
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

        ChangeAnimationFrame();
    }

    protected virtual void ChangeAnimationFrame()
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

    public virtual void Reset()
    {
        Health = MaxHealth;
    }

    private IEnumerable<Rectangle> EnumerateFramesRectangles()
    {
        for (int i = 0; i < FrameNumber; i++)
            yield return new Rectangle(i * Sprite.Width / FrameNumber, 0,
                Sprite.Width / FrameNumber, Sprite.Height);
    }
}