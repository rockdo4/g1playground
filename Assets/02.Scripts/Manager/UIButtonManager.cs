using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonManager : MonoBehaviour
{
    public GameObject[] buttons;
    public GameObject[] popUps;

    private void Awake()
    {
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckArea();
        }
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void PopUp(GameObject popup)
    {
        if (!popup.gameObject.activeSelf)
            popup.gameObject.SetActive(true);
        else
            popup.gameObject.SetActive(false);
    }

    public void AllClosePopUp()
    {
        foreach (var popup in popUps)
            popup.gameObject.SetActive(false);
    }

    public void CheckArea()
    {
        foreach (var button in buttons)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(button.GetComponent<RectTransform>(), Input.mousePosition, null))
            {
                return;
            }
        }
        foreach (var popup in popUps)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(popup.GetComponent<RectTransform>(), Input.mousePosition, null))
            {
                if(popup.gameObject.activeSelf)
                    return;
            }
        }
        AllClosePopUp();
    }
}
