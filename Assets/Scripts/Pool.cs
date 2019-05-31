using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public List<GameObject> pooledObjects;
    private GameObject myGameObject;

    public Pool(GameObject gameobject, int amountToPool)
    {
        pooledObjects = new List<GameObject>();
        myGameObject = gameobject;
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(gameobject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                //Debug.Log("Return pooled object " + i);
                return pooledObjects[i];
            }
        }
        GameObject obj = (GameObject)Instantiate(myGameObject);
        pooledObjects.Add(obj);
        //Debug.Log("Created new object in pool.");
        return obj;
    }
}
