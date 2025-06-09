using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component, IPoolable
{
    private readonly T prefab;
    private readonly Queue<T> objects = new Queue<T>();
    private readonly Transform parent;

    public ObjectPool(T obj, int initialSize = 10, Transform parentObj = null)
    {
        prefab = obj;
        parent = parentObj;

        for (int i = 0; i < initialSize; i++)
        {
            AddObjectToPool();
        }
    }

    private T AddObjectToPool()
    {
        T newObj = Object.Instantiate(prefab, parent);
        newObj.gameObject.SetActive(false);
        objects.Enqueue(newObj);
        return newObj;
    }

    public T Get()
    {
        if (objects.Count == 0)
        {
            AddObjectToPool();
        }

        T obj = objects.Dequeue();
        obj.gameObject.SetActive(true);
        obj.OnSpawn();
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.OnDespawn();
        obj.gameObject.SetActive(false);
        objects.Enqueue(obj);
    }
}