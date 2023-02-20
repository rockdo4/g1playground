using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupUIManager : MonoBehaviour
{
    public PopupUI inventoryPopup;
    public PopupUI skillPopup;
    public PopupUI characterStatsPopup;
    public PopupUI settingPopup;

    public Button inventoryButton;
    public Button skillButton;
    public Button charStatsButton;
    public Button settingButton;

    [Space]
    public KeyCode escapeKey = KeyCode.Escape;

    private LinkedList<PopupUI> activePopupLList;
    private List<PopupUI> allPopupList;

    private void Awake()
    {
        activePopupLList = new LinkedList<PopupUI>();
        Init();
        InitCloseAll();
    }

    private void Update()
    {
        if (Input.GetKeyDown(escapeKey))
        {
            if (activePopupLList.Count > 0)
            {
                ClosePopup(activePopupLList.First.Value);
            }
        }
    }

    private void Init()
    {
        allPopupList = new List<PopupUI>()
            {
                inventoryPopup, skillPopup, characterStatsPopup, settingPopup
            };

        foreach (var popup in allPopupList)
        {
            popup.OnFocus += () =>
            {
                activePopupLList.Remove(popup);
                activePopupLList.AddFirst(popup);
                RefreshAllPopupDepth();
            };

            popup.closeButton.onClick.AddListener(() => ClosePopup(popup));
        }
    }

    private void InitCloseAll()
    {
        foreach (var popup in allPopupList)
        {
            ClosePopup(popup);
        }
    }

    private void ToggleKeyDownAction(in KeyCode key, PopupUI popup)
    {
        if (Input.GetKeyDown(key))
        {
            ToggleOpenClosePopup(popup);
        }
    }

    private void ToggleOpenClosePopup(PopupUI popup)
    {
        if (!popup.gameObject.activeSelf)
            OpenPopup(popup);
        else
            ClosePopup(popup);
    }

    public void OpenPopup(PopupUI popup)
    {
        activePopupLList.AddFirst(popup);
        popup.gameObject.SetActive(true);
        RefreshAllPopupDepth();
    }

    public void ClosePopup(PopupUI popup)
    {
        activePopupLList.Remove(popup);
        popup.gameObject.SetActive(false);
        RefreshAllPopupDepth();
    }

    private void RefreshAllPopupDepth()
    {
        foreach (var popup in activePopupLList)
        {
            popup.transform.SetAsFirstSibling();
        }
    }
}
