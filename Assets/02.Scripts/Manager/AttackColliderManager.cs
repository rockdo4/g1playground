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
    private Dictionary<Type, Dictionary<string, LinkedList<AttackCollider>>> usingList = new();

    private void Start()
    {
        prefabList.Add(typeof(Projectile), projectilePrefabs);
        prefabList.Add(typeof(RangeCollider), rangeColliderPrefabs);
        attackColliderPool.Add(typeof(Projectile), new Dictionary<string, Queue<AttackCollider>>());
        attackColliderPool.Add(typeof(RangeCollider), new Dictionary<string, Queue<AttackCollider>>());
        usingList.Add(typeof(Projectile), new Dictionary<string, LinkedList<AttackCollider>>());
        usingList.Add(typeof(RangeCollider), new Dictionary<string, LinkedList<AttackCollider>>()); 
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
            {
                attackColliderPool[prefab.GetType()].Add(prefab.name, new Queue<AttackCollider>());
                usingList[prefab.GetType()].Add(prefab.name, new LinkedList<AttackCollider>());
            }
        }
    }

    public T Get<T>(string id) where T : AttackCollider
    {
        var pool = attackColliderPool[typeof(T)];
        if (pool[id].Count > 0)
        {
            var t = pool[id].Dequeue();
            usingList[typeof(T)][id].AddLast(t);
            t.name = id;
            t.Reset();
            return t as T;
        }
        var collider = Instantiate<T>(id);
        usingList[typeof(T)][id].AddLast(collider);
        return collider;
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
        var id = t.name;
        var type = t.GetType();
        if (!usingList[type].ContainsKey(t.name) || !usingList[type][t.name].Contains(t))
            return;
        if (!attackColliderPool[type].ContainsKey(id))
            return;
        t.Reset();
        t.gameObject.SetActive(false);
        usingList[type][t.name].Remove(t);
        attackColliderPool[type][id].Enqueue(t);
    }

    public void ReleaseAll()
    {
        foreach (var dict in usingList)
        {
            foreach (var list in dict.Value)
            {
                foreach(var collider in list.Value)
                {
                    switch (collider)
                    {
                        case Projectile:
                            Release((Projectile)collider);
                            break;
                        case RangeCollider:
                            Release((RangeCollider)collider);
                            break;
                    }
                }
            }
        }
    }
}
