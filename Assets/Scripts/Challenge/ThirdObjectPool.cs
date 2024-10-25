using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ThirdObjectPool : MonoBehaviour
{
    private List<GameObject> pool;
    private const int minSize = 0;                 // �������� 1,2 -> 50,  �������� 3 -> 0
    private const int maxSize = 100;                // �������� 1,2 -> 300, �������� 3 -> 100
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
        if (objectPool.TryDequeue(out GameObject obj))      // ������Ʈ Ǯ���� ������Ʈ �ҷ���
        {
            obj.SetActive(true);
        }

        else    // ������Ʈ Ǯ�� ������� ��
        {
            if (pool.Count < maxSize)  // ������ ������Ʈ�� ���� maxSize ���� ���� ��
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

        // [�䱸���� 2] Get Object
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
