using AirForce.Components;

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