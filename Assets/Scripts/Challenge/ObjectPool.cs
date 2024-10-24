using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> pool;
    private const int minSize = 10;
    private const int maxSize = 20;
    private Queue<GameObject> objectPool;
    //[SerializeField] private GameObject objectPrefab;
    //private Queue<GameObject> releasedObject;     // ���� ���� 2
    //private Queue<GameObject> activeObject;       // ���� ���� 2
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
        if (objectPool.TryDequeue(out GameObject obj))      // ������Ʈ Ǯ���� ������Ʈ �ҷ���
        {
            obj.SetActive(true);
            //activeObject.Enqueue(obj);            // Ȱ��ȭ�� object�� ť, �������� 2���� ����
        }

        else    // ������Ʈ Ǯ�� ������� ��
        {
            if (pool.Count < maxSize)  // ������ ������Ʈ�� ���� maxSize ���� ���� ��
            {
                obj = CreateObject();
                pool.Add(obj);
                obj = GetObject();
            }
            #region �������� 1,3
            else
            {
                obj = new GameObject("Object");
            }
            #endregion
            #region �������� 2
            //    else // if (pool.Count >= maxSize) // ������ ������Ʈ�� ���� maxSize�� �Ѿ��� ��
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

        // [�䱸���� 2] Get Object
    }

    public void ReleaseObject(GameObject obj)
    {
        if (pool.Contains(obj))
        {
            objectPool.Enqueue(obj);
            //releasedObject.Enqueue(obj);      // �������� 2
            obj.SetActive(false);
        }

        else
        {
            Destroy(obj);
        }
    }
}