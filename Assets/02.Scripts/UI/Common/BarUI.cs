using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    public Image image;
    public int curValue;
    public int maxValue;

    protected virtual void Awake()
    {
        image = GetComponent<Image>();

    }

    public void SetImageFillAmount(int value)
    {
        StartCoroutine(StatBar(value));
        image.fillAmount = (float)value / maxValue;

        curValue = value;
    }
    public float duration = 0.5f;
    private IEnumerator StatBar(int value)
    {

        int startValue = curValue;
        int endValue = Mathf.Clamp(value, 0, maxValue);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            int newValue;

            if (value < curValue)
                newValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, t));
            else
                newValue = Mathf.RoundToInt(Mathf.LerpUnclamped(startValue, endValue, t));

            if (curValue == value)
            {
                curValue = newValue;
                StopAllCoroutines();
            }
            yield return null;
        }
    }
}
