using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPopEvent : MonoBehaviour
{
    [SerializeField] private List<int> storyIdList = new List<int>();
    
    //when player enters gives EventManager story Id list and play story
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //EventManager.instance.Pause();
            EventManager.instance.SetStoryList(storyIdList);
            EventManager.instance.Pause();
            EventManager.instance.PlayStory();

            Destroy(gameObject);
        }
    }
}
