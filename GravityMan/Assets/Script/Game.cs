using System.Collections.Generic;
<<<<<<< Updated upstream
=======
using Unity.VisualScripting;
>>>>>>> Stashed changes
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private PlayerData player;
    [SerializeField] private Obstacle[] obstacles;

<<<<<<< Updated upstream
    [Header("Object List")]
    [SerializeField] private List<GameObject> damageObjects;
    [SerializeField] private List<GameObject> healingObjects;

    [SerializeField] private List<GameObject> SpawnedObjects;
    bool flip = false;


=======
    private List<Obstacle> damageObstacles;
>>>>>>> Stashed changes

    private void Start()
    {
        //set Player hp
        player.Health = player.MaxHealth;

        //spawn 5 of each added object type
        foreach (var obstacle in obstacles)
        {
            for (int i = 0; i < 5; i++)
            {
<<<<<<< Updated upstream
                GameObject obj = Instantiate(obstacle.obj);

                if (obstacle.CanDamage)
                    damageObjects.Add(obj);
                else
                    healingObjects.Add(obj);

                obj.SetActive(false);
=======
                GameObject obj = Instantiate(obstacle.prefab);
                float rnd = Random.Range(-4, 4);
                obj.transform.position = new Vector3(0, rnd, 0);
>>>>>>> Stashed changes
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
