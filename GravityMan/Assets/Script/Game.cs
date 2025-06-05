using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private PlayerData player;
    [SerializeField] private Obstacle[] obstacles;

    [Header("Object List")]
    [SerializeField] private List<GameObject> damageObjects;
    [SerializeField] private List<GameObject> healingObjects;

    [SerializeField] private List<GameObject> SpawnedObjects;
    bool flip = false;



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

                if (obstacle.CanDamage)
                    damageObjects.Add(obj);
                else
                    healingObjects.Add(obj);

                obj.SetActive(false);
            }
        }
    }

    private void Update()
    {
        player.PlayerUpdate();
    }

    private void SpawnRandomObject()
    {
        
    }
}
