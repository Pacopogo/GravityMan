using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Game : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float globalSpeed = 6;

    [Header("Game UI components")]
    [SerializeField] private TMP_Text speedText;

    [Header("Player Data")]
    [SerializeField] private PlayerData player;

    [Header("Object Data")]
    [SerializeField] private float spawnRange = 4f;
    [SerializeField] private Obstacle[] obstacles;

    [Header("Input map")]
    [SerializeField] private PlayerInputKeys keys;
    private IPlayerInputs inputs;

    [Header("Object Lists")]
    private List<GameObject> activeObjects = new List<GameObject>();

    [Header("Objectpool Settings")]
    [SerializeField] private int PoolSize = 5;

    private Objectpool damagePool;
    private Objectpool healPool;
    private Objectpool nonePool;

    public static bool isPlaying = false;

    private void Start()
    {
        inputs = player;

        player.PlayerStart();

        foreach (var obj in obstacles)
        {
            switch (obj.Type)
            {
                case objectType.None:
                    nonePool = new Objectpool(obj.prefab, PoolSize, this);
                    break;
                case objectType.Damage:
                    damagePool = new Objectpool(obj.prefab, PoolSize, this);
                    break;
                case objectType.Heal:
                    healPool = new Objectpool(obj.prefab, PoolSize, this);
                    break;
            }
        }
    }

    private void Update()
    {
        inputs.PauseGame(keys.Pause);

        if (!isPlaying)
            return;

        inputs.Jump(keys.Jump);
    }

    private void FixedUpdate()
    {
        if (!isPlaying)
            return;

        CollisionCheck();
        UpdateActiveObjects();

        player.PlayerUpdate();

        globalSpeed += 0.5f * Time.fixedDeltaTime;

        if (speedText != null)
            speedText.text = globalSpeed.ToString("f1") + " M/s";
    }

    private void SpawnObject(Objectpool pool)
    {
        GameObject current = pool.Get();
        float rnd = Random.Range(-spawnRange, spawnRange);

        current.transform.position = new Vector3(10, rnd, 0);
        activeObjects.Add(current);
    }

    private void UpdateActiveObjects()
    {
        if (activeObjects.Count == 0)
        {
            StartCoroutine(SpawnDamageObjects());
            StartCoroutine(SpawnHealObjects());
            return;
        }


        foreach (GameObject obj in activeObjects)
        {
            if (obj.transform.position.x <= -10)
            {
                obj.SetActive(false);
                activeObjects.Remove(obj);
                break;
            }

            obj.transform.Translate(Vector2.left * globalSpeed * Time.fixedDeltaTime);
        }
    }

    private void CollisionCheck()
    {
        foreach (GameObject obj in activeObjects)
        {
            //Object to player collision check (if > false > continue)
            if (!obj.GetComponent<Collider2D>().bounds.Contains(player.PlayerBody.position))
                continue;

            foreach (GameObject dmg in damagePool.PoolObjects)
            {
                if (obj != dmg)
                    continue;

                damagePool.ReturnToPool(obj);
                player.TakeDamage(1);
                return;
            }

            foreach (GameObject heal in healPool.PoolObjects)
            {
                if (obj != heal)
                    continue;

                healPool.ReturnToPool(obj);
                player.Heal(1);
                return;
            }

            foreach (GameObject none in nonePool.PoolObjects)
            {
                if (obj != none)
                    continue;

                damagePool.ReturnToPool(obj);
                return;
            }
        }
    }

    private IEnumerator SpawnDamageObjects()
    {
        float timeRND = Random.Range(0.5f, 1f);
        int amountRND = Random.Range(2, 6);

        for (int i = 0; i < amountRND; i++)
        {
            SpawnObject(damagePool);
            yield return new WaitForSeconds(timeRND);
        }

        StopCoroutine(SpawnDamageObjects());
    }

    private IEnumerator SpawnHealObjects()
    {
        float timeRND = Random.Range(0.3f, 2f);
        int amountRND = Random.Range(1, 3);

        for (int i = 0; i < amountRND; i++)
        {
            SpawnObject(healPool);
            yield return new WaitForSeconds(timeRND);
        }

        StopCoroutine(SpawnDamageObjects());
    }

    //Objectpool can't instantiate so I made a sudo class to be able to instatiate
    //Note: Pablo do not remove this :)
    public GameObject instantiateObject(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab);
        return newObject;
    }

}
