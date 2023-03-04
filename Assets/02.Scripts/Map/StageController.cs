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

    private List<EnemyController> enemies;
    public UnLockRequirement lockRequirement;
    private List<Portal> portals;
    [SerializeField] private List<GameObject> switches;
    private bool canOpen;   


    private void OnEnable()
    {
     
        enemies=new List<EnemyController>();
        enemies = gameObject.GetComponentsInChildren<EnemyController>().ToList();
        portals = gameObject.GetComponentsInChildren<Portal>().ToList();

        if (enemies.Count == 0)
        {
            foreach(var portal in portals)
            {
                portal.IsClearroom = true;
            }
        }
        StartCoroutine(DisablePortal());
      
    }

    IEnumerator DisablePortal()
    {
        yield return null;
        foreach (var portal in portals)
        {
            portal.gameObject.SetActive(false);
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

    //private void OnTriggerEnter(Collider other)
    //{

    //    if (other.transform.CompareTag("Player") )
    //    {
    //        MapManager.instance.SetCurrentMapName(transform.name);

    //        foreach (var near in nearStageRoomPrefab)
    //        {
    //            near.SetActive(true);
    //            var nearwall = near.GetComponent<StageController>().GetBlocks();

    //            foreach (var wall in nearwall)
    //            {
    //                wall.gameObject.SetActive(true);
    //            }

    //        }

    //        var mineBlocks = transform.GetComponent<StageController>().GetBlocks();

    //        foreach (var mineblock in mineBlocks)
    //        {
    //            mineblock.SetActive(true);
    //        }

    //    }
    //}

}
