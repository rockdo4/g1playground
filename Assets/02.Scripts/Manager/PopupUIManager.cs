using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupUIManager : MonoBehaviour
{
    public PopupUI _inventoryPopup;
    public PopupUI _skillPopup;
    public PopupUI _characterStatsPopup;
    public PopupUI _settingPopup;

    public Button _inventoryButton;
    public Button _skillButton;
    public Button _charStatsButton;
    public Button _settingButton;

    [Space]
    public KeyCode _escapeKey = KeyCode.Escape;

    private LinkedList<PopupUI> _activePopupLList;
    private List<PopupUI> _allPopupList;

    private void Awake()
    {
        _activePopupLList = new LinkedList<PopupUI>();
        Init();
        InitCloseAll();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_escapeKey))
        {
            if (_activePopupLList.Count > 0)
            {
                ClosePopup(_activePopupLList.First.Value);
            }
        }
    }

    private void Init()
    {
        _allPopupList = new List<PopupUI>()
            {
                _inventoryPopup, _skillPopup, _characterStatsPopup, _settingPopup
            };

        foreach (var popup in _allPopupList)
        {
            popup.OnFocus += () =>
            {
                _activePopupLList.Remove(popup);
                _activePopupLList.AddFirst(popup);
                RefreshAllPopupDepth();
            };

            popup._closeButton.onClick.AddListener(() => ClosePopup(popup));
        }
    }

    private void InitCloseAll()
    {
        foreach (var popup in _allPopupList)
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
        _activePopupLList.AddFirst(popup);
        popup.gameObject.SetActive(true);
        RefreshAllPopupDepth();
    }

    public void ClosePopup(PopupUI popup)
    {
        _activePopupLList.Remove(popup);
        popup.gameObject.SetActive(false);
        RefreshAllPopupDepth();
    }

    private void RefreshAllPopupDepth()
    {
        foreach (var popup in _activePopupLList)
        {
            popup.transform.SetAsFirstSibling();
        }
    }
}
