using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoryPopEvent : MonoBehaviour
{
    [SerializeField] private List<int> storyIdList = new List<int>();
    private List<int> playedStory = new List<int>();

    private void Awake()
    {
        Load();
    }
    //when player enters gives EventManager story Id list and play story
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            if (playedStory.Contains(storyIdList.First()))
            {
                Destroy(gameObject);
                return;
            }
            playedStory.Add(storyIdList.First());
            Save();
            //EventManager.instance.Pause();
            EventManager.instance.SetStoryList(storyIdList);
            EventManager.instance.Pause();
            EventManager.instance.PlayStory();
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        var saveData = new SavePlayedDataVer1();
        foreach (var played in playedStory)
        {

            saveData.playedId.Add(played);
        }
        SaveLoadSystem.Save(saveData);
    }

    public void Load()
    {
        var saveData = SaveLoadSystem.Load(SaveData.Types.Story) as SavePlayedDataVer1;
        if (saveData == null)
        {
            var newsaveData = new SavePlayedDataVer1();

            SaveLoadSystem.Save(newsaveData);
            saveData = newsaveData;
        }
        playedStory = saveData.playedId;
    }
}
