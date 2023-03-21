using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventManager : MonoBehaviour
{
    [Header("StageTitle")]
    [SerializeField] private GameObject stageTitlePanel;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private float titlePanelDelay = 3f;

    [Header("Story")]
    [SerializeField] private GameObject storyBoard;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private Image storyIcon;

    private List<int> storyList = new List<int>();
    private int storyCount = 0;
    private int currStoryIndex = 0;

    private static EventManager m_instance;
    public static EventManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<EventManager>();
            }

            return m_instance;
        }
    }

    private void Awake()
    {
        //singleton
        if (instance != this)
        {

            Destroy(gameObject);
        }
    }

    public void ShowStageTitile(string title)
    {
        stageTitlePanel.SetActive(true);
        textMesh.text = title;
        StartCoroutine(CoSetStageTitleFalse());
    }

    private IEnumerator CoSetStageTitleFalse()
    {
        //wait for delay after falling then set active to false
        yield return new WaitForSeconds(titlePanelDelay);
        stageTitlePanel.SetActive(false);
        StopAllCoroutines();
    }

    /////////////////////////Story//////////////////////////////////
    public void SetStoryList(List<int> stories)
    {
        storyList = stories;
        storyCount = storyList.Count;        
    }

    public void PlayStory()
    {
        if (currStoryIndex < storyCount) 
        {
            StoryData data = null;
            data = DataTableMgr.GetTable<StoryData>().Get(storyList[currStoryIndex].ToString());
            currStoryIndex++;

            ShowStoryBoard(data.iconSprite, data.storyScript);
        }
        else
        {
            currStoryIndex = 0;
            Resume();
            storyBoard.SetActive(false);
        }
    }

    public void ShowStoryBoard(Sprite icon, string text)
    {
        storyBoard.SetActive(true);
        storyIcon.sprite = icon;
        storyText.text = text;
    }

    public void SkipStory()
    {
        if (storyBoard.activeSelf)
        {
            currStoryIndex = 0;
            Resume();
            storyBoard.SetActive(false);
        }
    }
    ///////////////////////////////////////////////////////

    public void Pause()
    {
        Time.timeScale = 0f;       
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }
}
