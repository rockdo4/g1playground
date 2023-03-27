using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    public GameObject startPopUp;
    public GameObject crewPopUp;
    public GameObject exitPopUp;
    public GameObject button;
    private UIManager uiManager;

    private void Start()
    {
        uiManager = GetComponent<UIManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(!crewPopUp.activeSelf)
            {
                if (startPopUp.activeSelf)
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(button.GetComponent<RectTransform>(), Input.mousePosition, null))
                    {
                        return;
                    }
                    SceneManager.LoadScene(1);
                    startPopUp.SetActive(false);
                }
                else
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(button.GetComponent<RectTransform>(), Input.mousePosition, null))
                    {
                        return;
                    }
                    startPopUp.SetActive(true);
                }
            }
        }
    }
}
