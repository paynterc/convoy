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

    private int haulerCount;// Haulers in inventory
    private int haulersInPlayCount;// Haulers on the field
    private int savedHaulerCount = 0;
    private int savedCargoCount = 0;

    private float levelStartTime = 120f;
    private float levelStartMin = 120f;
    private float levelStartMax = 300f;

    private float spawnTimer = 0f;
    private float spawnBossTimer = 0f;
    private Object enemyPrefab;
    private Object checkpointPrefab;
    private Object haulerPrefab;
    private Object cargoPrefab;

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
    public List<GameObject> lieutenants;


    private UnityAction playerExitListener;
    private UnityAction haulerDeadListener;
    private UnityAction haulerEnteredWormhole;
    private UnityAction cargoEnteredWormhole;
    private UnityAction lostHaulerListener;

    private void Awake()
    {
        playerExitListener = new UnityAction(CompleteLevelSuccess);
        haulerDeadListener = new UnityAction(HaulerDestroyed);
        haulerEnteredWormhole = new UnityAction(HaulerEnteredWormhole);
        cargoEnteredWormhole = new UnityAction(CargoEnteredWormhole);
        lostHaulerListener = new UnityAction(ResetHaulerList);

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
        haulerPrefab = Resources.Load("Hauler2");
        cargoPrefab = Resources.Load("Cargo");

        spawnTimer = Time.time + 5.0f;
        spawnBossTimer = Time.time + spawnBossInterval;

        levelTimerText = GameObject.Find("CountdownText").GetComponent<Text>();
        gameUi = GameObject.Find("UIController").GetComponent<GameUI>();

        SetWormhole();
        StartListeners();
        CreateCheckpoints();
        CreateHaulers();

        LevelStatus();

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
        StopListeners();
    }

    private void OnDestroy()
    {
        StopListeners();
    }

    // LISTENERS
    private void StartListeners()
    {
        EventManager.StartListening("playerExitedLevel", playerExitListener);
        EventManager.StartListening("haulerDestroyed", haulerDeadListener);
        EventManager.StartListening("haulerEnteredWormhole", haulerEnteredWormhole);
        EventManager.StartListening("cargoEnteredWormhole", cargoEnteredWormhole);
        EventManager.StartListening("haulerLost", lostHaulerListener);
    }

    private void StopListeners()
    {
        EventManager.StopListening("playerExitedLevel", playerExitListener);
        EventManager.StopListening("haulerDestroyed", haulerDeadListener);
        EventManager.StopListening("haulerEnteredWormhole", haulerEnteredWormhole);
        EventManager.StopListening("cargoEnteredWormhole", cargoEnteredWormhole);
        EventManager.StopListening("haulerLost", lostHaulerListener);


    }

    private void CompleteLevelSuccess()
    {
        UpdateGameData();
        SceneManager.LoadScene("Intermission", LoadSceneMode.Single);
    }

    private void InitDifficulty()
    {
        maxEmy = GameSingleton.Instance.maxEmy;
        emyPerSpawn = GameSingleton.Instance.emyPerSpawn;
        spawnInterval = GameSingleton.Instance.spawnInterval;
        spawnBossInterval = GameSingleton.Instance.spawnBossInterval;
        bossPerSpawn = GameSingleton.Instance.bossPerSpawn;
        haulerCount = GameSingleton.Instance.HaulerCount;
        haulersInPlayCount = haulerCount;
        levelStartTime = GameSingleton.Instance.levelStartTime;

    }

    private void UpdateGameData()
    {
        GameSingleton.Instance.LostCargo = GameSingleton.Instance.HaulerCount - savedCargoCount;// Do this first
        GameSingleton.Instance.TotalLostCargo += GameSingleton.Instance.LostCargo;
        GameSingleton.Instance.LostHaulers = GameSingleton.Instance.HaulerCount - savedHaulerCount;
        GameSingleton.Instance.LostHaulersTotal += GameSingleton.Instance.LostHaulers;

        GameSingleton.Instance.HaulerCount = savedHaulerCount;
        GameSingleton.Instance.SavedCargo = savedCargoCount;
        GameSingleton.Instance.TotalSavedCargo += savedCargoCount;

        GameSingleton.Instance.maxEmy = Mathf.Clamp(GameSingleton.Instance.maxEmy+1,6,12);
        GameSingleton.Instance.emyPerSpawn = Mathf.Clamp(GameSingleton.Instance.emyPerSpawn + 1, 6, 12);
        GameSingleton.Instance.spawnInterval = Mathf.Clamp(GameSingleton.Instance.spawnInterval - 5, 8, 45);
        //GameSingleton.Instance.HaulerCount = Mathf.Clamp(GameSingleton.Instance.HaulerCount + 1, 3, 20);
        GameSingleton.Instance.levelStartTime = Mathf.Clamp(GameSingleton.Instance.levelStartTime + 20, levelStartMin, levelStartMax);

        GameSingleton.Instance.lieutenantOdds = Mathf.Clamp(GameSingleton.Instance.lieutenantOdds - 1,3,20);
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
        levelTimerText.text = "WORMHOLE OPENS IN: " + levelRemainingTime.ToString("F");

    }

    private void SetWormhole()
    {
        int xx = Random.Range(100, 300);
        int yy = Random.Range(100 , 300);
        int zz = Random.Range(100 , 300);
        Vector3 startPos = new Vector3(xx, yy, zz);
        wormhole.transform.position = startPos;
        wormhole.SetActive(false);
        wormHolePS1.Stop();
        wormHolePS2.Stop();

    }
    private void StartWormhole()
    {
        wormhole.SetActive(true);
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

            if ( Random.Range(1, GameSingleton.Instance.lieutenantOdds) == GameSingleton.Instance.lieutenantOdds)
            {
                // Spawn liutenants
                GameObject lType = lieutenants[Random.Range(0, lieutenants.Count - 1)];
                int c = Random.Range(1,5);
                m = Random.Range(1, 3);
                xx = Random.Range(100 * m, 300 * m);
                yy = Random.Range(100 * m, 300 * m);
                zz = Random.Range(100 * m, 300 * m);
                startPos = new Vector3(xx, yy, zz);
                for (int i = 0; i < c; i++)
                {
                
                    GameObject newL= Instantiate(lType, startPos, Quaternion.identity) as GameObject;
                }

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
            Vector3 startPos = new Vector3(0, 0, zz);
            SpawnHauler(startPos);
            zz -= 40;
        }
        SetHaulerTargets();

    }

    private void SpawnHauler(Vector3 startPos)
    {
        GameObject newHauler = Instantiate(haulerPrefab, startPos, Quaternion.identity) as GameObject;
        haulers.Add(newHauler);
        Vector3 startPos2 = new Vector3(startPos.x, startPos.y, startPos.z-8.0f);
        GameObject newCargo = Instantiate(cargoPrefab, startPos2, Quaternion.identity) as GameObject;
        newCargo.GetComponent<AiController>().target = newHauler.transform;
    }

    public void ResetHaulerList()
    {
        // Do this when a hauler is destroyed. Reset the hauler list with new indexes and then set checkpoint targets again.
        haulers = new List<GameObject>();
        haulers.AddRange(GameObject.FindGameObjectsWithTag("Hauler"));
        SetHaulerTargets();
    }

    public void SetHaulerTargets()
    {
        for (int i=0; i<haulers.Count; i++)
        {
            GameObject hauler = haulers[i];
            if (!hauler) continue;
            HaulerController h = hauler.GetComponent<HaulerController>();
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
        }

    }

    private void HaulerEnteredWormhole()
    {
        savedHaulerCount++;
        haulersInPlayCount = haulersInPlayCount < 1 ? 0 : haulersInPlayCount - 1;
        LevelStatus();
    }

    private void CargoEnteredWormhole()
    {
        savedCargoCount++;
        LevelStatus();
    }

    private void LevelStatus()
    {

        gameUi.UpdateHaulerCountText(haulersInPlayCount);
        gameUi.UpdateSaveCargoText(savedCargoCount);

        if (haulersInPlayCount < 1)
        {
            wormhole.GetComponent<Wormhole>().playerCanEnter = true;
        }
    }


    private void HaulerDestroyed()
    {
        haulerCount = haulerCount < 1 ? 0 : haulerCount -1;
        haulersInPlayCount = haulersInPlayCount < 1 ? 0 : haulersInPlayCount - 1;
        LevelStatus();

    }
}
