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
    private List<EnemyController> enemies;
    public UnLockRequirement lockRequirement;
    [SerializeField] private List<GameObject> blocks;
    [SerializeField] private List<GameObject> switches;
    private bool canOpen;

    private void OnEnable()
    {
        //doors = gameObject.transform.GetComponentsInChildren<Connector>().ToList();
        //foreach (var door in doors)
        //{
        //    door.gameObject.SetActive(false);
        //}
        enemies=new List<EnemyController>();
        doors=new List<Connector>();
        enemies = gameObject.GetComponentsInChildren<EnemyController>().ToList();

    }

    private void Update()
    {
        canOpen = true;
        foreach (var swit in switches)
        {
            if (!swit.GetComponent<BlockSwitchTile>().IsTriggered)
            {
                canOpen = false;
                break;
            }
        }
        foreach (var enemy in enemies)
        {
            if (enemy.gameObject.activeSelf)
            {
                canOpen = false;
                break;
            }
        }

        if (canOpen)
        {

            foreach (var door in doors)
            {
                door.gameObject.SetActive(true);
            }

            if (blocks != null)
            {
                foreach (var block in blocks)
                {

                    block.SetActive(false);

                }

            }

        }

        if (lockRequirement == UnLockRequirement.Heal)
        {
            //
        }
    }

    public List<EnemyController> GetEnemyList()
    {
        return enemies;
    }

}
