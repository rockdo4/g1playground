using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonManager : MonoBehaviour
{
    public static bool gamePaused = false;

    public GameObject[] buttons;
    public GameObject[] popUps;
    public GameObject exitMessage;

    private void Awake()
    {
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckArea();
        }


        if (GameManager.instance.uiManager.popupStack.Count > 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.uiManager.popupStack.Clear();
            AllClosePopUp();
        }

        else if (GameManager.instance.uiManager.popupStack.Count == 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            exitMessage.SetActive(true);
        }
    }

    public void OnApplicationQuit()
    {
        
    }

    public void PopUp(GameObject popup)
    {
        if (!popup.gameObject.activeSelf)
        {
            popup.gameObject.SetActive(true);
            GameManager.instance.uiManager.AddPopUp(popup);
            if (popup.gameObject.name != "Option")
            {
                var option = GameObject.Find("Option");
                if (option != null)
                {
                    if (option.activeSelf)
                    {
                        option.SetActive(false);
                        Time.timeScale = 0f;
                    }
                }  
            }
        }
        else
        {
            popup.gameObject.SetActive(false);
            GameManager.instance.uiManager.RemovePopUp();
            Time.timeScale = 1f;
        }
    }

    public void AllClosePopUp()
    {
        foreach (var popup in popUps)
        {
            popup.gameObject.SetActive(false);
        }
        if (DungeonManager.instance != null)
            DungeonManager.instance.uiDungeonSelect.SetActive(false);
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
                if (popup.gameObject.activeSelf)
                {
                    return;
                }
            }
        }
        if (DungeonManager.instance != null)
        {
            if (DungeonManager.instance.uiDungeonSelect.activeSelf)
            {
                return;
            }
        }
        
        AllClosePopUp();
    }
}
