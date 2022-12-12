namespace AirForce
{
    //not sure about the title
    public interface IShootable
    {
        public void UpdateReloadingTime();

        public void Shoot(Action<Type, int, int> creationAction);
    }
}
