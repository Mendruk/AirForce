namespace AirForce.Components;

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