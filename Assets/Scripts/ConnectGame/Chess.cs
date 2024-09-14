using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Chess : MonoBehaviour
{
    private List<Chess> selectedChess;

    private Image image;
    private Button button;

    private TextMeshProUGUI chessNumber;

    private bool isSelected = false;

    [SerializeField] private GameObject linePrefab;
    private List<Vector2> turningPointList;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        chessNumber = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        selectedChess = GameManager_ConnectGame.instance.selectedChess;
        turningPointList = new List<Vector2>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(SelectChess);

    }

    private void SelectChess()
    {
        //cannot select the chess itself and remove it from the chessboard lmao
        if (isSelected)
        {
            image.color = Color.white;
            isSelected = false;
            selectedChess.Clear();
            return;
        }

        if (selectedChess.Count < 1)
        {
            selectedChess.Add(this);
            image.color = Color.gray;
            isSelected = true;
        }
        else
        {
            if (chessNumber.text.Equals(selectedChess[0].chessNumber.text))
            {
                //check if is seccessful connection
                //destroy the two chesses

                if (CanConnect(selectedChess[0].transform.position))
                {
                    //DrawLine(lineVetexList.ToArray());
                    List<Vector2> vetexList = new List<Vector2>();
                    vetexList.Add(transform.position);
                    foreach (var vec in turningPointList)
                    {
                        vetexList.Add(vec);
                    }
                    vetexList.Add(selectedChess[0].transform.position);

                    DrawLine(vetexList.ToArray());
                    GameManager_ConnectGame.instance.DestroyChess(gameObject);
                    GameManager_ConnectGame.instance.DestroyChess(selectedChess[0].gameObject);
                }
            }

            foreach (var chess in selectedChess)
            {
                chess.image.color = Color.white;
            }
            image.color = Color.white;

            isSelected = false;
            if (selectedChess[0] != null)
            {
                selectedChess[0].GetComponent<Chess>().isSelected = false;
            }

            selectedChess.Clear();
        }
    }

    private bool CanConnect(Vector2 _targetChess)
    {
        if (CanConnectWith0Turining(transform.position, _targetChess))
        {
            return true;
        }
        else if (CanConnectWith1Turining(transform.position, _targetChess))
        {
            return true;
        }
        else if (CanConnectWith2Turning(transform.position, _targetChess))
        {
            return true;
        }

        return false;
    }

    private bool CanConnectWith0Turining(Vector2 _originalChess, Vector2 _targetChess)
    {
        float currentX = _originalChess.x;
        float currentY = _originalChess.y;
        float targetX = _targetChess.x;
        float targetY = _targetChess.y;

        //if these two chesses are next to each other
        if (Vector2.Distance(_originalChess, _targetChess) == GameManager_ConnectGame.chessSize)
        {
            return true;
        }
        else
        {
            //if these two chesses are in the same column
            if (_originalChess.x == _targetChess.x)
            {
                //check if there are chesses between these two
                int count = 0;

                foreach (var chess in GameManager_ConnectGame.instance.chessList)
                {
                    if (chess.transform.position.x == _originalChess.x && chess.transform.position.y > Mathf.Min(currentY, targetY) && chess.transform.position.y < Mathf.Max(currentY, targetY))
                    {
                        count++;
                        break;
                    }
                }

                if (count > 0)
                {
                    return false;
                }
                else
                {
                    //Vector2[] vetexList = new Vector2[2] { _originalChess, _targetChess };
                    //DrawLine(vetexList);

                    return true;
                }
            }
            //if these two chesses are in the same row
            else if (_originalChess.y == _targetChess.y)
            {
                int count = 0;

                foreach (var chess in GameManager_ConnectGame.instance.chessList)
                {
                    if (chess.transform.position.y == _originalChess.y && chess.transform.position.x > Mathf.Min(currentX, targetX) && chess.transform.position.x < Mathf.Max(currentX, targetX))
                    {
                        count++;
                        break;
                    }
                }

                if (count > 0)
                {
                    return false;
                }
                else
                {
                    //Vector2[] vetexList = new Vector2[2] { _originalChess, _targetChess };
                    //DrawLine(vetexList);

                    return true;
                }
            }

            //if these two chesses are neither in the same column nor in the same row
            return false;

        }
    }

    private bool CanConnectWith1Turining(Vector2 _originalChess, Vector2 _targetChess)
    {
        Vector2 turningPoint1 = new Vector2(_originalChess.x, _targetChess.y);
        Vector2 turningPoint2 = new Vector2(_targetChess.x, _originalChess.y);

        bool noChessAtTurningPoint1 = NoChessAtPosition(turningPoint1);
        bool noChessAtTurningPoint2 = NoChessAtPosition(turningPoint2);

        if (noChessAtTurningPoint1 && CanConnectWith0Turining(_originalChess, turningPoint1) && CanConnectWith0Turining(turningPoint1, _targetChess))
        {
            //Vector2[] vetexList = new Vector2[3] { _originalChess, turningPoint1, _targetChess };
            //DrawLine(vetexList);
            turningPointList.Clear();
            turningPointList.Add(turningPoint1);

            return true;
        }
        else if (noChessAtTurningPoint2 && CanConnectWith0Turining(_originalChess, turningPoint2) && CanConnectWith0Turining(turningPoint2, _targetChess))
        {
            //Vector2[] vetexList = new Vector2[3] { _originalChess, turningPoint2, _targetChess };
            //DrawLine(vetexList);

            turningPointList.Clear();
            turningPointList.Add(turningPoint2);

            return true;
        }

        return false;
    }

    private bool CanConnectWith2Turning(Vector2 _originalChess, Vector2 _targetChess)
    {
        RectTransform chessBoardTransform = GameManager_ConnectGame.instance.chessBoardTransform as RectTransform;
        float chessBoardWidth = chessBoardTransform.rect.width;
        float chessBoardHeight = chessBoardTransform.rect.height;

        //checking turning point horizontally
        float chessBoardLeftBounds = chessBoardTransform.position.x - (chessBoardWidth / 2) - GameManager_ConnectGame.chessSize;
        float chessBoardRightBounds = chessBoardTransform.position.x + (chessBoardWidth / 2) + GameManager_ConnectGame.chessSize;

        Debug.Log(chessBoardLeftBounds);

        for (int i = (int)chessBoardLeftBounds; i <= (int)chessBoardRightBounds; i += GameManager_ConnectGame.chessSize)
        {
            if (i == transform.position.x)
            {
                continue;
            }

            Vector2 turningPoint = new Vector2(i, _originalChess.y);

            if (NoChessAtPosition(turningPoint) && CanConnectWith0Turining(_originalChess, turningPoint) && CanConnectWith1Turining(turningPoint, _targetChess))
            {
                //Vector2[] vetexList = new Vector2[2] { _originalChess, turningPoint };
                //DrawLine(vetexList);

                return true;
            }
        }

        //checking turning point vertically
        float chessBoardUpBounds = chessBoardTransform.position.y + (chessBoardHeight / 2) + GameManager_ConnectGame.chessSize;
        float chessBoardDownBounds = chessBoardTransform.position.y - (chessBoardHeight / 2) - GameManager_ConnectGame.chessSize;

        Debug.Log(chessBoardUpBounds);

        for (int i = (int)chessBoardDownBounds; i <= (int)chessBoardUpBounds; i += GameManager_ConnectGame.chessSize)
        {
            if (i == transform.position.y)
            {
                continue;
            }

            Vector2 turningPoint = new Vector2(_originalChess.x, i);

            if (NoChessAtPosition(turningPoint) && CanConnectWith0Turining(_originalChess, turningPoint) && CanConnectWith1Turining(turningPoint, _targetChess))
            {
                //Vector2[] vetexList = new Vector2[2] { _originalChess, turningPoint };
                //DrawLine(vetexList);

                return true;
            }
        }

        return false;

    }

    private bool NoChessAtPosition(Vector2 _position)
    {
        foreach (var chess in GameManager_ConnectGame.instance.chessList)
        {
            if (chess.transform.position.x == _position. x && chess.transform.position.y == _position.y)
            {
                return false;
            }
        }

        return true;
    }

    private void DrawLine(Vector2[] vetexList)
    {
        GameObject line = Instantiate(linePrefab, transform.position, Quaternion.identity);
        LineRenderer newLine = line.GetComponent<LineRenderer>();

        newLine.startWidth = 0.1f;
        newLine.startColor = Color.white;

        newLine.endWidth = 0.1f;
        newLine.endColor = Color.white;

        newLine.positionCount = vetexList.Length;
        for (int i = 0; i < vetexList.Length; i++)
        {
            newLine.SetPosition(i, vetexList[i]);
        }
    }
}
