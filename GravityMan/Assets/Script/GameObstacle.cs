using UnityEngine;

public class GameObstacle
{
    public GameObject Prefab;
    public float Speed;
    public bool IsDamage;

    public class Builder
    {
        GameObject prefab;
        float speed;
        bool isDamage;

        public Builder SetPrefab(GameObject prefab)
        {
            this.prefab = prefab;
            return this;
        }
        public Builder SetSpeed(float speed)
        {
            this.speed = speed;
            return this;
        }
        public Builder SetIsDamage(bool isDamage)
        {
            this.isDamage = isDamage;
            return this;
        }
        
        public GameObstacle Build()
        {
            var obstacle = new GameObstacle();
            obstacle.Speed = speed;
            obstacle.IsDamage = isDamage;
            obstacle.Prefab = prefab;
            return obstacle;    

        }
    }
}
