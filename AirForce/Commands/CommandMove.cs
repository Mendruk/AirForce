namespace AirForce.Commands
{
    public class CommandMove : ICommand
    {
        private readonly int deltaX;
        private readonly int deltaY;

        private readonly GameObject receiver;

        public CommandMove(int deltaX, int deltaY, GameObject receiver)
        {
            this.deltaX = deltaX;
            this.deltaY = deltaY;
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.X += deltaX;
            receiver.Y += deltaY;
        }

        public void Undo()
        {
            receiver.X -= deltaX;
            receiver.Y -= deltaY;
        }
    }
}