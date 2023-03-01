using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileManager : MonoBehaviour
{
    public Projectile[] projectilePrefabs;
    private Dictionary<string, Queue<Projectile>> projectilePool = new Dictionary<string, Queue<Projectile>>();

    private void Start()
    {
        foreach (var prefab in projectilePrefabs)
        {
            if (prefab != null)
                projectilePool.Add(prefab.name, new Queue<Projectile>());
        }
    }
    public Projectile Get(string id)
    {
        if (projectilePool[id].Count > 0)
        {
            var projectile = projectilePool[id].Dequeue();
            projectile.name = id;
            projectile.OnCollided = null;
            projectile.gameObject.SetActive(true);
            return projectile;
        }

        foreach (var prefab in projectilePrefabs)
        {
            if (prefab.name == id)
            {
                var projectile = Instantiate(prefab);
                projectile.name = id;
                return projectile;
            }
        }
        return null;
    }

    public void Release(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        var id = projectile.name;
        if (!projectilePool.ContainsKey(id))
            return;
        projectilePool[id].Enqueue(projectile);
    }
}
