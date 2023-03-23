using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GamblingManager : MonoBehaviour
{
    private PlayerInventory inventory;
    public Dictionary<string, DrawData> drawDatas = new Dictionary<string, DrawData>();
    public Dictionary<string, SkillDrawData> skillDrawDatas = new Dictionary<string, SkillDrawData>();
    public GameObject reward;
    public GameObject rewardTen;

    private void Awake()
    {
        inventory = GameManager.instance.player.GetComponent<PlayerInventory>();
        drawDatas = DataTableMgr.GetTable<DrawData>().GetTable();
        skillDrawDatas = DataTableMgr.GetTable<SkillDrawData>().GetTable();
    }

    public void TryOne()
    {
        reward.SetActive(true);
        GetEquipments(1);
    }

    public void TryTen()
    {
        rewardTen.SetActive(true);
         GetEquipments(10);
    }

    public void TryOneSkill()
    {
        GetSkills(1);
    }

    public void TryTenSkills()
    {
        for (int i = 0; i < 10; i++)
        {
            GetSkills(i);
        }
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
                inventory.Weapons.Add(tempId);
                reward.GetComponent<RewardPanel>().OpenRewardPopUp(tempId);
            }
            else
            {
                inventory.Armors.Add(tempId);
                reward.GetComponent<RewardPanel>().OpenRewardPopUp(tempId);
            }
        }
        else
        {
            for (var i = 0; i < count; ++i)
            {
                var tempId = list[i];
                if (int.Parse(tempId[0].ToString()) == 2)
                {
                    inventory.Weapons.Add(tempId);
                }
                else
                {
                    inventory.Armors.Add(tempId);
                }
            }
            rewardTen.GetComponent<RewardPanel>().OpenTenRewardPopUp(list.ToArray());
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
        string tempId;
        tempId = PickSkill();
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
