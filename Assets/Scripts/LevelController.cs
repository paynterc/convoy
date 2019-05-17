using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public int maxEmy = 9;
    private float spawnTimer=0f;
    public float spawnInterval = 5f;
    private Object enemyPrefab;
    int enemyCount;   

    // Start is called before the first frame update
    void Start()
    {
        // GameObject myObject = Instantiate(prefab, position, rotation) as GameObject;
        enemyPrefab = Resources.Load("EnemyShip");
        spawnTimer = Time.time + spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (Time.time>spawnTimer && enemyCount<maxEmy)
        {
            spawnTimer = Time.time + spawnInterval;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Debug.Log("SPAWN");
        Vector3 startPos = new Vector3(0,0,0);
        GameObject newEnemy = Instantiate(enemyPrefab, startPos, Quaternion.identity) as GameObject;
    }
}
