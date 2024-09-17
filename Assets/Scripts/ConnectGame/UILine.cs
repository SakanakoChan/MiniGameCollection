using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILine : UILineRenderer
{
    [SerializeField] private float SelfDestroyCountDown;
    private float timer;

    protected override void Start()
    {
        timer = SelfDestroyCountDown;

        rectTransform.position = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetVertex(List<Vector2> _vertexes)
    {
        points = _vertexes;
    }
}
