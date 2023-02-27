using System;

namespace AirForce.Commands
{
    public class CommandDestroy : ICommand
    {
        private readonly GameObject receiver;

        private List<GameObject> gameObjectsToAdd;
        private List<GameObject> gameObjectsToRemove;

        public CommandDestroy(List<GameObject> gameObjectsToAdd, List<GameObject> gameObjectsToRemove, GameObject receiver)
        {
            this.receiver = receiver;
            this.gameObjectsToAdd = gameObjectsToAdd;
            this.gameObjectsToRemove = gameObjectsToRemove;
        }

        public void Execute()
        { 
            gameObjectsToRemove.Add(receiver);
        }

        public void Undo()
        {
            gameObjectsToAdd.Add(receiver);
        }
    }
}