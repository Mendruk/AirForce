namespace AirForce
{
    public interface IShootable
    {
        public void UpdateReloadingTime();

        //todo
        public void Shoot(Action<GameObject> creationAction);
    }
}
