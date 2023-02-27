﻿using AirForce.Commands;

namespace AirForce.Components;

public class LoopAnimationFollowedByDeletion : Component
{
    protected List<GameObject> gameObjectsToAdd;
    protected List<GameObject> gameObjectsToRemove;

    public LoopAnimationFollowedByDeletion(GameObject gameObject, List<GameObject> gameObjectsToAdd,List<GameObject>gameObjectsToRemove) : base(gameObject)
    {
        this.gameObjectsToAdd = gameObjectsToAdd;
        this.gameObjectsToRemove = gameObjectsToRemove;
    }

    public override void Update(List<GameObject> gameObjects, Queue<ICommand> commands)
    {

        if (GameObject.CurrentFrameNumber >= GameObject.FrameNumber-1)
        {
            GameObject.CurrentFrameNumber = 0;
            commands.Enqueue(new CommandDestroy(gameObjectsToAdd, gameObjectsToRemove, GameObject));
        }

        GameObject.CurrentFrameNumber++;
    }
}