namespace AirForce
{
    public abstract class Ship:GameObject
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
            //todo
            if (currentVerticalSpeed > maxVerticalSpeed)
                currentVerticalSpeed = maxVerticalSpeed;

            if (currentVerticalSpeed < -maxVerticalSpeed)
                currentVerticalSpeed = -maxVerticalSpeed;

            if (currentVerticalSpeed > 0)
                currentVerticalSpeed -= acceleration / 2;

            if (currentVerticalSpeed < 0)
                currentVerticalSpeed += acceleration / 2;

            Y += currentVerticalSpeed;

            if (currentReloadingTime < reloadingTime)
                currentReloadingTime++;
        }

        public abstract void Fire();


        public void TakeDamage(int damage)
        {
            currnetHp -= damage;

            if (currnetHp < 0)
                Die();
        }

        private void Die()
        {
            //todo
            sprite = explosion;
        }
    }
}
