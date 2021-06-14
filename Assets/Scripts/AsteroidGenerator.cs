using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{


    public GameObject asteroidPrefab;
    public int scale;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }


    private void Init()
    {
        int num = 50 * scale;
        for (int i = 1; i < num; i++)
        {
            int xx = Random.Range(100 * i, 300 * i * scale);
            int yy = Random.Range(100 * i, 300 * i * scale);
            int zz = Random.Range(100 * i, 300 * i * scale);
            Vector3 startPos = new Vector3(xx, yy, zz);
            GameObject newAsteroid = Instantiate(asteroidPrefab, startPos, Quaternion.identity) as GameObject;

        }

    }
}
