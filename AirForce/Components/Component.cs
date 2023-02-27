using AirForce.Commands;

namespace AirForce.Components;

public abstract class Component
{
    public GameObject GameObject;

    protected Component(GameObject gameObject)
    {
        GameObject = gameObject;
    }

    public abstract void Update(List<GameObject> gameObjects,Queue<ICommand> commands);
}