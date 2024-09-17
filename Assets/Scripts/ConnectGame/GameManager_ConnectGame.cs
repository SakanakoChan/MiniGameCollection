using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager_ConnectGame : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    public int chessAmount = 80;

    public int chessSize { get; private set; } = 100;

    public static GameManager_ConnectGame instance;

    [SerializeField] private List<GameObject> chessPrefabList;
    public Transform chessBoardTransform;
    public float chessBoardLeftBound {  get; private set; }
    public float chessBoardRightBound { get; private set; }
    public float chessBoardUpBound {  get; private set; }
    public float chessBoardDownBound { get; private set; }

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

        Invoke("GetChessBoardSizeInfo", 0.2f);
    }

    private void GetChessBoardSizeInfo()
    {
        chessBoardLeftBound = chessList[0].transform.position.x;
        chessBoardRightBound = chessList[0].transform.position.x;
        chessBoardUpBound = chessList[0].transform.position.y;
        chessBoardDownBound = chessList[0].transform.position.y;

        foreach (var chess in chessList)
        {
            if (chess.transform.position.x < chessBoardLeftBound)
            {
                chessBoardLeftBound = chess.transform.position.x;
            }

            if (chess.transform.position.x > chessBoardRightBound)
            {
                chessBoardRightBound = chess.transform.position.x;
            }

            if (chess.transform.position.y > chessBoardUpBound)
            {
                chessBoardUpBound = chess.transform.position.y;
            }

            if (chess.transform.position.y < chessBoardDownBound)
            {
                chessBoardDownBound = chess.transform.position.y;
            }
        }
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

        for (int i = 0; i < chessAmount / 2; i++)
        {
            int chessNumber = (int)Random.Range(0, 10);

            GenerateSpecifiedChess(chessNumber);
            GenerateSpecifiedChess(chessNumber);
        }

        ShuffleChessBoard();
        ShuffleChessBoard();

        Invoke("DisableGridLayoutGroup", 0.1f);
    }

    private void ShuffleChessBoard()
    {
        for (int i = 0; i < chessBoardTransform.childCount; i++)
        {
            int indexNumber = Random.Range(0, chessBoardTransform.childCount);

            chessList[i].transform.SetSiblingIndex(indexNumber);
        }

        for (int i = 0; i < chessBoardTransform.childCount; i++)
        {
            chessList[i].transform.SetSiblingIndex((int)Random.Range(0, chessBoardTransform.childCount));
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

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (chessAmount > 80)
        {
            throw new System.Exception("Chess Amount should be no more than 80!");
        }

        if (chessAmount % 2 != 0)
        {
            throw new System.Exception("Chess Amount should be even!");
        }
    }
#endif
}
