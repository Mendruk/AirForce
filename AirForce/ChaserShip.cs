namespace AirForce
{
    public class ChaserShip : Ship
    {
        private int horizontalSpeed = 5;

        public ChaserShip()
        {
            sprite = Resource.chaser_ship;
            X = Game.GameFieldWidth;
            Y = Game.GameFieldHeight / 2 - sprite.Height / 2;

            acceleration = 2;
            maxVerticalSpeed = 8;
        }

        public override void Fire()
        {
            //todo
        }

        public override void Update()
        {
            base.Update();

            X -= horizontalSpeed;

            foreach (GameObject gameObject in Game.GameObjectList)
            {
                if (!(gameObject is PlayerBullet))
                    continue;

                if (gameObject.X > X)
                    continue;

                if (gameObject.Y > Y &&
                    gameObject.Y < Y + sprite.Height)
                {
                    if (gameObject.Y > Y + sprite.Height / 2)
                        currentVerticalSpeed -= acceleration;
                    else
                        currentVerticalSpeed += acceleration;

                    return;
                }
            }
        }
    }
}
