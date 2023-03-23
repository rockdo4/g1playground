using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;

public class EventManager : MonoBehaviour
{
    [Header("StageTitle")]
    [SerializeField] private CanvasGroup stageTitlePanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private float fadeAmount = 0.01f;
    [SerializeField] private float fadeDelay = 1f;

    [Header("Story")]
    [SerializeField] private GameObject storyBoard;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private Image storyIcon;
    [SerializeField] private float textDelay = 0.1f;
    [SerializeField] private float fastTextDelay = 0.0f;

    [Header("Image Board")]
    [SerializeField] private Button imageBoard;

    private Color originTitleColor;
    private Color originTextColor;

    private float timer = 0f;

    private List<int> storyList = new List<int>();
    private float originTextDelay;

    private int storyCount = 0;
    private int currStoryIndex = 0;
    private int clickCount = 0;
    private bool isClickable = true;

    private int effectCount = 0;

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
        originTextDelay = textDelay;
        originTitleColor = stageTitlePanel.GetComponent<Image>().color;
        originTextColor = titleText.color;
    }

    public void ShowStageTitile(string title)
    {
        stageTitlePanel.gameObject.SetActive(true);
        stageTitlePanel.alpha = 1f;
        titleText.text = title.Replace("n", "\n");
        StartCoroutine(CoSetStageTitleFalse());
    }

    private IEnumerator CoSetStageTitleFalse()
    {
        //wait for second
        yield return new WaitForSeconds(fadeDelay);

        //decrease alpha by every fixedupdate
        while (stageTitlePanel.alpha > 0f)
        {
            stageTitlePanel.alpha -= fadeAmount;
            yield return null;
        }

        stageTitlePanel.gameObject.SetActive(false);
        StopCoroutine(CoSetStageTitleFalse());

    }

    /////////////////////////Story//////////////////////////////////
    public void SetStoryList(List<int> stories)
    {
        storyList = stories;
        storyCount = storyList.Count;        
    }

    public void GetNextText()
    {
        //isButtonClick = true;
        if (clickCount > 0)
        {
            PlayStory();
        }
        clickCount++;

    }

    public void PlayStory()
    {
        if (!isClickable)
        {
            textDelay = fastTextDelay;
            return;
        }
        if (currStoryIndex < storyCount) 
        {
            textDelay = originTextDelay;
            isClickable = false;
            StoryData data = null;
            data = DataTableMgr.GetTable<StoryData>().Get(storyList[currStoryIndex].ToString());
            currStoryIndex++;

            var icon = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(data.iconId).iconName);
            ShowStoryBoard(icon, data.storyLine);
        }
        else
        {
            textDelay = originTextDelay;
            currStoryIndex = 0;
            Resume();
            StopCoroutine("TextAnimationEffect");
            storyBoard.SetActive(false);
        }
    }

    public void ShowStoryBoard(Sprite icon, string text)
    {
        storyBoard.SetActive(true);
        storyIcon.sprite = icon;
        text = text.Replace("n", "\n");

        StartCoroutine(TextAnimationEffect(text));


    }

    IEnumerator TextAnimationEffect(string narration)
    {
        var writerText = "";

        //Text effect
        //Debug.Log(narration.Length);
        for (int i = 0; i < narration.Length; i++)
        {
            if (i >= narration.Length - 1) 
            {                
                isClickable = true;
            }
            //Debug.Log(i);
            writerText += narration[i];
            storyText.text = writerText;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.Elapsed.TotalSeconds < textDelay)
            {
                yield return null;
            }
            stopwatch.Stop();
            
        }

        ////Wait untill button click
        //while (true)
        //{
        //    if (isButtonClick)
        //    {
        //        isButtonClick = false;
        //        break;
        //    }
        //    yield return null;
        //}
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

    public void ShowStoryImage(Sprite storySprite)
    {
        imageBoard.image.sprite = storySprite;
    }
    ///////////////////////////////////////////////////////

    //Test Effect
    public void PlayEffect()
    {
        if (effectCount > 0)
        {
            return;
        }
        effectCount++;
        var effect = GameManager.instance.effectManager.GetEffect("Poof_electric");
        effect.transform.position = GameManager.instance.player.transform.position;
        float destroyTime = effect.GetComponent<ParticleSystem>().main.duration;
        GameManager.instance.effectManager.ReturnEffectOnTime("Poof_electric", effect, destroyTime);
    }
    
    public void ResetCount()
    {
        effectCount = 0;
    }

    public void Pause()
    {
        Time.timeScale = 0f;       
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }
}
