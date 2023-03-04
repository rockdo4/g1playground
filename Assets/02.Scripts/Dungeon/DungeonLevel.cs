using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDungeonStatusUi(string level)
    {
        // StringBuilder selectedLevel = new StringBuilder();
        StringBuilder text = new StringBuilder("레벨 ");

        DungeonManager.instance.SelectedLevel.Append("Level");
        DungeonManager.instance.SelectedLevel.Append(level);
      
        text.Append(DungeonManager.instance.DungeonTable.Get(DungeonManager.instance.SelectedLevel.ToString()).level);
        GameObject.Find("LevelNum").GetComponent<TextMeshProUGUI>().text = text.ToString();

        text.Clear();
        text.Append("난이도 : ");
        text.Append(DungeonManager.instance.DungeonTable.Get(DungeonManager.instance.SelectedLevel.ToString()).difficulty);
        GameObject.Find("Diff").GetComponent<TextMeshProUGUI>().text = text.ToString();

        GameObject.Find("Status").transform.Find("Reward").gameObject.SetActive(true);
        //todo:change img id from imgTable
        // GameObject.Find("RewardImg").GetComponent<Image>().sprite=
        GameObject.Find("RewardCount").GetComponent<TextMeshProUGUI>().text = DungeonManager.instance.DungeonTable.Get(DungeonManager.instance.SelectedLevel.ToString()).itemcount.ToString();

        GameObject.Find("StartButton").GetComponent<Button>().interactable = true;

    }
}
