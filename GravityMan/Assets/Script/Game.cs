using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

//DeathSpikes are between Y: -4 & 4
public class Game : MonoBehaviour
{
    [SerializeField] private Transform SpawnTrans;

    [Header("Player Components")]
    [SerializeField] private GameObject playerObj;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private BoxCollider2D playerCol;
    [SerializeField] private Animator playerAni;

    [Header("Player UI")]
    [SerializeField] private Canvas playerCanvas;
    [SerializeField] private Slider playerSlider;
    [SerializeField] private TMP_Text playerSpeedText; 

    [Header("Player Settings")]
    [SerializeField] private KeyCode flipInput = KeyCode.Space;
    [SerializeField] private float gravityPull = 2f;

    [SerializeField] private int playerMaxHp = 10;
    private int playerCurrentHp;

    [SerializeField] private UnityEvent OnPlayerHit;
    [SerializeField] private UnityEvent OnDeath;

    private bool gravityFlip = true;

    [Header("Spike Component")]
    [SerializeField] private GameObject deathSpikeObj;
    private List<GameObject> spikes = new List<GameObject>();

    [Header("Spike Settings")]
    [SerializeField] private int spikeAmount = 5;
    [SerializeField] private float spikeSpeed = 6f;

    [Header("Death Wall Components")]
    [SerializeField] private Collider2D deathWallCol;

    [Header("Scroller Objects")]
    [SerializeField] private GameObject[] scrollerObjects;

    private void Start()
    {
        SpawnSpikes();

        StartCoroutine(SpikeWave(1));
        

        playerCurrentHp = playerMaxHp;
        playerSlider.maxValue = playerMaxHp;
    }

    private void Update()
    {
        PlayerFlip();

    }

    private void FixedUpdate()
    {
        SpikeCollisionCheck();
        SpikeMove();

        spikeSpeed += Time.fixedDeltaTime * 0.2f;
        UpdateSpeedUI();
    }

    #region Game Logic

    private void UpdateSpeedUI() => playerSpeedText.text = spikeSpeed.ToString("f1") + " m/s";

    #endregion

    #region Player
    private void PlayerFlip()
    {
        if (Input.GetKeyDown(flipInput))
        {
            gravityFlip = !gravityFlip;
            playerRB.linearVelocityY = 0;
        }

        playerRB.gravityScale = gravityFlip ? gravityPull : -gravityPull;

        float rot = gravityFlip ? 0 : 180;

        playerCanvas.transform.rotation = Quaternion.Euler(0,0, rot);

        playerAni.SetBool("Flip", gravityFlip);
    }

    private void TakeDmg()
    {

        OnPlayerHit?.Invoke();
        playerCurrentHp--;

        playerSlider.value = playerCurrentHp;

        if (playerCurrentHp < 1)
            OnDeath?.Invoke();

        Debug.Log("Hit");
    }

    #endregion

    #region Spikes
    private void SpawnSpikes()
    {
        for (int i = 0; i < spikeAmount; i++)
        {
            GameObject spike = Instantiate(deathSpikeObj);
            spikes.Add(spike);
            spike.SetActive(false);
        }
    }
    private void SpikeCollisionCheck()
    {
        foreach (GameObject obj in spikes)
        {
            if (!obj.activeInHierarchy)
                continue;

            Collider2D col = obj.GetComponent<Collider2D>();

            if (col.bounds.Intersects(playerCol.bounds))
            {
                obj.SetActive(false);
                TakeDmg();
            }

            if (col.bounds.Intersects(deathWallCol.bounds))
                obj.SetActive(false);
        }
    }
    private void SpikeMove()
    {
        foreach (GameObject obj in spikes)
        {
            if (!obj.activeInHierarchy)
                continue;

            Transform trans = obj.transform;
            trans.Translate(Vector2.left * spikeSpeed * Time.fixedDeltaTime);
        }
    }

    private void GetSpikeAndSpawn()
    {
        foreach (GameObject obj in spikes)
        {
            if (obj.activeInHierarchy)
                continue;

            obj.SetActive(true);

            float rnd = Random.Range(-4, 4);
            obj.transform.position = new Vector3(SpawnTrans.position.x, rnd, SpawnTrans.position.y);

            return;
        }
        SpawnSpikes();
    }

    private IEnumerator SpikeWave(float Time)
    {
        yield return new WaitForSeconds(Time);

        float rnd = Random.Range(2, 7);

        for (int i = 0; i < rnd; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GetSpikeAndSpawn();
        }


        float rndTime = Random.Range(1, 2);
        StartCoroutine(SpikeWave(rndTime));
        
        StopCoroutine(SpikeWave(Time));
    }

    #endregion

    #region Objects



    #endregion
}
