using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    protected static SceneLoader instance;
    public static SceneLoader Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SceneLoader>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }

        private set
        {
            instance = value;
        }
    }

    [SerializeField]
    private CanvasGroup sceneLoaderCanvasGroup;
    [SerializeField]
    private Slider progressBar;

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject backImage;
    private string loadSceneName;

    public static SceneLoader Create()
    {
        var SceneLoaderPrefab = Resources.Load<SceneLoader>("SceneLoader");
        return Instantiate(SceneLoaderPrefab);
    }

    public TextMeshProUGUI text;
    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera camera = Camera.main;
        canvas.worldCamera = camera;

        if (backImage != null)
        {
            backImage.SetActive(true);
        }

        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
        }

        gameObject.SetActive(true);
        SceneManager.sceneLoaded += LoadSceneEnd;
        loadSceneName = sceneName;
        StartCoroutine(Load(sceneName));
    }

    private IEnumerator Load(string sceneName)
    {
        progressBar.value = 0f;
        text.text = "loading...";

        yield return StartCoroutine(Fade(true));
        

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;

            if (op.progress < 0.9f)
            {
                progressBar.value = Mathf.Lerp(progressBar.value, op.progress, timer);

                if (progressBar.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.value = Mathf.Lerp(progressBar.value, 1f, timer);

                if (progressBar.value == 1.0f)
                {
                    op.allowSceneActivation = true;

                    text.text = "Complete!";
                    yield break;
                }
            }
        }
    }

    private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= LoadSceneEnd;
            if (backImage != null)
            {
                backImage.SetActive(false); 
            }
            if (progressBar != null)
            {
                progressBar.gameObject.SetActive(false);
            }
        }
        StartCoroutine(CGetPlayerStatus());
    }

    private IEnumerator CGetPlayerStatus()
    {
        for (int i = 0; i < 2; ++i)
        {
            yield return null;
        }
        GameManager.instance.player.GetComponent<Status>().Restore();
        PlayerDataManager.instance.LoadFile();

    }


    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;

        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 2f;
            sceneLoaderCanvasGroup.alpha = Mathf.Lerp(isFadeIn ? 0 : 1, isFadeIn ? 1 : 0, timer);
        }

        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }
}