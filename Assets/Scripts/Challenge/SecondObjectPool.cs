using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SecondObjectPool : MonoBehaviour
{

    private List<GameObject> pool;
    private const int minSize = 50;
    private const int maxSize = 300;
    private Queue<GameObject> objectPool;
    private Queue<GameObject> releasedObject;
    private Queue<GameObject> activeObject;
    private int i = 0;
    [SerializeField] private GameObject objectPrefab;
    void Awake()
    {
        pool = new List<GameObject>();
        objectPool = new Queue<GameObject>();

        releasedObject = new Queue<GameObject>();
        activeObject = new Queue<GameObject>();

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
            activeObject.Enqueue(obj);
        }

        else    // 오브젝트 풀이 비어있을 때
        {
            if (pool.Count < maxSize)  // 생성된 오브젝트의 수가 maxSize 보다 작을 때
            {
                obj = CreateObject();
                pool.Add(obj);
                obj = GetObject();
            }

            else // if (pool.Count >= maxSize) // 생성된 오브젝트의 수가 maxSize를 넘었을 때
            {
                while (true)
                {
                    obj = activeObject.Dequeue();
                    if (!releasedObject.TryPeek(out GameObject result)) break;
                    if (result == obj)
                    {
                        releasedObject.Dequeue();
                    }
                    else break;
                }
                activeObject.Enqueue(obj);
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
            releasedObject.Enqueue(obj);
            obj.SetActive(false);
        }

        else
        {
            Destroy(obj);
        }
    }

    public void ReleaseObject()
    {

        if (!activeObject.TryPeek(out GameObject obj)) return;
        
        if (releasedObject.TryPeek(out GameObject rel))
        {
            if (obj == rel)
            {
                activeObject.Dequeue();
                releasedObject.Dequeue();
                ReleaseObject();
                return;
            }
        }
        objectPool.Enqueue(obj);
        releasedObject.Enqueue(obj);
        obj.SetActive(false);

    }
}