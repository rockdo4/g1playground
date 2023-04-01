using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitlePanel : PanelUi, IPointerDownHandler
{
    public CrewButton crewButton;

    public TextMeshProUGUI text;
    public float fadeInDuration = 0.2f;
    public float fadeOutDuration = 0.5f;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsPointerOverUIObject(eventData))
        {
            SceneLoader.Instance.LoadScene("Tutorial");
        }
    }


    private void Awake()
    {
        crewButton = GetComponentInChildren<CrewButton>(true);
        //text = GetComponentInChildren<TextMeshProUGUI>();
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
    private bool IsPointerOverUIObject(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // 첫 번째 요소는 배경 이미지 자체이므로, 결과가 두 개 이상 있어야 버튼 위에 있는 것으로 간주합니다.
        return results.Count > 1;
    }
}
