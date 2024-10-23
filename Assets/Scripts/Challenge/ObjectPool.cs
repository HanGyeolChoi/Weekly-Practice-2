using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> pool;
    private const int minSize = 10;
    private const int maxSize = 20;
    //private Queue<GameObject> objectPoolOverMaxSize;
    private Queue<GameObject> objectPool;
    void Awake()
    {
        pool = new List<GameObject>();
        objectPool = new Queue<GameObject>();
        for (int i = 0; i < minSize; i++)
        {
            pool.Add(CreateObject());
        }
    }

    private GameObject CreateObject()
    {
        #region 구현사항 2
        GameObject obj = new GameObject("Object");
        obj.transform.parent = transform;
        objectPool.Enqueue(obj);
        obj.SetActive(false);

        return obj;
        #endregion
    }

    public GameObject GetObject()
    {
        
        #region 구현사항 2
        if (pool == null) return null;

        if (objectPool.Count < pool.Count)
        {
            GameObject obj = objectPool.Dequeue();
            objectPool.Enqueue(obj);
            obj.SetActive(true);
            return obj;
        }
        if (objectPool.Count >= minSize && objectPool.Count < maxSize)
        {
            GameObject obj = CreateObject();
            pool.Add(obj);
            obj.SetActive(true);
            return obj;
        }

        else // if (objectPool.Count >= maxSize)
        {
            GameObject obj = objectPool.Dequeue();
            objectPool.Enqueue(obj);
            obj.SetActive(true);
            return obj;
        }
        #endregion 

        // [요구스펙 2] Get Object
    }

    public void ReleaseObject(GameObject obj)
    {
        
    }
}