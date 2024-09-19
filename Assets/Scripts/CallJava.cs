using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngineInternal;

public class CallJava : MonoBehaviour
{
    AndroidJavaClass javaClass = null;
    AndroidJavaObject javaObject = null;

    [SerializeField] private TMP_InputField logInputField;
    [SerializeField] private TMP_InputField logPrint;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField namePrint;


    void Start()
    {
        javaClass = new AndroidJavaClass("com.example.unitytest.Test");
        javaObject = new AndroidJavaObject("com.example.unitytest.Test");
    }

    public void SetLOG()
    {
        javaClass.CallStatic("SetLOG", logInputField.text);
    }

    public void SetLOGField()
    {
        javaClass.SetStatic("LOG", logInputField.text);
    }

    public void GetLOG()
    {
        string log = javaClass.CallStatic<string>("GetLOG");
        logPrint.text = log;
    }

    public void GetLOGField()
    {
        string log = javaClass.GetStatic<string>("LOG");
        logPrint.text = log;
    }

    public void SetName()
    {
        javaObject.Call("SetName", nameInputField.text);
    }

    public void SetNameField()
    {
        javaObject.Set("name", nameInputField.text);
    }

    public void GetName()
    {
        string name = javaObject.Call<string>("GetName");
        namePrint.text = name;
    }

    public void GetNameField()
    {
        string name = javaObject.Get<string>("name");
        namePrint.text = name;
    }
}
