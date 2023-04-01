using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    private Image image;
    private float curValue;
    public int maxValue;
    private float duration = 0.5f;

    protected virtual void Awake()
    {
        SetImage();
    }

    private void SetImage()
    {
        if (image == null)
            image = GetComponent<Image>();
    }

    public void SetImageFillAmount(int value)
    {
        SetImage();
        StopAllCoroutines();
        targetValue = (float)value / maxValue;
        StartCoroutine(UpdateBar());
    }

    private float targetValue;
    private IEnumerator UpdateBar()
    {
        float startTime = Time.realtimeSinceStartup;
        float elapsedTime = 0f;
        float initialCurValue = curValue;

        while (elapsedTime < duration)
        {
            elapsedTime = Time.realtimeSinceStartup - startTime;
            curValue = Mathf.Lerp(initialCurValue, targetValue, elapsedTime / duration);
            image.fillAmount = curValue;

            yield return new WaitForSecondsRealtime(0.01f);

            curValue = targetValue;
            image.fillAmount = curValue;
        }
    }
}
