using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Skybox")]
    [Range(0,4)]
    [SerializeField] private int skyboxIndex;

    [Header("StageID")]
    [SerializeField] private int stageId = 0;
    public int StageId { get { return StageId; } }

    [Header("World Map Button")]
    [SerializeField] private Button wMapButton;
    [SerializeField] private TileColorManager.ImageColor imageColor;
    private bool isChanged = true;

    [Header("Stage Story Event")]
    [SerializeField] private bool isStoryStage = false;
    [SerializeField] List<int> storyIdList = new List<int>();

    private List<BlocksOriginStatus> originBlockStatus = new List<BlocksOriginStatus>();

    private bool isClear = false;
    public bool IsClear { get { return isClear; } }

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

    public string PrevStageName { get; set; }

    private void Awake()
    {
        SkyboxManager.instance.SetSkybox(skyboxIndex);
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

    private void SetObjectTileActive()
    {
        foreach (var block in originBlockStatus)
        {
            block.blockObject.SetActive(block.isOn);
        }
    }



    private void OnEnable()
    {
        //Set Skybox
        SkyboxManager.instance.SetSkybox(skyboxIndex);

        //Show StageName
        if (DataTableMgr.GetTable<StageNameData>().Get(stageId.ToString()) != null) 
        {
            var stageName = DataTableMgr.GetTable<StageNameData>().Get(stageId.ToString()).stageName;
            EventManager.instance.ShowStageTitile(stageName);
        }
        

        //Change MiniMap
        MiniMap.instance.SetMiniMap(stageId);

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
            obj.GetComponent<ObjectTile>().ResetObject();
        }

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
            if (isStoryStage)
            {
                isStoryStage = false;
                EventManager.instance.SetStoryList(storyIdList);
                EventManager.instance.PlayStory();
                EventManager.instance.Pause();
            }
            //TileColorManager.instance.PlayEffect();
            TileColorManager.instance.ChangeTileMaterial(transform.name, true);
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
                    //Set worldmap stage button on
                    if (wMapButton != null && isChanged)
                    {
                        isChanged = false;
                        SetWorldMapButton();
                    }
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

    private void SetWorldMapButton()
    {
        wMapButton.interactable = true;
        wMapButton.GetComponent<Image>().sprite = TileColorManager.instance.GetImageSprite(imageColor);
    }
}
