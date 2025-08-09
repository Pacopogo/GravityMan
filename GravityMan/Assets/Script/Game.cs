using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] private GameObject healObj;
    [SerializeField] private GameObject dmgObj;

    [Header("Input map")]
    [SerializeField] private PlayerInputKeys keys;
    private IPlayerInputs inputs;

    [Header("Object Lists")]
    private List<GameObject> activeObjects = new List<GameObject>();

    [Header("Objectpool Settings")]
    [SerializeField] private int poolSize = 5;

    private Objectpool damagePool;
    private Objectpool healPool;
    private Objectpool nonePool;

    private GameObstacle damageObstacle;
    private GameObstacle healObstacle;

    public static bool isPlaying = true;

    private void Start()
    {

        inputs = player;

        player.PlayerStart();

        BuildObjects();

        damagePool = new Objectpool(damageObstacle.Prefab, poolSize, this);
        healPool = new Objectpool(healObstacle.Prefab, poolSize, this);
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

        UpdateActiveObjects();

        player.PlayerUpdate();

        globalSpeed += 1 * Time.fixedDeltaTime;

        if (speedText != null)
            speedText.text = globalSpeed.ToString("f0") + ":M/s";
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
            CollisionCheck(obj);
        }
    }

    private void CollisionCheck(GameObject obj)
    {

        //Object to player collision check (if > false > continue)
        if (!obj.GetComponent<Collider2D>().bounds.Intersects(player.Collider.bounds))
            return;

        foreach (GameObject dmg in damagePool.PoolObjects)
        {
            if (obj != dmg)
                continue;

            obj.transform.position = new Vector3(-12, 0, 0);
            player.TakeDamage(1);

            return;
        }

        foreach (GameObject heal in healPool.PoolObjects)
        {
            if (obj != heal)
                continue;

            obj.transform.position = new Vector3(-12, 0, 0);
            player.Heal(1);

            return;
        }

        foreach (GameObject none in nonePool.PoolObjects)
        {
            if (obj != none)
                continue;

            obj.transform.position = new Vector3(-12, 0, 0);
            return;
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
    public GameObject InstantiateObject(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab);
        return newObject;
    }

    private void BuildObjects()
    {
        damageObstacle = new Builder<GameObstacle>()
            .SetVar(c => c.IsDamage = true)
            .SetVar(c => c.Prefab = dmgObj)
            .SetVar(c => c.Speed = 6)
            .Build();

        healObstacle = new Builder<GameObstacle>()
            .SetVar(c => c.Prefab = healObj)
            .SetVar(c => c.Speed = 3)
            .Build();
    }

}
