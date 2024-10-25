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
        if (objectPool.TryDequeue(out GameObject obj))      // ������Ʈ Ǯ���� ������Ʈ �ҷ���
        {
            obj.SetActive(true);
            activeObject.Enqueue(obj);
        }

        else    // ������Ʈ Ǯ�� ������� ��
        {
            if (pool.Count < maxSize)  // ������ ������Ʈ�� ���� maxSize ���� ���� ��
            {
                obj = CreateObject();
                pool.Add(obj);
                obj = GetObject();
            }

            else // if (pool.Count >= maxSize) // ������ ������Ʈ�� ���� maxSize�� �Ѿ��� ��
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

        // [�䱸���� 2] Get Object
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