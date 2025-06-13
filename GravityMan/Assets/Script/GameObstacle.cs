using UnityEngine;

public class GameObstacle
{
    public GameObject Prefab;
    public objectType ObjectType;
    public float Speed;
    public bool isDamage;

    public class Builder
    {
        GameObject prefab;
        objectType type;
        float speed;
        bool isDamage;

        public Builder SetPrefab(GameObject prefab)
        {
            this.prefab = prefab;
            return this;
        }
        public Builder SetType(objectType type)
        {
            this.type = type;
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
            obstacle.isDamage = isDamage;
            obstacle.Prefab = prefab;
            obstacle.ObjectType = type;
            return obstacle;    

        }
    }
}
