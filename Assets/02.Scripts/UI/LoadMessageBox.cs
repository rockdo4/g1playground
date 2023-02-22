using System;
using System.Collections;
using System.Collections.Generic;
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

    public enum MsgType { notice, error, warning, exit };

    public GameObject popupView;
    
    public Text m_MessageType;
    public TextMeshProUGUI m_Msg;
    public Button Btn_Exit;
    public Button Btn_OK;
    public Button Btn_Cancle;

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
        instance.PopupCheckWindowOpen(action, "Exit", MsgType.exit, "종료 하시겠습니까?");
    }

    public void PopupCheckWindowOpen(UnityEngine.Events.UnityAction action, string actionName, MsgType msgtype, string msg)
    {
        Btn_OK.gameObject.SetActive(true);
        Btn_Cancle.gameObject.SetActive(true);
        Btn_OK.onClick.AddListener(delegate { ConfirmEvent(this, new PopupCheckConfirmEventArgs() { events = action, eventName = actionName }); });
        PopupWindowOpen(msgtype, msg);
    }

    public void OnConfirmOnClick(object sender, PopupCheckConfirmEventArgs e)
    {
        e.events();
        Btn_OK.onClick.RemoveAllListeners();
    }


    public void PopupWindowOpen(MsgType msgtype, string msg)
    {
        popupView.SetActive(true);

        if (msgtype == MsgType.error)
        {
            m_MessageType.text = "에러";
            m_Msg.text = msg;
            m_MessageType.color = Color.red;
        }
        else if (msgtype == MsgType.warning)
        {
            m_MessageType.text = "알림";
            m_Msg.text = msg;
            m_MessageType.color = Color.yellow;
        }
        else if (msgtype == MsgType.exit)
        {
            m_MessageType.text = "나가기";
            m_Msg.text = msg;
            m_MessageType.color = Color.yellow;
        }
        else
        {
            m_MessageType.text = "Notice";
            m_Msg.text = msg;
            m_MessageType.color = Color.white;
        }
    }
}
