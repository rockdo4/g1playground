using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public int poolSize = 10;

    private List<GameObject>[] enemyPools;

    void Start()
    {
        enemyPools = new List<GameObject>[enemyPrefabs.Length];

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemyPools[i] = new List<GameObject>();

            for (int j = 0; j < poolSize; j++)
            {
                GameObject enemy = Instantiate(enemyPrefabs[i]);
                enemy.SetActive(false);
                enemyPools[i].Add(enemy);
            }
        }
    }

    public GameObject GetPooledEnemy(int index)
    {
        if (index < 0 || index >= enemyPrefabs.Length)
        {
            return null;
        }

        foreach (GameObject enemy in enemyPools[index])
        {
            if (!enemy.activeInHierarchy)
            {
                return enemy;
            }
        }

        GameObject newEnemy = Instantiate(enemyPrefabs[index]);
        newEnemy.SetActive(false);
        enemyPools[index].Add(newEnemy);
        return newEnemy;
    }
}
