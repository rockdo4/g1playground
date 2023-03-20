using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPopEvent : MonoBehaviour
{
    [SerializeField] private List<int> storyIdList = new List<int>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.instance.SetStoryList(storyIdList);
            EventManager.instance.PlayStory();
            gameObject.SetActive(false);
        }
    }
}
