using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform popup;
    [SerializeField] private float animationTime = 0.3f;
    [SerializeField] private Vector3 initialScale = new Vector3(0.1f, 0.1f, 0.1f);

    private void Awake()
    {
        popup = gameObject.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        StartCoroutine(ScalePopupCoroutine(initialScale, Vector3.one, animationTime, true));
    }

    private void OnDisable()
    {
        if(gameObject.activeSelf)
            StartCoroutine(ScalePopupCoroutine(Vector3.one, initialScale, animationTime, false));
    }

    public void MenuPopUpExit()
    {
        StartCoroutine(ScalePopupCoroutine(Vector3.one, initialScale, animationTime, false));
    }

    private IEnumerator ScalePopupCoroutine(Vector3 startScale, Vector3 endScale, float duration, bool setActive)
    {
        if (setActive)
        {
            popup.gameObject.SetActive(true);
        }

        popup.localScale = startScale;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            popup.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        popup.localScale = endScale;

        if (!setActive)
        {
            popup.gameObject.SetActive(false);
        }
    }
}