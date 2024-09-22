using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public TextMeshProUGUI messageInfo;

    public void ReceiveAuthroizeMsg(string _msg)
    {
        messageInfo.text = _msg;
    }

    public void ReceiveAuthorizeErrorMsg(string _msg)
    {
        messageInfo.text = "Error: " + _msg;
    }

    public void ReceiveShareCallBack(string _msg)
    {

    }
}
