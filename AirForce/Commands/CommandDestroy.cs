using System;

namespace AirForce.Commands
{
    public class CommandCreate : ICommand
    {
        private readonly GameObject receiver;

        private List<GameObject> gameObjectsToAdd;
        private List<GameObject> gameObjectsToRemove;

        public CommandCreate(List<GameObject> gameObjectsToAdd, List<GameObject> gameObjectsToRemove, GameObject receiver)
        {
            this.receiver = receiver;
            this.gameObjectsToAdd = gameObjectsToAdd;
            this.gameObjectsToRemove = gameObjectsToRemove;
        }

        public void Execute()
        {
            gameObjectsToAdd.Add(receiver);
        }

        public void Undo()
        {
            gameObjectsToRemove.Add(receiver);
        }
    }
}