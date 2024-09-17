using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public enum CarType
{
    Horizontal,
    Vertical
};

public class Car : MonoBehaviour
{
    [SerializeField] private CarType carType;
    [SerializeField] private float carMoveSpeed;

    private Rigidbody2D rb;

    private bool isDragingCar;

    RigidbodyConstraints2D originalRBConstraints;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        originalRBConstraints = rb.constraints;
    }

    protected virtual void Update()
    {
        if (isDragingCar)
        {
            rb.constraints = originalRBConstraints;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (carType == CarType.Horizontal)
            {
                if (transform.position.x != mousePosition.x)
                {
                    rb.velocity = new Vector2((mousePosition.x - transform.position.x) * carMoveSpeed, 0);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }
            else if (carType == CarType.Vertical)
            {
                if (transform.position.y != mousePosition.y)
                {
                    rb.velocity = new Vector2(0, (mousePosition.y - transform.position.y) * carMoveSpeed);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            rb.velocity = Vector2.zero;
        }
    }

    private void OnMouseDown()
    {
        isDragingCar = true;
    }

    private void OnMouseUp()
    {
        isDragingCar = false;
    }
}
