using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        //SceneLoader.Instance.LoadScene("Scene02");

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
            // 알파값을 즉시 올립니다.
            Color textColor = text.color;
            textColor.a = 1f;
            text.color = textColor;

            yield return new WaitForSeconds(fadeInDuration);

            // 알파값을 천천히 떨어뜨립니다.
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

            // 글자를 완전히 숨깁니다 (알파값을 0으로 설정).
            textColor = text.color;
            textColor.a = 0f;
            text.color = textColor;

            yield return new WaitForSeconds(0.1f);
        }
    }

}
