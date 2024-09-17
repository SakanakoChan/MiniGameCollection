using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_SokobanGame : GameManager
{
    public static GameManager_SokobanGame instance;
    public int completedBoxAmount { get; private set; }
    private int totalBoxAmount;
    public Transform boxList;

    private System.Action OnCompletedBoxAmountIncrease;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        completedBoxAmount = 0;
        totalBoxAmount = boxList.childCount;
    }

    private void Start()
    {
        OnCompletedBoxAmountIncrease += () =>
        {
            if (completedBoxAmount == totalBoxAmount)
            {
                Debug.Log("Game passed!");
            }
        };
    }

    public void IncreaseCompletedBoxAmount()
    {
        completedBoxAmount++;

        if(OnCompletedBoxAmountIncrease != null)
        {
            OnCompletedBoxAmountIncrease();
        }
    }

    public void DecreaseCompletedBoxAmount()
    {
        completedBoxAmount--;
    }

    public void ResetLevel()
    {
        StartCoroutine(ResetLevel_Coroutine());
    }

    private IEnumerator ResetLevel_Coroutine()
    {
        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
