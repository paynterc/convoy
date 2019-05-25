using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public int maxEmy = 18;
    private float spawnTimer=0f;
    public float spawnInterval = 15f;
    private float spawnBossTimer = 0f;
    public float spawnBossInterval = 120f;
    private Object enemyPrefab;
    private Object checkpointPrefab;
    private Object haulerPrefab;
    private int enemyCount = 0;
    // Lists: https://answers.unity.com/questions/1292679/how-to-add-to-generic-list.html
    List<GameObject> checkpoints = new List<GameObject>();
    int checkpointCurrent = 0;// Pointer for the current checkpoint
    List<GameObject> haulers = new List<GameObject>();
    public GameObject bossPrefab;
    private bool bossSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        // GameObject myObject = Instantiate(prefab, position, rotation) as GameObject;
        enemyPrefab = Resources.Load("EnemyShip");
        checkpointPrefab = Resources.Load("Checkpoint");
        haulerPrefab = Resources.Load("Hauler");

        spawnTimer = Time.time + spawnInterval;
        spawnBossTimer = Time.time + spawnBossInterval;
        CreateCheckpoints();

        CreateHaulers();

        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        EnemySpawner();
        SpawnBoss();

    }

    private void EnemySpawner()
    {
        if (Time.time > spawnTimer && enemyCount < maxEmy)
        {
            spawnTimer = Time.time + spawnInterval;
            int m = Random.Range(1, 3);
            int xx = Random.Range(100 * m, 300 * m);
            int yy = Random.Range(100 * m, 300 * m);
            int zz = Random.Range(100 * m, 300 * m);
            Vector3 startPos = new Vector3(xx, yy, zz);
            for (int i=0;i<6;i++)
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
            int xx = Random.Range(100 * m, 300 * m);
            int yy = Random.Range(100 * m, 300 * m);
            int zz = Random.Range(100 * m, 300 * m);
            Vector3 startPos = new Vector3(xx, yy, zz);

            GameObject newBoss = Instantiate(bossPrefab, startPos, Quaternion.identity) as GameObject;

            spawnInterval = spawnInterval * 3;// Reduce the rate of drone spawns
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
        for (int i = 0; i < 5; i++)
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
            if (i==0)
            {
                h.SetTarget(checkpoints[checkpointCurrent].transform);
            }
            else
            {
                h.SetTarget(haulers[i-1].transform);
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
    public void ProcessArrival(GameObject hauler)
    {
        if (hauler == haulers[0])
        {
            IncrementCheckpoint();
        }
    }
}
