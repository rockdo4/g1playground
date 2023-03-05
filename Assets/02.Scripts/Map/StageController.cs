using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public enum UnLockRequirement
    {
        Fight,
        Puzzle,
        Heal,
    }

    private List<EnemyController> enemies;
    public UnLockRequirement lockRequirement;
    private List<Portal> portals;
    [SerializeField] private List<GameObject> switches;
    private bool canOpen;

    private void OnEnable()
    {
        var switchcheck = false;
        enemies = new List<EnemyController>();
        enemies = gameObject.GetComponentsInChildren<EnemyController>().ToList();
        portals = gameObject.GetComponentsInChildren<Portal>().ToList();

        foreach (var swit in switches)
        {
            if (!swit.GetComponent<BlockSwitchTile>().IsTriggered)
            {
                switchcheck = true;
                break;
            }
        }

        if (enemies.Count > 0 || switchcheck)
            StartCoroutine(DisablePortal());

    }

    IEnumerator DisablePortal()
    {
        yield return null;
        foreach (var portal in portals)
        {
            portal.gameObject.SetActive(false);
            portal.CanUse = true;
        }
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

            if (portals != null)
            {
                foreach (var portal in portals)
                {
                    portal.gameObject.SetActive(true);
                }
            }
        }

        if (lockRequirement == UnLockRequirement.Heal)
        {
            //
        }


    }

    public List<EnemyController> GetStageEnemies()
    {
        return enemies;
    }

}
