using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gambling : MonoBehaviour
{
    public GameObject oneReward;
    public GameObject tenReward;

    public void TryTen()
    {
        GetEquipments(10);
        tenReward.SetActive(true);
    }

    public List<GameObject> GetEquipments(int count)
    {
        List<GameObject> equipmentList = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject newEquipment = RandomEquipments();
            equipmentList.Add(newEquipment);
        }

        return equipmentList;
    }

    public GameObject RandomEquipments()
    {
        GameObject newEquipment = new GameObject();
        newEquipment.name = "Equipment" + Random.Range(0, 8);

        return newEquipment;
    }

    public List<GameObject> GetSkills(int count)
    {
        List<GameObject> skillList = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject newSkills = RandomSkills();
            skillList.Add(newSkills);
        }

        return skillList;
    }

    public GameObject RandomSkills()
    {
        GameObject newSkill = new GameObject();
        newSkill.name = "Skill" + Random.Range(0, 5);

        return newSkill;
    }
}
