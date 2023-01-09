namespace AirForce;

public class GameObject
{
    public int X, Y;

    public int CurrentFrameNumber;
    public int FrameNumber;
    public Bitmap Bitmap;
    public int Size;
    public List<Rectangle> FrameRectangles;

    public GameObjectType Type;
    public int Health;

    public List<Component> Components;

    public GameObject(int x, int y, Bitmap bitmap, GameObjectType type, int health, List<Component> components)
    {
        X = x;
        Y = y;
        Bitmap = bitmap;
        Components = components;
        Type = type;
        Health = health;

        FrameNumber = Bitmap.Width / Bitmap.Height;
        Size = Bitmap.Height;
        FrameRectangles = EnumerateFramesRectangles().ToList();
    }

    private IEnumerable<Rectangle> EnumerateFramesRectangles()
    {
        for (int i = 0; i < FrameNumber; i++)
            yield return new Rectangle(i * Bitmap.Width / FrameNumber, 0,
                Bitmap.Width / FrameNumber, Bitmap.Height);
    }

    public void Update(List<GameObject> gameObjects)
    {
        foreach (Component component in Components)
        {
            component.Update(gameObjects);
        }
    }

    public void Draw(Graphics graphics)
    {
        Rectangle rectangle = new(X - Size / 2, Y - Size / 2, Size, Size);
        graphics.DrawImage(Bitmap, rectangle, FrameRectangles[CurrentFrameNumber], GraphicsUnit.Pixel);
    }

    public int GetDistanceTo(GameObject gameObject)
    {
        return (int)Math.Sqrt(Math.Pow(X - gameObject.X, 2) +
                              Math.Pow(Y - gameObject.Y, 2));
    }
}

public abstract class Component
{
    public GameObject GameObject;

    protected Component(GameObject gameObject)
    {
        GameObject = gameObject;
    }

    public abstract void Update(List<GameObject> gameObjects);
}

public class LoopAnimation : Component
{
    public LoopAnimation(GameObject gameObject) : base(gameObject)
    {
    }

    public override void Update(List<GameObject> gameObjects)
    {
        GameObject.CurrentFrameNumber += 1;

        if (GameObject.CurrentFrameNumber >= GameObject.FrameNumber)
            GameObject.CurrentFrameNumber = 0;
    }
}

public class LoopAnimationFollowedByDeletion : Component
{
    private readonly Action<GameObject> delete;

    public LoopAnimationFollowedByDeletion(GameObject gameObject, Action<GameObject> delete) : base(gameObject)
    {
        this.delete = delete;
    }

    public override void Update(List<GameObject> gameObjects)
    {
        GameObject.CurrentFrameNumber += 1;

        if (GameObject.CurrentFrameNumber >= GameObject.FrameNumber)
            delete(GameObject);
    }
}

public class MoveHorizontal : Component
{
    private readonly int speed;


    public MoveHorizontal(GameObject gameObject, int speed) : base(gameObject)
    {
        this.speed = speed;
    }

    public override void Update(List<GameObject> gameObjects)
    {
        GameObject.X += speed;
    }
}

public class MoveVertical : Component
{
    private readonly int speed;


    public MoveVertical(GameObject gameObject, int speed) : base(gameObject)
    {
        this.speed = speed;
    }

    public override void Update(List<GameObject> gameObjects)
    {
        GameObject.Y += speed;
    }
}

public class Dodge : Component
{
    private readonly int maxVerticalSpeed;
    private readonly int verticalAcceleration;
    private int currentVerticalSpeed;

    public Dodge(GameObject gameObject, int maxVerticalSpeed,int verticalAcceleration) : base(gameObject)
    {
        this.maxVerticalSpeed = maxVerticalSpeed;
        this.verticalAcceleration = verticalAcceleration;
    }

    public override void Update(List<GameObject> gameObjects)
    {
        foreach (GameObject obj in gameObjects)
        {
            if (obj.Type != GameObjectType.PlayerBullet ||
                !HasDodge(GameObject, obj, out DodgeDirection direction))
                continue;

            if (direction == DodgeDirection.Up)
                DodgeUp();
            else if (direction == DodgeDirection.Down)
                DodgeDown();
            break;
        }
        UpdateDodge();
    }

    public void DodgeUp()
    {
        currentVerticalSpeed -= verticalAcceleration;
    }

    public void DodgeDown()
    {
        currentVerticalSpeed += verticalAcceleration;
    }

    public void UpdateDodge()
    {
        if (currentVerticalSpeed > maxVerticalSpeed)
            currentVerticalSpeed = maxVerticalSpeed;

        if (currentVerticalSpeed < -maxVerticalSpeed)
            currentVerticalSpeed = -maxVerticalSpeed;

        if (currentVerticalSpeed > 0)
            currentVerticalSpeed -= 1;

        if (currentVerticalSpeed < 0)
            currentVerticalSpeed += 1;

        GameObject.Y += currentVerticalSpeed;

        if (GameObject.Y < GameObject.Size / 2)
            GameObject.Y = GameObject.Size / 2;
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

public class Fire : Component
{
    public Fire(GameObject gameObject) : base(gameObject)
    {
    }

    public override void Update(List<GameObject> gameObjects)
    {
        throw new NotImplementedException();
    }
}

public class Control : Component
{
    public Control(GameObject gameObject) : base(gameObject)
    {

    }

    public override void Update(List<GameObject> gameObjects)
    {
        
    }
}