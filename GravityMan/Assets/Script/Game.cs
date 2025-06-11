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
    [SerializeField] private List<GameObject> noneObj;
    [SerializeField] private List<GameObject> healObj;
    [SerializeField] private List<GameObject> damageObj;
    private List<GameObject> activeObjects = new List<GameObject>();

    [Header("Objectpool Settings")]
    [SerializeField] private int PoolSize = 5;

    public bool isPlaying = false;

    private void Start()
    {
        inputs = player;

        player.PlayerStart();

        //spawn 5 of each added object type
        InizializeObjects();
    }

    private void Update()
    {
        isPlaying = inputs.PauseGame(keys.Pause, isPlaying);

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

    private void InizializeObjects()
    {
        GameObject currentObject;
        foreach (var obstacle in obstacles)
        {
            for (int i = 0; i < PoolSize; i++)
            {
                currentObject = Instantiate(obstacle.prefab);
                SortObject(currentObject, obstacle.Type);
                currentObject.SetActive(false);
            }
        }

    }
    private void SortObject(GameObject obj, objectType type)
    {
        switch (type)
        {
            case objectType.None:
                noneObj.Add(obj);
                break;

            case objectType.Damage:
                damageObj.Add(obj);
                break;

            case objectType.Heal:
                healObj.Add(obj);
                break;
            default:
                break;
        }
    }
    private void SpawnObject(List<GameObject> list)
    {
        GameObject current;
        float rnd = Random.Range(-spawnRange, spawnRange);
        current = GetFromPool(list);
        if (!current)
        {
            AddToPool(list);
            current = GetFromPool(list);
        }
        current.transform.position = new Vector3(10, rnd, 0);
    }
    #region ObjectPool

    private void AddToPool(List<GameObject> list)
    {
        GameObject obj = list[0];
        GameObject newObj;

        for (int i = 0; i < PoolSize; i++)
        {
            newObj = Instantiate(obj);
            newObj.SetActive(false);
            list.Add(newObj);
        }
    }
    private GameObject GetFromPool(List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            if (obj.activeInHierarchy)
                continue;

            obj.SetActive(true);
            activeObjects.Add(obj);
            return obj;
        }

        return null;
    }
    private void ReturnToPool(GameObject obj)
    {
        activeObjects.Remove(obj);
        obj.SetActive(false);
    }
    #endregion
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
                ReturnToPool(obj);
                break;
            }

            obj.transform.Translate(Vector2.left * globalSpeed * Time.fixedDeltaTime);
        }
    }
    private void CollisionCheck()
    {
        foreach (GameObject obj in activeObjects)
        {
            if (!obj.GetComponent<Collider2D>().bounds.Contains(player.PlayerBody.position))
                continue;

            foreach (GameObject dmg in damageObj)
            {
                if (obj != dmg)
                    continue;

                Debug.Log("DMG");
                ReturnToPool(obj);
                player.TakeDamage(1);
                return;
            }

            foreach (GameObject heal in healObj)
            {
                if (obj != heal)
                    continue;

                Debug.Log("HEAL");
                ReturnToPool(obj);
                player.Heal(1);
                return;
            }

            foreach (GameObject none in noneObj)
            {
                if (obj != none)
                    continue;

                ReturnToPool(obj);
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
            SpawnObject(damageObj);
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
            SpawnObject(healObj);
            yield return new WaitForSeconds(timeRND);
        }

        StopCoroutine(SpawnDamageObjects());
    }

}
