using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TtileScreen : MonoBehaviour
{
    public void LoadLevel(string _levelName)
    {
        SceneManager.LoadScene(_levelName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void WechatLoginAuthorization()
    {
        AndroidJavaClass javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject javaObject = javaClass.GetStatic<AndroidJavaObject>("currentActivity");

        javaObject.Call("WechatLoginAuthorize");
    }

}
