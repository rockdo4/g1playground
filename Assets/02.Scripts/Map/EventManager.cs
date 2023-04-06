using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.Rendering;

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
    [SerializeField] private GameObject imageBoard;
    [SerializeField] private Image buttonImage;
    [SerializeField] private TextMeshProUGUI imageText;

    [Header("Text Board")]
    [SerializeField] private GameObject TextBoard;
    [SerializeField] private TextMeshProUGUI textBoardText;

    private List<int> storyList = new List<int>();

    private float originTextDelay;
    private float timer = 0f;

    private int storyCount = 0;
    private int currStoryIndex = 0;
    private int clickCount = 0;
    private bool isClickable = true;
    public bool IsPlayingStory { get; set; }

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
        IsPlayingStory = true;
    }

    ////////////////////////Stage Title///////////////////////////////
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
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        while (stopwatch.Elapsed.TotalSeconds < fadeDelay)
        {
            yield return null;
        }
        stopwatch.Stop();

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
        storyList.Clear();
        storyList = stories;
        storyCount = storyList.Count;
        IsPlayingStory = true;
        currStoryIndex = 0;
        isClickable = true;
    }

    public void PlayStory()
    {
        //when Click and text is not all printed reduce the textDelay
        if (!isClickable)
        {
            textDelay = fastTextDelay;
            return;
        }

        //Get story and display them
        if (currStoryIndex < storyCount) 
        {            
            textDelay = originTextDelay;
            isClickable = false;
            StoryData data = DataTableMgr.GetTable<StoryData>().Get(storyList[currStoryIndex].ToString());
            currStoryIndex++;

            var icon = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(data.iconId).iconName);
            
            switch (int.Parse(data.type))
            {
                case 0:
                    //Story Board
                    imageBoard.SetActive(false);
                    TextBoard.SetActive(false);
                    ShowStoryBoard(icon, data.storyLine);
                    break;
                case 1:
                    //Story only Image
                    storyBoard.SetActive(false);
                    TextBoard.SetActive(false);
                    ShowStoryImage(icon, data.storyLine);
                    break;
                case 2:
                    //Story only Text
                    storyBoard.SetActive(false);
                    imageBoard.SetActive(false);
                    ShowStoryText(data.storyLine);
                    break;
            }       
        }
        else
        {
            //Reset story board
            RestStory();
        }
    }

    IEnumerator TextAnimationEffect(TextMeshProUGUI textBoard, string narration)
    {
        var writerText = "";

        //Text effect
        for (int i = 0; i < narration.Length; i++)
        {
            if (i >= narration.Length - 1)
            {
                isClickable = true;
            }
            writerText += narration[i];
            textBoard.text = writerText;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.Elapsed.TotalSeconds < textDelay)
            {
                yield return null;
            }
            stopwatch.Stop();

        }
    }

    public void RestStory()
    {
        textDelay = originTextDelay;
        currStoryIndex = 0;
        StopCoroutine("TextAnimationEffect");
        StopCoroutine("CoPlayStoryDelay");
        storyText.text = string.Empty;
        imageText.text = string.Empty;
        textBoardText.text = string.Empty;
        storyBoard.SetActive(false);
        imageBoard.SetActive(false);
        TextBoard.SetActive(false);
        IsPlayingStory = false;
        isClickable = true;
        ResetCount();
        Resume();
    }

    //////////////////////Story Board/////////////////////////////////
    public void ShowStoryBoard(Sprite icon, string text)
    {
        storyBoard.SetActive(true);
        storyIcon.sprite = icon;
        text = text.Replace("n", "\n");

        StartCoroutine(TextAnimationEffect(storyText, text));
    }

    public void SkipStory()
    {
        if (storyBoard.activeSelf)
        {
            RestStory();
            //currStoryIndex = 0;
            //Resume();
            //storyBoard.SetActive(false);
        }
    }


    /////////////////Image///////////////////////////
    public void ShowStoryImage(Sprite storySprite, string text)
    {
        imageBoard.SetActive(true);
        buttonImage.sprite = storySprite;
        imageText.text = text.Replace("n", "\n");
        isClickable = true;
    }

    /////////////////Text///////////////////////////
    public void ShowStoryText(string text)
    {
        TextBoard.SetActive(true);
        text = text.Replace("n", "\n");

        StartCoroutine(TextAnimationEffect(textBoardText, text));
    }

    /////////////////Color Effect/////////////////////////////
    public void ChangeColorEffect(string stageName)
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

        StartCoroutine(CoChangeColorDelay(stageName, destroyTime)); 
    }

    public void ChangeColorEffectStory(string stageName)
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

        StartCoroutine(CoChangeColorDelay(stageName, destroyTime));
        StartCoroutine(CoPlayStoryDelay(stageName, destroyTime));
    }

    IEnumerator CoChangeColorDelay(string stageName, float delay)
    {

        while (timer <= delay - 1f) 
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;

        TileColorManager.instance.ChangeTileMaterial(stageName, true);
    }

    IEnumerator CoPlayStoryDelay(string stageName, float delay)
    {
        //Stopwatch stopwatch = new Stopwatch();
        //stopwatch.Start();
        //while (stopwatch.Elapsed.TotalSeconds < delay)
        //{
        //    yield return null;
        //}
        //stopwatch.Stop();
        while (timer <= delay - 0.6f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        //TileColorManager.instance.ChangeTileMaterial(stageName, true);

        Pause();
        PlayStory();
    }

    public void ResetCount()
    {
        effectCount = 0;
    }


    //////////////////////////////////////////////////////////////
    public void Pause()
    {
        Time.timeScale = 0f;       
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }
}
