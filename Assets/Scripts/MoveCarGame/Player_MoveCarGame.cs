using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MoveCarGame : Car
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Destination"))
        {
            Debug.Log("Game passed!");
        }
    }
}
