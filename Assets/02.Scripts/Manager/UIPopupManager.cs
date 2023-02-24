using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupManager : MonoBehaviour
{
    public PopupUI optionPopup;
    public PopupUI skillPopup;
    public PopupUI equipInventoryPopup;
    public PopupUI useInventoryPopup;
    public PopupUI magicBookPopup;
    public PopupUI monsterBookPopup;
    public PopupUI settingPopup;
    public PopupUI messageBoxPopup;

    [Space]

    private LinkedList<PopupUI> activePopupLList;
    private List<PopupUI> allPopupList;

    private void Awake()
    {
        //activePopupLList = new LinkedList<PopupUI>();
        //Init();
        //InitCloseAll();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (activePopupLList.Count > 0)
        //    {
        //        ClosePopup(activePopupLList.First.Value);
        //    }
        //}
    }

    private void Init()
    {
        //allPopupList = new List<PopupUI>()
        //    {
        //    optionPopup, skillPopup, equipInventoryPopup, useInventoryPopup, magicBookPopup, monsterBookPopup, settingPopup, messageBoxPopup,
        //    };

        //foreach (var popup in allPopupList)
        //{
        //    popup.OnFocus += () =>
        //    {
        //        activePopupLList.Remove(popup);
        //        activePopupLList.AddFirst(popup);
        //        RefreshAllPopupDepth();
        //    };

        //    popup.closeButton.onClick.AddListener(() => ClosePopup(popup));
        //}
    }

    private void InitCloseAll()
    {
        //foreach (var popup in allPopupList)
        //{
        //    ClosePopup(popup);
        //}
    }

    private void ToggleKeyDownAction(in KeyCode key, PopupUI popup)
    {
        //if (Input.GetKeyDown(key))
        //{
        //    ToggleOpenClosePopup(popup);
        //}
    }

    private void ToggleOpenClosePopup(PopupUI popup)
    {
        //if (!popup.gameObject.activeSelf)
        //    OpenPopup(popup);
        //else
        //    ClosePopup(popup);
    }

    public void OpenPopup(PopupUI popup)
    {
        //activePopupLList.AddFirst(popup);
        //popup.gameObject.SetActive(true);
        //RefreshAllPopupDepth();
    }

    public void ClosePopup(PopupUI popup)
    {
        //activePopupLList.Remove(popup);
        //popup.gameObject.SetActive(false);
        //RefreshAllPopupDepth();
    }

    private void RefreshAllPopupDepth()
    {
        //foreach (var popup in activePopupLList)
        //{
        //    popup.transform.SetAsFirstSibling();
        //}
    }
}
