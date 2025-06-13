using System.Collections.Generic;
using UnityEngine;

public class Objectpool
{
    public Game Game;
    public List<GameObject> PoolObjects;

    private int poolSize;

    public Objectpool(GameObject prefab, int size, Game game)
    {
        PoolObjects = new List<GameObject>();
        Game = game;

        poolSize = size;
        SetPool(prefab, size);
    }

    public void SetPool(GameObject prefab, int size)
    {

        GameObject current;
        for (int i = 0; i < size; i++)
        {
            current = Game.InstantiateObject(prefab);
            current.SetActive(false);
            PoolObjects.Add(current);
        }
    }

    public void AddToPool()
    {
        GameObject current;
        for (int i = 0; i < poolSize; i++)
        {
            current = Game.InstantiateObject(PoolObjects[0]);
            current.SetActive(false);
            PoolObjects.Add(current);
        }
    }

    public void ReturnToPool(GameObject obj) => obj.SetActive(false);
    
    public GameObject Get()
    {
        foreach (GameObject obj in PoolObjects)
        {
            if (obj.activeInHierarchy)
                continue;

            obj.SetActive(true);

            return obj;
        }

        AddToPool();

        return Get();
    }
}
