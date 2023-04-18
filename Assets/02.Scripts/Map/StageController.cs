using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StageController : MonoBehaviour
{
    public enum UnLockRequirement
    {
        Fight,
        Puzzle,
        Heal,
        Tutorial,
        Village,
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
    public bool IsStoryStage { get { return isStoryStage; } set { isStoryStage = value; } }
    [SerializeField] List<int> storyIdList = new List<int>();



    [Header("Clear Sound Effect")]
    [SerializeField] private string stageClearClip = "Success 1";

    private List<BlocksOriginStatus> originBlockStatus = new List<BlocksOriginStatus>();

    private bool isClear = false;
    public bool IsClear { get { return isClear; } set { isClear = value; } }

    private List<GameObject> enemies;
    private List<GameObject> objectTiles = new List<GameObject>();

    public UnLockRequirement unlockRequirement;
    private List<Portal> portals;
    public List<Portal> Portals { get { return portals; } set { portals = value; } }

    [SerializeField] private List<GameObject> switches;
    private bool canOpen = true;
    [SerializeField]
    private List<GameObject> greenWall;
    private bool greenwallopen = true;

    [SerializeField]
    private GameObject RewardBox;

    public string PrevStageName { get; set; }

    public static RectTransform minimapPlayerpos;

    private bool rewarded = false;
    [SerializeField]
    private int firstRewardId;
    [SerializeField]
    private int secondRewardId;
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



        if (MapManager.instance.currentMapCollider != null)
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
            if (unlockRequirement != UnLockRequirement.Puzzle)
            {
                isClear = true;
                greenwallopen = true;

            }


            //Set worldmap stage button on
            if ((wMapButton != null && isChanged) && UnLockRequirement.Puzzle != unlockRequirement)
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
        if (unlockRequirement == UnLockRequirement.Puzzle)
        {
            TileColorManager.instance.ChangeTileMaterial(transform.name, true);
        }
        //SetUi();
        StartCoroutine(CoSetUI());


        if (SceneManager.GetActiveScene().name == "Scene02")
        {
            var playerdeath = GameManager.instance.player.GetComponent<DestructedEvent>();

            if (playerdeath != null)
            {
                playerdeath.OnDestroyEvent = (() => UI.Instance.popupPanel.stageDeathPopUp.ActiveTrue());
            }
        }
        if (RewardBox != null)
            RewardBox.SetActive(false);
        rewarded = false;
    }

    void SetUi()
    {
        switch (unlockRequirement)
        {
            case UnLockRequirement.Fight:
                UI.Instance.SetBattle();
                break;
            case UnLockRequirement.Puzzle:
                UI.Instance.SetVillageUi();
                break;
            case UnLockRequirement.Heal:
                UI.Instance.SetVillageUi();
                break;
        }
    }
    IEnumerator CoSetUI()
    {
        yield return null;
        //UI setting
        switch (unlockRequirement)
        {
            case UnLockRequirement.Fight:
                UI.Instance.SetBattle();
                break;
            case UnLockRequirement.Puzzle:
                UI.Instance.SetVillageUi();
                break;
            case UnLockRequirement.Heal:
                UI.Instance.SetVillageUi();
                break;
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
        if (!isClear && UnLockRequirement.Puzzle == unlockRequirement)
        {
            if (RewardBox != null)
            {
                RewardBox.SetActive(true);
            }
        }
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
            MapManager.instance.SaveProgress();
            EventManager.instance.SetStoryList(storyIdList);
            EventManager.instance.ChangeColorEffectStory(transform.name);
        }

        if (isClear && !rewarded)
        {
            rewardCheck();
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
            if ((wMapButton != null && isChanged) && unlockRequirement != UnLockRequirement.Puzzle)
            {
                isChanged = false;
                SetWorldMapButton();
            }
        }

        if (unlockRequirement == UnLockRequirement.Heal)
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

    void rewardCheck()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.gameObject.GetComponent<Enemy>().GetIsLive())
            {

                break;
            }
            else if (enemy == enemies.Last())
            {

                if (unlockRequirement == UnLockRequirement.Fight)
                {
                    SetReward(true);
                    rewarded = true;

                }

                if (RewardBox != null)
                {
                    RewardBox.SetActive(true);
                    RewardBox.GetComponent<GoldBox>().isFirst = IsClear;
                    rewarded = true;

                }
            }
        }
    }

    public void SetReward(bool isSecond)
    {
        if (SceneManager.GetActiveScene().name != "Scene02")
        {
            return;
        }
        //  GameManager.instance.ui.popupPanel.gameObject.SetActive(false);
        GameManager.instance.ui.popupPanel.gameObject.GetComponentInChildren<StageRewardPopUp>(true).gameObject.SetActive(true);
        var rewardUi = GameManager.instance.ui.popupPanel.GetComponentInChildren<StageRewardPopUp>(true).transform.Find("StageReward").Find("RewardItems").gameObject;

        if (UnLockRequirement.Puzzle == unlockRequirement)
        {
            SetWorldMapButton();

            isClear = true;
            if (SceneManager.GetActiveScene().name == "Scene02")
                MapManager.instance.SaveProgress();
        }

        List<GameObject> rewardUiList = new List<GameObject>();

        var rewardTable = DataTableMgr.GetTable<RewardData>();
        var powder = rewardTable.Get(firstRewardId.ToString()).powder;
        var essnece = rewardTable.Get(firstRewardId.ToString()).essence;
     
        var exp = rewardTable.Get(firstRewardId.ToString()).exp;
        rewardTable = DataTableMgr.GetTable<RewardData>();

        if (!isSecond)
        {
            powder = rewardTable.Get(firstRewardId.ToString()).powder;
            essnece = rewardTable.Get(firstRewardId.ToString()).essence;          
            exp = rewardTable.Get(firstRewardId.ToString()).exp;
        }
        else
        {
            powder = rewardTable.Get(secondRewardId.ToString()).powder;
            essnece = rewardTable.Get(secondRewardId.ToString()).essence;            
            exp = rewardTable.Get(secondRewardId.ToString()).exp;
        }

        for (int i = 0; i < rewardUi.transform.childCount; i++)
        {
            rewardUiList.Add(rewardUi.transform.GetChild(i).gameObject);
        }


        if (powder != 0)
        {
            rewardUiList[0].SetActive(true);
            rewardUiList[0].transform.Find("Count").GetComponent<TextMeshProUGUI>().text = powder.ToString();
            GameManager.instance.player.GetComponent<PlayerInventory>().AddConsumable("40003", powder);
        }
        else
        {
            rewardUiList[0].SetActive(false);
        }

        if (essnece != 0)
        {
            rewardUiList[1].SetActive(true);
            rewardUiList[1].transform.Find("Count").GetComponent<TextMeshProUGUI>().text = essnece.ToString();
            GameManager.instance.player.GetComponent<PlayerInventory>().AddConsumable("40004", essnece);
        }
        else
        {
            rewardUiList[1].SetActive(false);
        }
        if (exp != 0)
        {
            rewardUiList[4].SetActive(true);
            rewardUiList[4].transform.Find("Count").GetComponent<TextMeshProUGUI>().text = exp.ToString();
            GameManager.instance.player.GetComponent<PlayerLevelManager>().CurrExp += exp;
        }
        else
        {
            rewardUiList[4].SetActive(false);
        }
       

    }

    private void CheckEnemies()
    {
        if (enemies.Count > 0)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.gameObject.GetComponent<Enemy>().GetIsLive())
                {
                    canOpen = false;
                    greenwallopen = false;
                    break;
                }
                else if (enemy == enemies.Last())
                {
                    if (!isStoryStage)
                    {
                        EventManager.instance.ChangeColorEffect(transform.name);
                        //TileColorManager.instance.ChangeTileMaterial(transform.name, true);
                    }
                    //Clear Sound
                    SoundManager.instance.PlaySoundEffect(stageClearClip);
                    if (RewardBox != null)
                    {
                        RewardBox.GetComponent<GoldBox>().isFirst = IsClear;
                    }
                    if (unlockRequirement == UnLockRequirement.Fight)
                    {
                        SetReward(false);

                    }
                    if (unlockRequirement == UnLockRequirement.Fight || unlockRequirement == UnLockRequirement.Tutorial)
                        isClear = true;

                    canOpen = true;
                    greenwallopen = true;
                    rewarded = true;
                    if (RewardBox != null)
                    {
                        RewardBox.SetActive(true);
                    }

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

    public void SetWorldMapButton()
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
        foreach (var enemy in enemies)
        {
            enemy.SetActive(false);
            enemy.SetActive(true);
        }
    }
}
