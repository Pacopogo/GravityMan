using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float globalSpeed = 6;

    [Header("Game UI components")]
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private GameObject pauseObject;

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

    private CommandManager commandManager;
    private PlayerInput jumpInput;
    private PlayerInput pauseInput;

    public static bool isPlaying = true;

    private void Start()
    {
        commandManager = new CommandManager(inputs);

        inputs = player;

        player.PlayerStart();

        BuildObjects();

        damagePool = new Objectpool(damageObstacle.Prefab, poolSize, this);
        healPool = new Objectpool(healObstacle.Prefab, poolSize, this);
    }

    private void Update()
    {
        if (!Input.anyKey)
            return;

        pauseInput.CheckInput();

        if (!isPlaying)
            return;

        jumpInput.CheckInput();
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

    private void TogglePause()
    {
        isPlaying = !isPlaying;
        pauseObject.SetActive(!isPlaying);
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

    public void QuitGame()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            return;

        Application.Quit();
    }

    private void BuildObjects()
    {
        //Note: when making new input put the actions at the added actions at the end

        jumpInput = new Builder<PlayerInput>()
            .SetVar(c => c.Keys = keys.Jump)
            .SetVar(c => c.CommandManager = commandManager)
            .SetVar(c => c.InputCommand = commandManager.Jump)
            .SetVar(c => c.InputAction  += player.FlipGravity)
            .Build();

        pauseInput = new Builder<PlayerInput>()
            .SetVar(c => c.Keys = keys.Pause)
            .SetVar(c => c.CommandManager = commandManager)
            .SetVar(c => c.InputCommand = commandManager.Pause)
            .SetVar(c => c.InputAction += TogglePause)
            .SetVar(c => c.InputAction += player.SimulatePlayerPhysics)
            .Build();


        //Note: make the obstacles underneath here
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
