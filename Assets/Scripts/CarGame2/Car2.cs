using UnityEngine;

public class Car2 : MonoBehaviour
{
    public const int CELL_SIZE = 1;

    public int carLength;
    public int carWidth;
    public LayerMask canCollideWithWhat;
    //public float distanceToDestroyCar = 30;

    private bool isHorizontallyPlaced;
    private bool isMoving;

    private Vector3 forwardCheckPosition;
    private Vector3 backwardCheckPosition;

    private Vector3 destinationPosition = Vector3.negativeInfinity;
    //private static int destinationLayer;

    private float initialDistanceToDestination = float.NegativeInfinity;

    private BoxCollider2D cd;

    //private System.Action OnCarDestroyed;

    private void Start()
    {
        if (transform.right == Vector3.right)
        {
            isHorizontallyPlaced = true;
        }
        else
        {
            isHorizontallyPlaced = false;
            //Debug.Log("Is placed vertically");
        }

        UpdateRayCastCheckPosition();

        cd = GetComponent<BoxCollider2D>();

        //destinationLayer = LayerMask.NameToLayer("Destination");
        //Debug.Log(destinationLayer);
    }


    private void Update()
    {
        if (isMoving && destinationPosition != Vector3.negativeInfinity)
        {
            transform.position = Vector3.Lerp(transform.position, destinationPosition, 30f * (1f / initialDistanceToDestination) * Time.deltaTime);

            if (Vector3.Distance(transform.position, destinationPosition) < 0.05f)
            {
                transform.position = destinationPosition;
                UpdateRayCastCheckPosition();
                isMoving = false;
            }
        }

        if (Vector3.Distance(transform.position, Vector3.zero) >= GameManager_Car2.instance.distanceToDestroyCar)
        {
            GameManager_Car2.instance.DestroyCar(gameObject);
            //Car2GameManager.instance.OnCarDestroyed?.Invoke(gameObject);
            //Destroy(gameObject);
        }

        //Debug.Log(isMoving);
        //Debug.Log(cd.transform.position);
    }

    private void OnMouseDown()
    {
        if (!isMoving)
        {
            if (canGoForward())
            {
                isMoving = true;
                return;
            }
            else if (canGoBackward())
            {
                isMoving = true;
                return;
            }
            else
            {
                isMoving = false;
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawLine(forwardCheckPosition, forwardCheckPosition + transform.right * CELL_SIZE);
    //    Gizmos.DrawLine(backwardCheckPosition, backwardCheckPosition - transform.right * CELL_SIZE);
    //}

    private void UpdateRayCastCheckPosition()
    {
        if (isHorizontallyPlaced)
        {
            forwardCheckPosition = new Vector3(transform.position.x + 0.01f + (float)carLength / 2, transform.position.y, 0);
            backwardCheckPosition = new Vector3(transform.position.x - 0.01f - (float)carLength / 2, transform.position.y, 0);
        }
        else
        {
            forwardCheckPosition = new Vector3(transform.position.x, transform.position.y + 0.01f + (float)carLength / 2, 0);
            backwardCheckPosition = new Vector3(transform.position.x, transform.position.y - 0.01f - (float)carLength / 2, 0);
        }
    }


    private bool canGoForward()
    {
        if (!Physics2D.Raycast(forwardCheckPosition, transform.right, CELL_SIZE - 0.55f, canCollideWithWhat))
        {
            var hit = Physics2D.Raycast(forwardCheckPosition, transform.right, 100, canCollideWithWhat);
            if (hit)
            {
                //Debug.Log($"hit point is: {hit.point}");
                //Debug.Log($"hit target name is {hit.collider.gameObject}");

                //if can directly go to the destination
                //if (hit.collider.gameObject.layer == destinationLayer)
                //{
                //    destinationPosition = destinationPosition = hit.collider.gameObject.transform.position;
                //}
                //if can't go to the destination
                //else
                //{
                if (isHorizontallyPlaced)
                {
                    destinationPosition = new Vector3(RoundToNearestHalf(hit.point.x - (float)carLength / 2), RoundToNearestHalf(hit.point.y), 0);
                }
                else
                {
                    destinationPosition = new Vector3(RoundToNearestHalf(hit.point.x), RoundToNearestHalf(hit.point.y - (float)carLength / 2), 0);
                }
                //}
            }
            else
            {
                //destinationPosition = Vector3.negativeInfinity;
                destinationPosition = transform.position + transform.right * 100;
                cd.enabled = false;
            }

            initialDistanceToDestination = Vector3.Distance(transform.position, destinationPosition);
            //Debug.Log(destinationPosition);

            //Debug.Log("Can go forward");
            return true;
        }
        else
        {
            return false;
        }

    }

    private bool canGoBackward()
    {
        if (!Physics2D.Raycast(backwardCheckPosition, -transform.right, CELL_SIZE - 0.55f, canCollideWithWhat))
        {
            var hit = Physics2D.Raycast(backwardCheckPosition, -transform.right, 100, canCollideWithWhat);
            if (hit)
            {
                //Debug.Log($"hit point is: {hit.point}");
                //Debug.Log($"hit target name is {hit.collider.gameObject}");
                //if (hit.collider.gameObject.layer == destinationLayer)
                //{
                //    destinationPosition = hit.collider.gameObject.transform.position;
                //}
                //else
                //{
                if (isHorizontallyPlaced)
                {
                    destinationPosition = new Vector3(RoundToNearestHalf(hit.point.x + (float)carLength / 2), RoundToNearestHalf(hit.point.y), 0);
                }
                else
                {
                    destinationPosition = new Vector3(RoundToNearestHalf(hit.point.x), RoundToNearestHalf(hit.point.y + (float)carLength / 2), 0);
                }
                //}
            }
            else
            {
                //destinationPosition = Vector3.negativeInfinity;
                destinationPosition = transform.position - transform.right * 100;
                cd.enabled = false;
            }

            initialDistanceToDestination = Vector3.Distance(transform.position, destinationPosition);
            //Debug.Log(destinationPosition);

            //Debug.Log("Can go backward");
            return true;
        }
        else
        {
            return false;
        }
    }

    private float RoundToNearestHalf(float _number)
    {
        return Mathf.Round(_number * 2) / 2f;
    }
}
