using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GamblingManager : MonoBehaviour
{
    private PlayerInventory playerInventory;
    private PlayerSkills playerSkills;
    public Dictionary<string, DrawData> drawDatas = new Dictionary<string, DrawData>();
    public Dictionary<string, SkillDrawData> skillDrawDatas = new Dictionary<string, SkillDrawData>();
    public GameObject reward;
    public GameObject rewardTen;
    public GameObject materialPopUp;

    public TextMeshProUGUI skillPowderCount;
    public TextMeshProUGUI equipPowderCount;

    private UIInventory itemInventory;
    private UISkillInventory skillInventory;

    private void Awake()
    {
        playerInventory = GameManager.instance.player.GetComponent<PlayerInventory>();
        playerSkills = GameManager.instance.player.GetComponent<PlayerSkills>();
        drawDatas = DataTableMgr.GetTable<DrawData>().GetTable();
        skillDrawDatas = DataTableMgr.GetTable<SkillDrawData>().GetTable();
        itemInventory = GameManager.instance.uiManager.itemInventory.GetComponent<UIInventory>();
        skillInventory = GameManager.instance.uiManager.skillInventory.GetComponent<UISkillInventory>();
    }

    private void OnEnable()
    {
        ShowPowderCount();
    }

    private void ShowPowderCount()
    {
        var count = GetPowderCount();
        skillPowderCount.text = count.ToString();
        equipPowderCount.text = count.ToString();
    }

    public int GetPowderCount()
    {
        var consumeTable = DataTableMgr.GetTable<ConsumeData>().GetTable();
        string powderId = null;
        foreach (var data in consumeTable)
        {
            if (data.Value.consumeType == ConsumeTypes.Powder)
            {
                powderId = data.Value.id;
                break;
            }
        }
        return playerInventory.GetConsumableCount(powderId);
    }

    public void TryOne()
    {
        var consumeTable = DataTableMgr.GetTable<ConsumeData>().GetTable();
        string powderId = null;
        foreach (var data in consumeTable)
        {
            if (data.Value.consumeType == ConsumeTypes.Powder)
            {
                powderId = data.Value.id;
                break;
            }
        }
        if (GetPowderCount() >= 300)
        {
            playerInventory.UseConsumable(powderId, 300);
            reward.SetActive(true);
            GetEquipments(1);
            ShowPowderCount();
        }
        else
            materialPopUp.SetActive(true);
    }

    public void TryTen()
    {
        var consumeTable = DataTableMgr.GetTable<ConsumeData>().GetTable();
        string powderId = null;
        foreach (var data in consumeTable)
        {
            if (data.Value.consumeType == ConsumeTypes.Powder)
            {
                powderId = data.Value.id;
                break;
            }
        }
        if (GetPowderCount() >= 2700)
        {
            playerInventory.UseConsumable(powderId, 2700);
            rewardTen.SetActive(true);
            GetEquipments(10);
            ShowPowderCount();
        }
        else
            materialPopUp.SetActive(true);
    }

    public void TryOneSkill()
    {
        var consumeTable = DataTableMgr.GetTable<ConsumeData>().GetTable();
        string powderId = null;
        foreach (var data in consumeTable)
        {
            if (data.Value.consumeType == ConsumeTypes.Powder)
            {
                powderId = data.Value.id;
                break;
            }
        }
        if (GetPowderCount() >= 300)
        {
            playerInventory.UseConsumable(powderId, 300);
            reward.SetActive(true);
            GetSkills(1);
            ShowPowderCount();
        }        
        else
            materialPopUp.SetActive(true);
    }

    public void TryTenSkills()
    {
        var consumeTable = DataTableMgr.GetTable<ConsumeData>().GetTable();
        string powderId = null;
        foreach (var data in consumeTable)
        {
            if (data.Value.consumeType == ConsumeTypes.Powder)
            {
                powderId = data.Value.id;
                break;
            }
        }
        if (GetPowderCount() >= 2700)
        {
            playerInventory.UseConsumable(powderId, 2700);
            rewardTen.SetActive(true);
            GetSkills(10);
            ShowPowderCount();
        }                
        else
            materialPopUp.SetActive(true);
    }

    public void GetEquipments(int count)
    {
        var list = new List<string>();
        for (var i = 0; i < count; ++i)
        {
            list.Add(PickEquip());
        }

        if (count == 1)
        {
            var tempId = list[0];
            if (int.Parse(tempId[0].ToString()) == 2)
            {
                playerInventory.AddWeapon(tempId);
                reward.GetComponent<RewardPanel>().OpenRewardPopUp(tempId);
                // 무기 갯수
            }
            else
            {
                playerInventory.AddArmor(tempId);
                reward.GetComponent<RewardPanel>().OpenRewardPopUp(tempId);
                // 방어구 갯수
            }
        }
        else
        {
            for (var i = 0; i < count; ++i)
            {
                var tempId = list[i];
                if (int.Parse(tempId[0].ToString()) == 2)
                {
                    playerInventory.AddWeapon(tempId);
                }
                else
                {
                    playerInventory.AddArmor(tempId);
                }
            }
            rewardTen.GetComponent<RewardPanel>().OpenTenRewardPopUp(list.ToArray());
        }

        int weaponRemainder = itemInventory.slotCount - playerInventory.Weapons.Count;
        int armorRemainder = itemInventory.slotCount - playerInventory.Armors.Count;

        if (weaponRemainder <= count || armorRemainder <= count)
        {
            if ((weaponRemainder < 5 && count <= 5) || (armorRemainder < 5 && count <= 5))
            {
                itemInventory.SlotInstantiate(5);
            }
            else if ((weaponRemainder < 10 && count > 5) || (armorRemainder < 10 && count > 5))
            {
                itemInventory.SlotInstantiate(10);
            }
        }
    }

    public string PickEquip()
    {
        float total = 0f;
        float currTotal = 0f;

        foreach (var drawData in drawDatas)
        {
            total += drawData.Value.rate;
        }

        float random = Random.Range(0, total);
        foreach (var drawData in drawDatas)
        {
            currTotal += drawData.Value.rate;
            if (random <= currTotal)
            {
                return drawData.Value.itemId;
            }
        }
        return null;
    }

    public void GetSkills(int count)
    {
        var list = new List<string>();
        for (var i = 0; i < count; ++i)
        {
            list.Add(PickSkill());
        }

        if (count == 1)
        {
            var tempId = list[0];
            playerSkills.AddSkill(tempId);
            reward.GetComponent<RewardPanel>().OpenRewardPopUp(tempId);
        }
        else
        {
            for (var i = 0; i < count; ++i)
            {
                var tempId = list[i];
                playerSkills.AddSkill(tempId);
            }
            rewardTen.GetComponent<RewardPanel>().OpenTenRewardPopUp(list.ToArray());
        }

        int remainder = skillInventory.slotCount - playerSkills.PossessedSkills.Count;

        if (remainder <= count)
        {
            if (remainder < 5 && count <= 5)
            {
                skillInventory.SlotInstantiate(5);
            }
            else if(remainder < 10 && count > 5)
            {
                skillInventory.SlotInstantiate(10);
            }
        }
    }

    public string PickSkill()
    {
        float total = 0f;
        float currTotal = 0f;

        foreach (var skillDrawData in skillDrawDatas)
        {
            total += skillDrawData.Value.rate;
        }

        float random = Random.Range(0, total);
        foreach (var skillDrawData in skillDrawDatas)
        {
            currTotal += skillDrawData.Value.rate;
            if (random <= currTotal)
            {
                return skillDrawData.Value.skillId;
            }
        }
        return null;
    }
}
