namespace AirForce
{
    public class Effect : GameObject
    {
        public static Queue<Effect> Pull;
        private static int pullSize = 7;

        static Effect()
        {
            Pull = new Queue<Effect>();

            for (int i = 0; i < pullSize; i++)
                Pull.Enqueue(new Effect());
        }

        public Effect()
        {
            sprite = Resource.explosion;
            frameNumber = 4;
            size = sprite.Height;

            Start();
        }

        public static void CreateEffect(int x, int y)
        {
            Effect explosion = Pull.Dequeue();
            explosion.X = x;
            explosion.Y = y;
            explosion.isEnable = true;
        }

        public override void Update()
        {
            currnetFrameNumber++;

            if (currnetFrameNumber >= frameNumber)
            {
                isEnable=false;
                Pull.Enqueue(this);
                currnetFrameNumber = 0;
            }
        }
    }
}
