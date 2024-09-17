using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Box : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private Color completeColor;
    private Color originalColor;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public bool CanMove(Vector2 _moveDirection)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)_moveDirection * 1.8f, _moveDirection, 2.5f);

        if (!hit || hit.collider.CompareTag("Destination"))
        {
            transform.Translate(_moveDirection * 4f);
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y + 1.8f), new Vector2(transform.position.x, 5f + transform.position.y));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Destination"))
        {
            sr.color = completeColor;
            GameManager_SokobanGame.instance.IncreaseCompletedBoxAmount();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Destination"))
        {
            sr.color = originalColor;
            GameManager_SokobanGame.instance.DecreaseCompletedBoxAmount();
        }
    }
}
