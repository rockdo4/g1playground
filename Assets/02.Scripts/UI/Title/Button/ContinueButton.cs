using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;
using UnityEngine;

public class ContinueButton : ButtonUi
{
    public TextMeshProUGUI text;
    public float fadeInDuration = 0.2f;
    public float fadeOutDuration = 0.5f;

    private void Awake()
    {
        
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void ClickButton()
    {
        SceneLoader.Instance.LoadScene("Scene02");
    }

    private void OnEnable()
    {
        StartCoroutine(BlinkText());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private IEnumerator BlinkText()
    {
        while (true)
        {
            Color textColor = text.color;
            textColor.a = 1f;
            text.color = textColor;

            yield return new WaitForSeconds(fadeInDuration);

            float startTime = Time.time;
            float elapsedTime = 0f;

            while (elapsedTime < fadeOutDuration)
            {
                elapsedTime = Time.time - startTime;
                textColor = text.color;
                textColor.a = Mathf.Lerp(1, 0, elapsedTime / fadeOutDuration);
                text.color = textColor;

                yield return null;
            }

            textColor = text.color;
            textColor.a = 0f;
            text.color = textColor;

            yield return new WaitForSeconds(0.1f);
        }
    }

}
