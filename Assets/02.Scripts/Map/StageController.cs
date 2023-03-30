using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [Range(0, 4)]
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

    [Header("Clear Sound Effect")]
    [SerializeField] private string stageClearClip = "Success 1";

    private List<BlocksOriginStatus> originBlockStatus = new List<BlocksOriginStatus>();

    private bool isClear = false;
    public bool IsClear { get { return isClear; } set { isClear = value; } }

    private List<GameObject> enemies;
    private List<GameObject> objectTiles = new List<GameObject>();

    public UnLockRequirement lockRequirement;
    private List<Portal> portals;
    public List<Portal> Portals { get { return portals; } set { portals = value; } }

    [SerializeField] private List<GameObject> switches;
    private bool canOpen = true;
    [SerializeField]
    private List<GameObject> greenWall;
    private bool greenwallopen = true;

    public string PrevStageName { get; set; }

    public static RectTransform minimapPlayerpos;

    // private EachCorner eachCorner;
    private void Awake()
    {
        SkyboxManager.instance.SetSkybox(skyboxIndex);
        GetObjectTiles();
        SetObjectTileOriginPos();

    }

    //Get all Object tiles
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

    //Save the very first state of the tiles
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

    IEnumerator CSetStageCollider()
    {
        yield return null;

        GetStateColliderEachCornerWorldSpace();

    }

    private void GetStateColliderEachCornerWorldSpace()
    {

        if (MapManager.instance.GetCurrentMapName() != "Village" && SceneManager.GetActiveScene().name == "Scene02")
        {
            MapManager.instance.outlines.Clear();

            var outline = transform.Find("Outline");
            if (outline != null)
            {
                int outlinechildcount = outline.childCount;
                for (int i = 0; i < outlinechildcount; i++)
                {
                    var child = outline.GetChild(i);
                    if (child.GetComponent<BoxCollider>() != null)
                    {
                        MapManager.instance.outlines.Add(child.gameObject);
                    }
                }
            }

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
        if (MiniMap.instance != null)
        {
            MiniMap.instance.SetMiniMap(stageId);
        }


        if (MapManager.instance.currentMapCollider != null )
        {
            StopCoroutine(CSetStageCollider());

            StartCoroutine(CSetStageCollider());

        }

        EventManager.instance.ResetCount();

        
        //Set enemies
        enemies = new List<GameObject>();
        var childCount = gameObject.transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            var child = gameObject.transform.GetChild(i);
            if (child.CompareTag("Enemy"))
                enemies.Add(child.gameObject);
        }

        //Set portals
        portals = gameObject.GetComponentsInChildren<Portal>().ToList();

        //Set Switches
        var switchcheck = false;
        foreach (var swit in switches)
        {
            if (!swit.GetComponent<BlockSwitchTile>().IsTriggered)
            {
                switchcheck = true;
                break;
            }
        }

        //Set Walls
        if (greenWall != null)
        {
            foreach (var green in greenWall)
            {
                green.SetActive(true);
            }
        }

        //Reset objects
        ResetObject();
        SetObjectTileActive();

        //If there is no enemies in the stage
        if (enemies.Count <= 0)
        {
            isClear = true;
            greenwallopen = true;

            //Set worldmap stage button on
            if (wMapButton != null && isChanged)
            {
                isChanged = false;
                SetWorldMapButton();
            }
        }

        if (!isClear)
        {
            if (enemies.Count > 0 || switchcheck)
                StartCoroutine(DisablePortal());
            TileColorManager.instance.ChangeTileMaterial(transform.name, false);
        }
        else
        {
            TileColorManager.instance.ChangeTileMaterial(transform.name, true);
        }
    }

    public void ResetObject()
    {
        foreach (var obj in objectTiles)
        {
            obj.GetComponent<ObjectTile>().ResetObject();
        }
        foreach (var enemy in enemies)
        {
            enemy.SetActive(true);
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
        foreach (var swit in switches)
        {
            if (!swit.GetComponent<BlockSwitchTile>().IsTriggered)
            {
                canOpen = false;
                break;
            }
        }

        //Enemies check
        //Walls check
        //If cleared once
        if (!isClear)
        {
            CheckEnemies();
            CheckWalls();
        }

        if (isClear && isStoryStage) 
        {            
            isStoryStage = false;
            EventManager.instance.SetStoryList(storyIdList);
            EventManager.instance.ChangeColorEffect(transform.name);           
        }

        if (canOpen || isClear) 
        {

            if (portals != null)
            {
                foreach (var portal in portals)
                {
                   
                    portal.gameObject.SetActive(true);
                }
            }
            //Set worldmap stage button on
            if (wMapButton != null && isChanged)
            {
                isChanged = false;
                SetWorldMapButton();
            }
        }

        if (lockRequirement == UnLockRequirement.Heal)
        {
            //
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (var kill in enemies)
            {                
                kill.SetActive(false);
            }
        }
    }

    private void CheckEnemies()
    {
        if (enemies.Count > 0)
        {
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
                    if (!isStoryStage)
                    {
                        EventManager.instance.ChangeColorEffect();
                        TileColorManager.instance.ChangeTileMaterial(transform.name, true);
                    }
                    //Clear Sound
                    SoundManager.instance.PlaySoundEffect(stageClearClip);
                    isClear = true;
                    canOpen = true;
                    greenwallopen = true;

                    if (SceneManager.GetActiveScene().name == "Scene02")
                        MapManager.instance.SaveProgress();

                }
            }
        }
    }

    private void CheckWalls()
    {
        if (greenWall != null)
        {
            if (enemies.Count == 0 || greenwallopen)
            {
                foreach (var green in greenWall)
                {
                    green.SetActive(false);
                }
            }
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

    public bool FindEnemiesActive()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    public void EnemiesReset()
    {
        foreach(var enemy in enemies)
        {
            enemy.SetActive(false);
            enemy.SetActive(true);
        }
    }
}
