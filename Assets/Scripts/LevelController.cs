using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private int maxEmy = 6;
    private int emyPerSpawn=6;
    private float spawnInterval = 15f;
    private float spawnBossInterval = 120f;
    private int bossPerSpawn = 1;
    private int haulerCount = 2;
    private float levelStartTime = 120f;

    private float spawnTimer = 0f;
    private float spawnBossTimer = 0f;
    private Object enemyPrefab;
    private Object checkpointPrefab;
    private Object haulerPrefab;

    private float levelRemainingTime;
    public Text levelTimerText;
    public GameObject wormhole;
    public ParticleSystem wormHolePS1;
    public ParticleSystem wormHolePS2;
    private bool wormholeStarted = false;


    // Lists: https://answers.unity.com/questions/1292679/how-to-add-to-generic-list.html
    List<GameObject> checkpoints = new List<GameObject>();
    int checkpointCurrent = 0;// Pointer for the current checkpoint
    List<GameObject> haulers = new List<GameObject>();
    public GameObject bossPrefab;
    private bool bossSpawned = false;
    public GameUI gameUi;

    private UnityAction playerExitListener;

    private void Awake()
    {
        playerExitListener = new UnityAction(CompleteLevelSuccess);

    }

    // Start is called before the first frame update
    void Start()
    {

        Time.timeScale = 1.0f;
        GameSingleton.Instance.SavedCargo = 0;
        InitDifficulty();// Get stored vars in singleton
        levelRemainingTime = levelStartTime;

        // GameObject myObject = Instantiate(prefab, position, rotation) as GameObject;
        enemyPrefab = Resources.Load("EnemyShip");
        checkpointPrefab = Resources.Load("Checkpoint");
        haulerPrefab = Resources.Load("Hauler");

        spawnTimer = Time.time + spawnInterval;
        spawnBossTimer = Time.time + spawnBossInterval;

        levelTimerText = GameObject.Find("CountdownText").GetComponent<Text>();
        gameUi = GameObject.Find("UIController").GetComponent<GameUI>();

        SetWormhole();
        Debug.Log(GameSingleton.Instance.MyTestString);

        // LISTENERS
        EventManager.StartListening("playerExitedLevel", playerExitListener);

        CreateCheckpoints();
        CreateHaulers();

    }

    // Update is called once per frame
    void Update()
    {
        DecrementTimer();
        EnemySpawner();
        SpawnBoss();

    }

    void OnDisable()
    {
        EventManager.StopListening("playerExitedLevel", playerExitListener);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("playerExitedLevel", playerExitListener);
    }

    private void CompleteLevelSuccess()
    {
        SetDifficulty();
        SceneManager.LoadScene("Intermission", LoadSceneMode.Single);
    }

    private void InitDifficulty()
    {
        maxEmy = GameSingleton.Instance.maxEmy;
        emyPerSpawn = GameSingleton.Instance.emyPerSpawn;
        spawnInterval = GameSingleton.Instance.spawnInterval;
        spawnBossInterval = GameSingleton.Instance.spawnBossInterval;
        bossPerSpawn = GameSingleton.Instance.bossPerSpawn;
        haulerCount = GameSingleton.Instance.haulerCount;
        levelStartTime = GameSingleton.Instance.levelStartTime;
    }

    private void SetDifficulty()
    {
        GameSingleton.Instance.maxEmy = Mathf.Clamp(GameSingleton.Instance.maxEmy+1,6,12);
        GameSingleton.Instance.emyPerSpawn = Mathf.Clamp(GameSingleton.Instance.emyPerSpawn + 1, 6, 12);
        GameSingleton.Instance.spawnInterval = Mathf.Clamp(GameSingleton.Instance.spawnInterval - 5, 8, 45);
        GameSingleton.Instance.haulerCount = Mathf.Clamp(GameSingleton.Instance.haulerCount + 1, 3, 20);
        GameSingleton.Instance.levelStartTime = Mathf.Clamp(GameSingleton.Instance.levelStartTime + 10, 240, 240*3);
    }

    private void DecrementTimer()
    {
        if (levelRemainingTime>0.0f)
        {
            levelRemainingTime -= Time.deltaTime;
        }
        else
        {
            levelRemainingTime = 0.0f;
            if (!wormholeStarted)
            {
                wormholeStarted = true;
                StartWormhole();
            }

        }
        levelTimerText.text = "Time to Wormhole: " + levelRemainingTime.ToString("F");

    }

    private void SetWormhole()
    {
        int xx = Random.Range(100, 300);
        int yy = Random.Range(100 , 300);
        int zz = Random.Range(100 , 300);
        Vector3 startPos = new Vector3(xx, yy, zz);
        wormhole.transform.position = startPos;
        wormHolePS1.Stop();
        wormHolePS2.Stop();

    }
    private void StartWormhole()
    {

        wormHolePS1.Play();
        wormHolePS2.Play();


    }

    private void EnemySpawner()
    {
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (Time.time > spawnTimer && enemyCount < maxEmy)
        {
            spawnTimer = Time.time + spawnInterval;
            int m = Random.Range(1, 3);
            int xx = Random.Range(100 * m, 300 * m);
            int yy = Random.Range(100 * m, 300 * m);
            int zz = Random.Range(100 * m, 300 * m);
            Vector3 startPos = new Vector3(xx, yy, zz);
            for (int i=0; i<emyPerSpawn; i++)
            {
                SpawnEnemy(startPos);
            }
        }
    }

    private void SpawnEnemy(Vector3 startPos)
    {

        GameObject newEnemy = Instantiate(enemyPrefab, startPos, Quaternion.identity) as GameObject;
    }

    private void SpawnBoss()
    {
        if (Time.time > spawnBossTimer && !bossSpawned)
        {
            bossSpawned = true;
            spawnBossTimer = Time.time + spawnBossInterval;
            int m = 1;
            for (var i=0; i < bossPerSpawn; i++)
            {
                int xx = Random.Range(100 * m, 300 * m);
                int yy = Random.Range(100 * m, 300 * m);
                int zz = Random.Range(100 * m, 300 * m);
                Vector3 startPos = new Vector3(xx, yy, zz);
                GameObject newBoss = Instantiate(bossPrefab, startPos, Quaternion.identity) as GameObject;
            }
            //spawnInterval = spawnInterval * 3;// Reduce the rate of drone spawns
        }
    }

    private void CreateCheckpoints()
    {
        for(int i=0;i<3;i++)
        {
            SpawnCheckpoint(i);
        }
    }

    private void SpawnCheckpoint(int m)
    {
        int xx = Random.Range(100 * m,300 * m );
        int yy = Random.Range(100 * m, 300 * m);
        int zz = Random.Range(100 * m, 300 * m);
        Vector3 startPos = new Vector3(xx,yy,zz);
        GameObject newCheckpoint = Instantiate(checkpointPrefab, startPos, Quaternion.identity) as GameObject;
        checkpoints.Add(newCheckpoint);
    }

    private void CreateHaulers()
    {
        int zz = 0;
        for (int i = 0; i < haulerCount; i++)
        {
            Vector3 startPos = new Vector3(0, 20, zz);
            SpawnHauler(startPos);
            zz -= 20;
        }
        SetHaulerTargets();
        EventManager.TriggerEvent("cargoUpdate");
    }

    private void SpawnHauler(Vector3 startPos)
    {
        GameObject newHauler = Instantiate(haulerPrefab, startPos, Quaternion.identity) as GameObject;
        haulers.Add(newHauler);
    }

    public void SetHaulerTargets()
    {
        for (int i=0; i<haulers.Count; i++)
        {
            HaulerController h = haulers[i].GetComponent<HaulerController>();
            if (wormholeStarted)
            {
                h.SetTarget(wormhole.transform);
            }
            else
            {
                if (i == 0)
                {
                    h.SetTarget(checkpoints[checkpointCurrent].transform);
                }
                else
                {
                    h.SetTarget(haulers[i - 1].transform);
                }
            }

        }

    }

    public void IncrementCheckpoint()
    {
        checkpointCurrent++;
        if (checkpointCurrent >= checkpoints.Count)
        {
            checkpointCurrent = 0;
        }
        SetHaulerTargets();
    }

    // A hauler has arrived at a checkpoint
    public void ProcessArrival(GameObject hauler, GameObject target)
    {
        //Debug.Log("target.name " + target.name);
        if (target.name == "Checkpoint(Clone)")
        {
            if (hauler == haulers[0])
            {
                IncrementCheckpoint();
            }
        }else if (target.name=="Wormhole")
        {
            ProcessWormholeArrival(hauler);
        }

    }

    private void ProcessWormholeArrival(GameObject hauler)
    {
        // Count attached cargo containers
        CargoController[] cargoContainers = hauler.GetComponentsInChildren<CargoController>();
        // Add count to a "saved containers" total
        GameSingleton.Instance.SavedCargo += cargoContainers.Length;
        GameSingleton.Instance.TotalSavedCargo += cargoContainers.Length;
        // Deactivate hauler and children
        hauler.SetActive(false);
        // Update UI
        UpdateSaveCargoText();

        CargoStatus();
        EventManager.TriggerEvent("cargoUpdate");
    }

    private void CargoStatus()
    {
        int cargoCount = GameObject.FindGameObjectsWithTag("Cargo").Length;
        if (cargoCount<1)
        {
            wormhole.GetComponent<Wormhole>().playerCanEnter = true;
        }
    }

    private void UpdateSaveCargoText()
    {
        gameUi.UpdateSaveCargoText(GameSingleton.Instance.SavedCargo);

    }
}
