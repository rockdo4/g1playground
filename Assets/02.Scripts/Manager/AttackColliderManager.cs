using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AttackColliderManager : MonoBehaviour
{
    public Projectile[] projectilePrefabs;
    public RangeCollider[] rangeColliderPrefabs;

    private Dictionary<Type, AttackCollider[]> prefabList = new Dictionary<Type, AttackCollider[]>();
    private Dictionary<Type, Dictionary<string, Queue<AttackCollider>>> attackColliderPool = new Dictionary<Type, Dictionary<string, Queue<AttackCollider>>>();

    private void Start()
    {
        prefabList.Add(typeof(Projectile), projectilePrefabs);
        prefabList.Add(typeof(RangeCollider), rangeColliderPrefabs);
        attackColliderPool.Add(typeof(Projectile), new Dictionary<string, Queue<AttackCollider>>());
        attackColliderPool.Add(typeof(RangeCollider), new Dictionary<string, Queue<AttackCollider>>());
        foreach (var prefabs in prefabList)
        {
            AddPool(prefabs.Value);
        }
    }

    private void AddPool(AttackCollider[] prefabs)
    {
        foreach (var prefab in prefabs)
        {
            if (prefab != null)
                attackColliderPool[prefab.GetType()].Add(prefab.name, new Queue<AttackCollider>());
        }
    }

    public T Get<T>(string id) where T : AttackCollider
    {
        var pool = attackColliderPool[typeof(T)];
        if (pool[id].Count > 0)
        {
            var t = pool[id].Dequeue();
            t.name = id;
            t.Reset();
            return t as T;
        }
        return Instantiate<T>(id);
    }

    private T Instantiate<T>(string id) where T : AttackCollider
    {
        foreach (var prefab in prefabList[typeof(T)])
        {
            if (prefab.name == id)
            {
                var t = Instantiate(prefab);
                t.name = id;
                return t as T;
            }
        }
        return null;
    }

    public void Release<T>(T t) where T : AttackCollider
    {
        t.gameObject.SetActive(false);
        var id = t.name;
        var type = t.GetType();
         if (!attackColliderPool[type].ContainsKey(id))
            return;
        attackColliderPool[type][id].Enqueue(t);
    }
}
