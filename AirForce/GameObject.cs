using AirForce.Commands;
using AirForce.Components;

namespace AirForce;

public class GameObject
{
    private readonly Bitmap bitmap;
    private readonly List<Rectangle> frameRectangles;

    public readonly GameObjectType Type;
    public readonly int FrameNumber;
    public int CurrentFrameNumber;

    public List<Component> Components;
    public int Health;
    public int Size;
    public int X, Y;

    public GameObject(int x, int y, Bitmap bitmap, GameObjectType type, int health, List<Component> components)
    {
        X = x;
        Y = y;
        this.bitmap = bitmap;
        Components = components;
        Type = type;
        Health = health;

        FrameNumber = this.bitmap.Width / this.bitmap.Height;
        Size = this.bitmap.Height;
        frameRectangles = EnumerateFramesRectangles().ToList();
    }

    private IEnumerable<Rectangle> EnumerateFramesRectangles()
    {
        for (int i = 0; i < FrameNumber; i++)
            yield return new Rectangle(i * bitmap.Width / FrameNumber, 0,
                bitmap.Width / FrameNumber, bitmap.Height);
    }

    public void Update(List<GameObject> gameObjects, Queue<ICommand> commands)
    {
        foreach (Component component in Components) component.Update(gameObjects, commands);
    }

    public void Draw(Graphics graphics)
    {
        Rectangle rectangle = new(X - Size / 2, Y - Size / 2, Size, Size);
        graphics.DrawImage(bitmap, rectangle, frameRectangles[CurrentFrameNumber], GraphicsUnit.Pixel);
    }

    public int GetDistanceTo(GameObject gameObject)
    {
        return (int)Math.Sqrt(Math.Pow(X - gameObject.X, 2) +
                              Math.Pow(Y - gameObject.Y, 2));
    }
}