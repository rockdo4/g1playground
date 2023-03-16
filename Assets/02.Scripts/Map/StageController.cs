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
    private struct BlocksOriginStatus
    {
        public GameObject blockObject;
        public bool isOn;
    }

    private List<BlocksOriginStatus> originBlockStatus = new List<BlocksOriginStatus>();

    private bool isClear = false;
    private List<GameObject> enemies;
    public UnLockRequirement lockRequirement;
    private List<Portal> portals;
    public List<Portal> Portals { get; set; }
    [SerializeField] private List<GameObject> switches;
    private bool canOpen;
    [SerializeField]
    private List<GameObject> greenWall;
    private bool greenwallopen = false;

    private List<FallingTile> fallingtile = new List<FallingTile>();
    private List<GameObject> bombs = new List<GameObject>();
    private List<GameObject> walls = new List<GameObject>();


    private void Awake()
    {

        StartCoroutine(AttachObject());
        //SaveStatus();
    }
    private void GetChildren()
    {
        List<Transform> child = new List<Transform>();
        var childCount = gameObject.transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            child.Add(gameObject.transform.GetChild(i));

        }
        //Debug.Log("child "+child.Count);

        foreach (var ob in child)
        {
            if (ob.GetComponentsInChildren<FallingTile>() != null)
            {
                var fallingTiles = ob.GetComponentsInChildren<FallingTile>().ToList();
                foreach (var falling in fallingTiles)
                {
                    fallingtile.Add(falling);
                }
            }
        }
        //Debug.Log(fallingtile.Count);
    }

    private void SaveStatus()
    {
        List<Transform> child = new List<Transform>();
        var childCount = gameObject.transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            child.Add(gameObject.transform.GetChild(i));

        }

        foreach (var ob in child)
        {
            var grandChildrenCount = ob.childCount;
            for (int i = 0; i < grandChildrenCount; ++i)
            {
                var block = ob.GetChild(i).gameObject;
                if (block.GetComponent<IResetObject>() != null)
                {
                    BlocksOriginStatus temp = new BlocksOriginStatus();
                    temp.blockObject = block;

                    if (block.activeSelf)
                    {
                        temp.isOn = true;

                    }
                    else
                    {
                        temp.isOn = false;
                    }
                    originBlockStatus.Add(temp);
                }
            }
            Debug.Log(originBlockStatus.Count);
        }

    }
    IEnumerator AttachObject()
    {
        yield return null;

        GetChildren();

    }



    private void OnEnable()
    {
        var switchcheck = false;
        enemies = new List<GameObject>();
        var childCount = gameObject.transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            var child = gameObject.transform.GetChild(i);
            if (child.CompareTag("Enemy"))
                enemies.Add(child.gameObject);
        }
        portals = gameObject.GetComponentsInChildren<Portal>().ToList();

        foreach (var swit in switches)
        {
            if (!swit.GetComponent<BlockSwitchTile>().IsTriggered)
            {
                switchcheck = true;
                break;
            }
        }
        //    TileColorManager.instance.ChangeTileMaterial(transform.name, false);

        foreach (var green in greenWall)
        {
            green.SetActive(true);
        }

        if (enemies.Count > 0 || switchcheck)
            StartCoroutine(DisablePortal());

        ResetObject();

        if (!isClear)
        {
            TileColorManager.instance.ChangeTileMaterial(transform.name, false);
        }
    }

    public void ResetObject()
    {
        //foreach (var block in originBlockStatus)
        //{
        //    if (block.blockObject.GetComponent<BoxTile>() != null)
        //    {
        //        block.blockObject.GetComponent<BoxTile>().ResetObject();
        //        Debug.Log("block");
        //    }

        //    //block.blockObject.SetActive(block.isOn);
        //}

        foreach (var fall in fallingtile)
        {
            fall.gameObject.SetActive(true);
            fall.ResetObject();
        }
    }

    public void PortalClose()
    {
        foreach (var portal in portals)
        {
            portal.gameObject.SetActive(false);
            //portal.CanUse = true;
        }
    }
    public void PortalOpen()
    {
        foreach (var portal in portals)
        {
            portal.gameObject.SetActive(true);
        }
    }
    IEnumerator DisablePortal()
    {
        yield return null;
        foreach (var portal in portals)
        {
            portal.gameObject.SetActive(false);
            portal.CanUse = false;
        }
    }

    private void Update()
    {
        canOpen = true;
        greenwallopen = true;

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
                greenwallopen = false;
                break;
            }
            else if (enemy == enemies.Last())
            {
                greenwallopen = true;
            }
        }

        if (enemies.Count == 0 || greenwallopen)
        {
            foreach (var green in greenWall)
            {
                green.SetActive(false);
            }
        }

        if (greenwallopen)
        {
            foreach (var green in greenWall)
            {
                green.SetActive(false);
                TileColorManager.instance.ChangeTileMaterial(transform.name, true);

            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (var kill in enemies)
            {
                kill.SetActive(false);
            }
        }

        if (canOpen)
        {

            if (portals != null)
            {
                foreach (var portal in portals)
                {
                    isClear = true;
                    portal.gameObject.SetActive(true);
                    TileColorManager.instance.ChangeTileMaterial(transform.name, true);
                }
            }
        }

        if (lockRequirement == UnLockRequirement.Heal)
        {
            //
        }


    }

    public List<GameObject> GetStageEnemies()
    {
        return enemies;
    }

}
