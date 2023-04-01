using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiReinforceInfo : MonoBehaviour
{
    public Image resultImage;
    public TextMeshProUGUI resultName;
    public TextMeshProUGUI resultDesc;
    public Image material1;
    public Image material2;
    public Image material3;
    private Sprite powderSprite;
    private Sprite essenceSprite;
    public GameObject resultPopup;
    private bool isLoaded = false;

    private void OnDisable()
    {
        StopAllCoroutines();
        resultPopup.SetActive(false);
    }

    private void Load()
    {
        var consumeTable = DataTableMgr.GetTable<ConsumeData>().GetTable();
        var iconTable = DataTableMgr.GetTable<IconData>();
        foreach (var consumeData in consumeTable)
        {
            switch (consumeData.Value.consumeType)
            {
                case ConsumeTypes.Powder:
                    powderSprite = Resources.Load<Sprite>(iconTable.Get(consumeData.Value.iconSpriteId).iconName);
                    break;
                case ConsumeTypes.Essence:
                    essenceSprite = Resources.Load<Sprite>(iconTable.Get(consumeData.Value.iconSpriteId).iconName);
                    break;
            }
        }
    }

    public void SetEmpty()
    {
        resultImage.sprite = null;
        material1.sprite = null;
        material2.sprite = null;
        material3.sprite = null;
        resultImage.color = Color.clear;
        material1.color = Color.clear;
        material2.color = Color.clear;
        material3.color = Color.clear;
        resultName.text = string.Empty;
        resultDesc.text = string.Empty;
    }

    public bool Set(ReinforceSystem.Types type, string id)
    {
        if (!isLoaded)
        {
            isLoaded = true;
            Load();
        }

        if (!ReinforceSystem.CheckReinforcable(id))
        {
            ShowPopUp("이 아이템은 더 이상 강화할 수 없습니다");
            SetEmpty();
            return false;
        }

        var data = ReinforceSystem.GetData(id);
        var weaponTable = DataTableMgr.GetTable<WeaponData>();
        var armorTable = DataTableMgr.GetTable<ArmorData>();
        var skillTable = DataTableMgr.GetTable<SkillData>();
        var consumeTable = DataTableMgr.GetTable<ConsumeData>();
        var iconTable = DataTableMgr.GetTable<IconData>();
        var nameTable = DataTableMgr.GetTable<NameData>();
        var descTable = DataTableMgr.GetTable<DescData>();

        //if (!ReinforceSystem.CheckMaterials(type, id))
        //{
        //    ShowPopUp("재료가 부족합니다");
        //    SetEmpty();
        //    resultImage.color = Color.white;
        //    material1.color = Color.white;
        //    switch (type)
        //    {
        //        case ReinforceSystem.Types.Weapon:
        //            resultImage.sprite = Resources.Load<Sprite>(iconTable.Get(weaponTable.Get(data.result).iconSpriteId).iconName);
        //            resultName.text = nameTable.Get(weaponTable.Get(data.result).name).name;
        //            resultDesc.text = descTable.Get(weaponTable.Get(data.result).desc).text;
        //            material1.sprite = Resources.Load<Sprite>(iconTable.Get(weaponTable.Get(data.material1).iconSpriteId).iconName);
        //            break;
        //        case ReinforceSystem.Types.Armor:
        //            resultImage.sprite = Resources.Load<Sprite>(iconTable.Get(armorTable.Get(data.result).iconSpriteId).iconName);
        //            resultName.text = nameTable.Get(armorTable.Get(data.result).name).name;
        //            resultDesc.text = descTable.Get(armorTable.Get(data.result).desc).text;
        //            material1.sprite = Resources.Load<Sprite>(iconTable.Get(armorTable.Get(data.material1).iconSpriteId).iconName);
        //            break;
        //        case ReinforceSystem.Types.Skill:
        //            resultImage.sprite = Resources.Load<Sprite>(iconTable.Get(skillTable.Get(data.result).iconSpriteId).iconName);
        //            resultName.text = nameTable.Get(skillTable.Get(data.result).name).name;
        //            resultDesc.text = descTable.Get(skillTable.Get(data.result).desc).text;
        //            material1.sprite = Resources.Load<Sprite>(iconTable.Get(skillTable.Get(data.material1.ToString()).iconSpriteId).iconName);
        //            break;
        //    }
        //    return false;
        //}

        resultImage.color = Color.white;
        material1.color = Color.white;
        material2.color = Color.white;
        material3.color = Color.white;
        switch (type)
        {
            case ReinforceSystem.Types.Weapon:
                resultImage.sprite = Resources.Load<Sprite>(iconTable.Get(weaponTable.Get(data.result).iconSpriteId).iconName);
                resultName.text = nameTable.Get(weaponTable.Get(data.result).name).name;
                resultDesc.text = descTable.Get(weaponTable.Get(data.result).desc).text;
                material1.sprite = Resources.Load<Sprite>(iconTable.Get(weaponTable.Get(data.material1).iconSpriteId).iconName);
                material2.sprite = powderSprite;
                material3.sprite = essenceSprite;
                break;
            case ReinforceSystem.Types.Armor:
                resultImage.sprite = Resources.Load<Sprite>(iconTable.Get(armorTable.Get(data.result).iconSpriteId).iconName);
                resultName.text = nameTable.Get(armorTable.Get(data.result).name).name;
                resultDesc.text = descTable.Get(armorTable.Get(data.result).desc).text;
                material1.sprite = Resources.Load<Sprite>(iconTable.Get(armorTable.Get(data.material1).iconSpriteId).iconName);
                material2.sprite = powderSprite;
                material3.sprite = essenceSprite;
                break;
            case ReinforceSystem.Types.Skill:
                resultImage.sprite = Resources.Load<Sprite>(iconTable.Get(skillTable.Get(data.result).iconSpriteId).iconName);
                resultName.text = nameTable.Get(skillTable.Get(data.result).name).name;
                resultDesc.text = descTable.Get(skillTable.Get(data.result).desc).text;
                material1.sprite = Resources.Load<Sprite>(iconTable.Get(skillTable.Get(data.material1.ToString()).iconSpriteId).iconName);
                material2.sprite = Resources.Load<Sprite>(iconTable.Get(ReinforceSystem.GetSkillMaterial(data.material1).iconSpriteId).iconName);
                material3.sprite = powderSprite;
                break;
        }
        return true;
    }
    public void ShowPopUp(string text) => StartCoroutine(CoShowPopup(text));

    public IEnumerator CoShowPopup(string text)
    {
        resultPopup.SetActive(true);
        resultPopup.GetComponentInChildren<TextMeshProUGUI>().text = text;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        while (stopwatch.Elapsed.TotalSeconds < 1)
        {
            yield return null;
        }
        stopwatch.Stop();
        resultPopup.SetActive(false);
    }
}
