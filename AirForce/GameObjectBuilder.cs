using AirForce.Components;

namespace AirForce
{
    public class GameObjectBuilder
    {
        private readonly List<GameObject> gameObjectsToAdd;
        private readonly List<GameObject> gameObjectsToRemove;

        public GameObjectBuilder(List<GameObject> gameObjectsToAdd, List<GameObject> gameObjectsToRemove)
        {
            this.gameObjectsToAdd = gameObjectsToAdd;
            this.gameObjectsToRemove = gameObjectsToRemove;
        }

        public GameObject GetMeteor(int x, int y)
        {
            GameObject meteor = new(x, y, Resource.asteroid, GameObjectType.Meteor, 10, new List<Component>());
            meteor.Components.Add(new MoveHorizontal(meteor, -5));
            meteor.Components.Add(new MoveVertical(meteor, 5));
            return meteor;
        }

        public GameObject GetBird(int x, int y)
        {
            GameObject bird = new(x, y, Resource.bird, GameObjectType.Bird, 1, new List<Component>());
            bird.Components.Add(new LoopAnimation(bird));
            bird.Components.Add(new MoveHorizontal(bird, -5));
            bird.Components.Add(new BirdDodge(bird, 5, 3));
            return bird;
        }

        public GameObject GetChaserShip(int x, int y)
        {
            GameObject chaserShip = new(x, y, Resource.chaser_ship,
                GameObjectType.Enemy, 1, new List<Component>());
            chaserShip.Components.Add(new LoopAnimation(chaserShip));
            chaserShip.Components.Add(new MoveHorizontal(chaserShip, -5));
            chaserShip.Components.Add(new EnemyDodge(chaserShip, 10, 3));
            return chaserShip;
        }

        public GameObject GetBomberShip(int x, int y)
        {
            GameObject bomberShip = new(x, y, Resource.bomber_ship, GameObjectType.Enemy, 3, new List<Component>());
            bomberShip.Components.Add(new LoopAnimation(bomberShip));
            bomberShip.Components.Add(new MoveHorizontal(bomberShip, -3));
            bomberShip.Components.Add(new EnemyFire(gameObjectsToAdd, gameObjectsToRemove, bomberShip, 20,GetEnemyBullet));
            return bomberShip;
        }

        public GameObject GetPlayerBullet(int x, int y)
        {
            GameObject playerBullet = new(x, y, Resource.player_shot, GameObjectType.PlayerBullet, 1,
                new List<Component>());
            playerBullet.Components.Add(new MoveHorizontal(playerBullet, 10));

            return playerBullet;
        }

        public GameObject GetEnemyBullet(int x, int y)
        {
            GameObject enemyBullet =
                new(x, y, Resource.enemy_shot, GameObjectType.EnemyBullet, 1, new List<Component>());
            enemyBullet.Components.Add(new MoveHorizontal(enemyBullet, -10));

            return enemyBullet;
        }
    }
}
