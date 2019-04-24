using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    protected List<GameObject> pool;
    protected GameObject prefab;
    protected int capacity;

    protected int lastFetchId = -1;

    public ObjectPool(GameObject prefab, int capacity)
    {
        this.prefab = prefab;
        this.capacity = capacity;

        CreatePool();
    }

    public ObjectPool(string prefabPath, int capacity)
    {
        this.prefab = Resources.Load<GameObject>(prefabPath);
        this.capacity = capacity;

        CreatePool();
    }

    private void CreatePool()
    {
        pool = new List<GameObject>(capacity);

        for (int i = 0; i < capacity; i++)
        {
            pool.Add(GameObject.Instantiate(this.prefab, Vector3.zero, Quaternion.identity));
            pool[i].SetActive(false);

            Debug.Log(pool.Capacity + "," + pool.Count);
        }
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        int id = (lastFetchId + 1) % capacity;

        if (pool[id].activeSelf == false)
        {
            lastFetchId = id;
            pool[id].SetActive(true);
            pool[id].transform.position = position;
            pool[id].transform.rotation = rotation;
            return pool[id];
        }

        for(int i = 0; i < capacity; i++)
        {
            if(pool[i].activeSelf == false)
            {
                pool[i].SetActive(true);
                pool[i].transform.position = position;
                pool[i].transform.rotation = rotation;
                return pool[i];
            }
        }

        GameObject newInstance = GameObject.Instantiate(this.prefab, position, rotation);
        pool.Add(newInstance);

        return newInstance;
    }

    public virtual void Recycle(GameObject go)
    {
        go.SetActive(false);

        Rigidbody body = go.GetComponent<Rigidbody>();

        if (body)
        {
            body.velocity = Vector3.zero;
        }
    }
}
