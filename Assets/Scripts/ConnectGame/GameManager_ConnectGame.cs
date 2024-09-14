using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_ConnectGame : MonoBehaviour
{
    public const int chessSize = 100;

    public static GameManager_ConnectGame instance;

    [SerializeField] private List<GameObject> chessPrefabList;
    public Transform chessBoardTransform;
    public Dictionary<int, int> chessInfo { get; private set; } //<chessNumber, chessAmount>
    public List<GameObject> chessList { get; private set; }
    public List<Chess> selectedChess { get; set; }

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
    }

    private void Start()
    {
        chessInfo = new Dictionary<int, int>();
        chessList = new List<GameObject>();
        selectedChess = new List<Chess>();

        GenerateChessBoard();
    }

    public void ClearChessBoard()
    {
        for (int i = 0; i < chessBoardTransform.childCount; i++)
        {
            Destroy(chessBoardTransform.GetChild(i).gameObject);
        }

        chessInfo.Clear();
        chessList.Clear();
        selectedChess.Clear();
    }

    public void GenerateChessBoard()
    {
        ClearChessBoard();

        for (int i = 0; i < 50; i++)
        {
            int chessNumber = (int)Random.Range(0, 10);

            GenerateSpecifiedChess(chessNumber);
            GenerateSpecifiedChess(chessNumber);
        }

        ShuffleChessBoard();

        Invoke("DisableGridLayoutGroup", 0.1f);
    }

    private void ShuffleChessBoard()
    {
        for (int i = 0; i < 100; i++)
        {
            int indexNumber = Random.Range(0, chessBoardTransform.childCount);

            chessList[i].transform.SetSiblingIndex(indexNumber);
        }

        for (int i = 0; i < 100; i++)
        {
            chessList[i].transform.SetSiblingIndex((int)Random.Range(0, 100));
        }
    }

    public void GenerateSpecifiedChess(int _chessNumber)
    {
        GameObject chess = Instantiate(chessPrefabList[_chessNumber], chessBoardTransform);

        if (chessInfo.ContainsKey(_chessNumber))
        {
            chessInfo[_chessNumber]++;
        }
        else
        {
            chessInfo.Add(_chessNumber, 1);
        }

        chessList.Add(chess);
    }

    private void DisableGridLayoutGroup()
    {
        chessBoardTransform.GetComponent<GridLayoutGroup>().enabled = false;
    }

    public void DestroyChess(GameObject _chess)
    {
        StartCoroutine(DestroyChess_Coroutine(_chess));
    }

    private IEnumerator DestroyChess_Coroutine(GameObject _chess)
    {
        yield return new WaitUntil(() =>
        {
            Destroy(_chess);
            return true;
        });

        RefreshChessList();
    }

    public void RefreshChessList()
    {
        chessList.Clear();

        for (int i = 0; i < chessBoardTransform.childCount; i++)
        {
            chessList.Add(chessBoardTransform.GetChild(i).gameObject);
        }
    }

}
