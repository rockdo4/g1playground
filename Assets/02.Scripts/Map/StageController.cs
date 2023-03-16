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

    private List<GameObject> enemies;
    public UnLockRequirement lockRequirement;
    private List<Portal> portals;
    public List<Portal> Portals { get; set; }
    [SerializeField] private List<GameObject> switches;
    private bool canOpen;
    [SerializeField]
    private List<GameObject> greenWall;
    private bool greenwallopen = false;

    private List<GameObject> fallingtill=new List<GameObject>();
    private List<GameObject> bombs = new List<GameObject>();
    private List<GameObject> walls = new List<GameObject>();


    private void Awake()
    {
        //foreach (Transform child in transform)
        //{

        //    //if (child.CompareTag("Falling"))
        //    //    fallingtill.Add(child.gameObject);
        //    //else if(child.CompareTag("Ground"))
        //    //    walls.Add(child.gameObject);
        //    //else if (child.GetComponent<PushBomb>() != null) {                
        //    //    bombs.Add(child.gameObject);
        //    //}
        //}
       
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

    public List<GameObject> GetStageEnemies()
    {
        return enemies;
    }

}
