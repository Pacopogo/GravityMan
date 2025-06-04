using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private PlayerData player;
    [SerializeField] private Obstacle[] obstacles;

    private void Start()
    {
        //set Player hp
        player.Health = player.MaxHealth;

        //spawn 5 of each added object type
        foreach (var obstacle in obstacles)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = Instantiate(obstacle.obj);
                float rnd = Random.Range(-4, 4);
                obj.transform.position = new Vector3(0, rnd, 0);
            }
        }
    }

    private void Update()
    {
        player.PlayerUpdate();
    }
}
