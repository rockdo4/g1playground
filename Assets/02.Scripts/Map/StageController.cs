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
    private List<GameObject> objectTiles = new List<GameObject>();

    public UnLockRequirement lockRequirement;
    private List<Portal> portals;
    public List<Portal> Portals { get { return portals; } set { portals = value; } }

    [SerializeField] private List<GameObject> switches;
    private bool canOpen;
    [SerializeField]
    private List<GameObject> greenWall;
    private bool greenwallopen = false;

    private List<FallingTile> fallingtile = new List<FallingTile>();
    private List<GameObject> bombs = new List<GameObject>();
    private List<GameObject> walls = new List<GameObject>();

    public string PrevStageName { get; set; }

    private void Awake()
    {
        GetObjectTiles();
        SetObjectTileOriginPos();
    }

    IEnumerator AttachObject()
    {
        yield return null;

        GetObjectTiles();
        
    }

    private void GetObjectTiles()
    {
        var childCount = gameObject.transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            var child = gameObject.transform.GetChild(i);
            
            if (child.gameObject.layer == LayerMask.NameToLayer("ObjectTile"))
            {
                objectTiles.Add(child.gameObject);
            }
                
        }
        //save the objects status
        SaveStatus();
    }

    private void SaveStatus()
    {

        foreach (var block in objectTiles)
        {
            BlocksOriginStatus temp = new BlocksOriginStatus();
            temp.blockObject = block;
            Debug.Log(block.activeSelf);
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
        Debug.Log("block save count: " + originBlockStatus.Count);
    }

    private void SetObjectTileActive()
    {
        foreach (var block in originBlockStatus)
        {
            block.blockObject.SetActive(block.isOn);
        }
    }



    private void OnEnable()
    {
        //GetChildren();
        Debug.Log("Set");
       
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
        
        foreach (var green in greenWall)
        {
            green.SetActive(true);
        }

        if (enemies.Count > 0 || switchcheck)
            StartCoroutine(DisablePortal());

        ResetObject();
        SetObjectTileActive();
        if (!isClear)
        {
            TileColorManager.instance.ChangeTileMaterial(transform.name, false);
        }
    }

    public void ResetObject()
    {
        foreach (var obj in objectTiles)
        {
            //if (obj.GetComponent<FallingTile>() != null || obj.GetComponent<PushBomb>() != null)
            //{
            //    //Debug.Log("enable");
            //    obj.gameObject.SetActive(true);
            //}
            obj.GetComponent<ObjectTile>().ResetObject();
        }
        //foreach (var fall in fallingtile)
        //{
        //    fall.gameObject.SetActive(true);
        //    fall.ResetObject();
        //}
    }

    private void SetObjectTileOriginPos()
    {
        foreach (var obj in objectTiles)
        {
            obj.GetComponent<ObjectTile>().SetOriginPos();
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
