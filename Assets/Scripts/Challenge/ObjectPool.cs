using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> pool;
    private const int minSize = 10;
    private const int maxSize = 20;
    private Queue<GameObject> objectPool;
    //[SerializeField] private GameObject objectPrefab;
    //private Queue<GameObject> releasedObject;     // 구현 사항 2
    //private Queue<GameObject> activeObject;       // 구현 사항 2
    void Awake()
    {
        pool = new List<GameObject>();
        objectPool = new Queue<GameObject>();

        //releasedObject = new Queue<GameObject>();
        //activeObject = new Queue<GameObject>();

        for (int i = 0; i < minSize; i++)
        {
            pool.Add(CreateObject());
        }
    }

    private GameObject CreateObject()
    {
        GameObject obj = new GameObject("Object");
        obj.transform.parent = transform;
        objectPool.Enqueue(obj);
        obj.SetActive(false);

        return obj;
    }

    public GameObject GetObject()
    {
        if (objectPool.TryDequeue(out GameObject obj))      // 오브젝트 풀에서 오브젝트 불러옴
        {
            obj.SetActive(true);
            //activeObject.Enqueue(obj);            // 활성화된 object의 큐, 구현사항 2에서 쓰임
        }

        else    // 오브젝트 풀이 비어있을 때
        {
            if (pool.Count < maxSize)  // 생성된 오브젝트의 수가 maxSize 보다 작을 때
            {
                obj = CreateObject();
                pool.Add(obj);
                obj = GetObject();
            }
            #region 구현사항 1,3
            else
            {
                obj = new GameObject("Object");
            }
            #endregion
            #region 구현사항 2
            //    else // if (pool.Count >= maxSize) // 생성된 오브젝트의 수가 maxSize를 넘었을 때
            //    {
            //        while (true)
            //        {
            //            obj = activeObject.Dequeue();
            //            if (!releasedObject.TryDequeue(out GameObject result)) break;
            //            if (result == obj) break;
            //        }
            //        activeObject.Enqueue(obj);
            //    }
            #endregion
        }

        return obj;

        // [요구스펙 2] Get Object
    }

    public void ReleaseObject(GameObject obj)
    {
        if (pool.Contains(obj))
        {
            objectPool.Enqueue(obj);
            //releasedObject.Enqueue(obj);      // 구현사항 2
            obj.SetActive(false);
        }

        else
        {
            Destroy(obj);
        }
    }
}