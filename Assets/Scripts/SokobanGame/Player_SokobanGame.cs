using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SokobanGame : MonoBehaviour
{
    Vector2 moveDirection;
    [SerializeField] private LayerMask whatIsWallAndBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            moveDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            moveDirection = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            moveDirection = Vector2.down;
        }
        else if(Input.GetKeyDown(KeyCode.W))
        {
            moveDirection = Vector2.up;
        }

        if(moveDirection != Vector2.zero)
        {
            if(CanMove(moveDirection))
            {
                Move(moveDirection);
            }
        }

        moveDirection = Vector2.zero;

    }

    private bool CanMove(Vector2 _moveDirection)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _moveDirection, 5f, whatIsWallAndBox);

        if (!hit)
        {
            return true;
        }
        else
        {
            if (hit.collider.GetComponent<Box>() != null)
            {
               return hit.collider.GetComponent<Box>().CanMove(_moveDirection);
            }
        }

        return false;
    }

    private void Move(Vector2 _moveDirection)
    {
        transform.Translate(_moveDirection * 4);
    }
}
