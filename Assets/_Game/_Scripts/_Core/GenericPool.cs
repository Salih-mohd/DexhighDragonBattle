using System.Collections.Generic;
using UnityEngine;

public class GenericPool<T> where T : Component
{
    private readonly Queue<T> pool = new Queue<T>();

    private readonly T prefab;
    private readonly Transform parent;

    public GenericPool(
        T prefab,
        int initialSize,
        Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        CreateInitialPool(initialSize);
    }

    private void CreateInitialPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            T item = CreateNewItem();

            pool.Enqueue(item);
        }
    }

    private T CreateNewItem()
    {
        T item = Object.Instantiate(
            prefab,
            parent
        );

        item.gameObject.SetActive(false);

        return item;
    }

    public T Get()
    {
        T item;

        if (pool.Count > 0)
        {
            item = pool.Dequeue();
        }
        else
        {
            item = CreateNewItem();
        }

        item.gameObject.SetActive(true);

        return item;
    }

    public void Release(T item)
    {
        if (item == null)
            return;

        item.gameObject.SetActive(false);

        pool.Enqueue(item);
    }
}