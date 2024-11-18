using UnityEngine;

public class GameManager_Car2 : GameManager
{
    public static GameManager_Car2 instance;

    public float distanceToDestroyCar = 30;
    public Transform carParent;
    public GameObject gamePassHint;

    private int carCount;

    //public System.Action OnCarDestroyed;

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
        carCount = carParent.childCount;
    }

    public void DestroyCar(GameObject _car)
    {
        carCount--;
        Destroy(_car);

        if (carCount == 0)
        {
            gamePassHint.SetActive(true);
        }
    }
}
