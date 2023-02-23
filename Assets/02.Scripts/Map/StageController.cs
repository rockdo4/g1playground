using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public enum UnLockRequirement
    {
        Fight,
        Puzzle,
        Heal,
    }

    private List<Connector> doors;
    private List<Enemy> enemies;   
    public UnLockRequirement lockRequirement;

    private void OnEnable()
    {
        doors=gameObject.transform.GetComponentsInChildren<Connector>().ToList();
        foreach (var door in doors)
        {
            door.gameObject.SetActive(false);
        }

        enemies = gameObject.GetComponentsInChildren<Enemy>().ToList();

    }

    private void Update()
    {
        enemies.Clear();
        enemies = gameObject.GetComponentsInChildren<Enemy>().ToList();

        if (enemies.Count==0)
        {
            foreach (var door in doors)
            {
                door.gameObject.SetActive(true);
            }
        }
    }



}
