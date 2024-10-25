using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ThirdObjectPool : MonoBehaviour
{
    private List<GameObject> pool;
    private const int minSize = 0;                 // 구현사항 1,2 -> 50,  구현사항 3 -> 0
    private const int maxSize = 100;                // 구현사항 1,2 -> 300, 구현사항 3 -> 100
    private Queue<GameObject> objectPool;

    private int i = 0;
    [SerializeField] private GameObject objectPrefab;
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
        GameObject obj = Instantiate(objectPrefab, transform);
        obj.name = $"Object{i}";
        i++;
        objectPool.Enqueue(obj);
        obj.SetActive(false);

        return obj;
    }

    public GameObject GetObject()
    {
        if (objectPool.TryDequeue(out GameObject obj))      // 오브젝트 풀에서 오브젝트 불러옴
        {
            obj.SetActive(true);
        }

        else    // 오브젝트 풀이 비어있을 때
        {
            if (pool.Count < maxSize)  // 생성된 오브젝트의 수가 maxSize 보다 작을 때
            {
                obj = CreateObject();
                pool.Add(obj);
                obj = GetObject();
            }
            else
            {
                obj = new GameObject("Object");
            }


        }

        return obj;

        // [요구스펙 2] Get Object
    }

    public void ReleaseObject(GameObject obj)
    {
        if (pool.Contains(obj))
        {
            objectPool.Enqueue(obj);
            obj.SetActive(false);
        }

        else
        {
            Destroy(obj);
        }
    }


}
