namespace AirForce.Commands
{
    public class CommandAnimationNextFrame : ICommand
    {
        private readonly GameObject receiver;

        public CommandAnimationNextFrame(GameObject receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.CurrentFrameNumber++;
        }

        public void Undo()
        {
            receiver.CurrentFrameNumber--;
            if (receiver.CurrentFrameNumber < 0)
                receiver.CurrentFrameNumber = receiver.FrameNumber-1;
        }
    }
}