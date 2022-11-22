namespace AirForce
{
    public abstract class Ship : GameObject
    {
        private Bitmap explosion = Resource.explosion;

        public int maxHp;
        public int currnetHp;

        protected int acceleration;
        protected int maxVerticalSpeed;
        protected int currentVerticalSpeed;

        protected int reloadingTime;
        protected int currentReloadingTime;


        public override void Update()
        {            
            base.Update();

            if (currentVerticalSpeed > maxVerticalSpeed)
                currentVerticalSpeed = maxVerticalSpeed;

            if (currentVerticalSpeed < -maxVerticalSpeed)
                currentVerticalSpeed = -maxVerticalSpeed;

            if (currentVerticalSpeed > 0)
                currentVerticalSpeed -= 1;

            if (currentVerticalSpeed < 0)
                currentVerticalSpeed += 1;

            Y += currentVerticalSpeed;

            if (currentReloadingTime < reloadingTime)
                currentReloadingTime++;
        }

        protected void TakeDamage(int damage)
        {
            currnetHp -= damage;

            if (currnetHp <= 0)
                Destroy();
        }

        protected abstract void Destroy();

    }
}
