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
}
