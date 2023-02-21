using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonManager : MonoBehaviour
{
    public GameObject[] popButton;
    public PopupUI ui;
    public GameObject[] saveDatas;

    private void Awake()
    {
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIObject(Input.mousePosition))
            {
                ui.gameObject.SetActive(false);
            }
        }
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void OpenCloseUI()
    {
        if (!ui.gameObject.activeSelf)
            ui.gameObject.SetActive(true);
        else
            ui.gameObject.SetActive(false);
    }

    public bool IsPointerOverUIObject(Vector2 touchPos)
    {
        PointerEventData eventDataCurrPos = new PointerEventData(EventSystem.current);
        eventDataCurrPos.position = touchPos;

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventDataCurrPos, results);

        return results.Count > 0;
    }
}
