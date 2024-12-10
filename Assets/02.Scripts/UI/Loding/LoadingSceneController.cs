using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    public Slider progressBar;
    public TextMeshProUGUI progressText;

    private void Awake()
    {
        progressBar = GetComponentInChildren<Slider>();
        progressText= GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        string targetSceneName = "Scene02"; 
        AsyncOperation operation = SceneManager.LoadSceneAsync(targetSceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //float progress = operation.progress;
            progressBar.value = progress;
            progressText.text = string.Format("{0:0}%", progress * 100);
            //yield return new WaitForSeconds(0.1f);
            yield return null;
        }
    }
}
