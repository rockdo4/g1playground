using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupCheckConfirmEventArgs : EventArgs
{
    public UnityEngine.Events.UnityAction events;
    public string eventName;
}

public class LoadMessageBox : MonoBehaviour
{
    public static LoadMessageBox instance;
    public static event EventHandler<PopupCheckConfirmEventArgs> ConfirmEvent;

    public enum MsgType { Notice, Error, Warning, Exit };

    public GameObject popupView;
    
    public TextMeshProUGUI msg;
    public Button yesBt;
    public Button noBt;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;

        ConfirmEvent += OnConfirmOnClick;

        if (transform.parent != null && transform.parent.GetComponent<Canvas>() != null)
            transform.SetSiblingIndex(transform.parent.childCount);
        else
            Debug.LogError("error");

        void action()
        {
            Application.Quit();
        }
        instance.PopupCheckWindowOpen(action, "Exit", MsgType.Exit, "종료 하시겠습니까?");
    }

    public void PopupCheckWindowOpen(UnityEngine.Events.UnityAction action, string actionName, MsgType msgtype, string msg)
    {
        yesBt.gameObject.SetActive(true);
        noBt.gameObject.SetActive(true);
        yesBt.onClick.AddListener(delegate { ConfirmEvent(this, new PopupCheckConfirmEventArgs() { events = action, eventName = actionName }); });
        PopupWindowOpen(msgtype, msg);
    }

    public void OnConfirmOnClick(object sender, PopupCheckConfirmEventArgs e)
    {
        e.events();
        yesBt.onClick.RemoveAllListeners();
    }


    public void PopupWindowOpen(MsgType msgtype, string msgText)
    {
        popupView.SetActive(true);

        if (msgtype == MsgType.Error)
        {
            msg.text = msgText;
        }
        else if (msgtype == MsgType.Warning)
        {
            msg.text = msgText;
        }
        else if (msgtype == MsgType.Exit)
        {
            msg.text = msgText;
        }
        else
        {
            msg.text = msgText;
        }
    }
}
