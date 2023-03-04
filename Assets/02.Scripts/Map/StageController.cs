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
    [SerializeField] private List<GameObject> blocks;
    [SerializeField] private List<GameObject> switches;
    private bool canOpen;
    [SerializeField]
    private List<GameObject> nearStageRoomPrefab;


    private void OnEnable()
    {
        //doors = gameObject.transform.GetComponentsInChildren<Connector>().ToList();
        //foreach (var door in doors)
        //{
        //    door.gameObject.SetActive(false);
        //}
        enemies=new List<EnemyController>();
        enemies = gameObject.GetComponentsInChildren<EnemyController>().ToList();

    }

    public List<GameObject> GetBlocks()
    {
        return blocks;
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

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.CompareTag("Player") && nearStageRoomPrefab.Count != 0)
        {
            Debug.Log("Enter");
            MapManager.instance.SetCurrentMapName(transform.name);

            foreach (var near in nearStageRoomPrefab)
            {
                near.SetActive(true);
                var nearwall = near.GetComponent<StageController>().GetBlocks();

                foreach (var wall in nearwall)
                {
                    wall.gameObject.SetActive(true);
                }

            }

            var mineBlocks = transform.GetComponent<StageController>().GetBlocks();

            foreach (var mineblock in mineBlocks)
            {
                mineblock.SetActive(true);
            }

        }
    }

}
