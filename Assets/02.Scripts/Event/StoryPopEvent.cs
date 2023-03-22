using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPopEvent : MonoBehaviour
{
    public enum StoryPopType
    {
        Story,
        Image,
    }
    [SerializeField] private StoryPopType storyType;
    [SerializeField] private List<int> storyIdList = new List<int>();

    //when player enters gives EventManager story Id list and play story
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.instance.SetStoryList(storyIdList);

            switch (storyType)
            {
                case StoryPopType.Story:
                    
                    EventManager.instance.PlayStory(); 
                    break;
                case StoryPopType.Image:
                    break;
            }
            EventManager.instance.Pause();
            Destroy(gameObject);
        }
    }
}
