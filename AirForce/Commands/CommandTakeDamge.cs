namespace AirForce.Commands
{
    public class CommandTakeDamage : ICommand
    {
        private int damage;
        private readonly GameObject receiver;

        public CommandTakeDamage(int damage, GameObject receiver)
        {
            this.receiver = receiver;
            this.damage = damage;
        }

        public void Execute()
        {
            receiver.Health-= damage;
        }

        public void Undo()
        {
            receiver.Health += damage;
        }
    }
}