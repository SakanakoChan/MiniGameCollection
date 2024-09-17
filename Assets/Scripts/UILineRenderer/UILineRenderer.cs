using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UILineRenderer : Graphic
{
    public List<Vector2> points;

    public float lineThickness;


    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points == null || points.Count < 2 )
        {
            return;
        }

        for (int i = 0; i < points.Count - 1; i++)
        {
            AddTwoPointsForLine(points[i], points[i + 1], vh);

            int index = i * 5;

            vh.AddTriangle(index, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index);

            if (i != 0)
            {
                vh.AddTriangle(index, index - 1, index - 3);
                vh.AddTriangle(index + 1, index - 1, index - 2);
            }
        }

    }

    public void AddTwoPointsForLine(Vector3 _point1, Vector3 _point2, VertexHelper _vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        //add _point1
        Quaternion rotationAngle = Quaternion.Euler(0, 0, GetAngle(_point1, _point2) + 90f);
        vertex.position = rotationAngle * new Vector3(-lineThickness / 2, 0);
        vertex.position += _point1;
        _vh.AddVert(vertex);

        vertex.position = rotationAngle * new Vector3(lineThickness / 2, 0);
        vertex.position += _point1;
        _vh.AddVert(vertex);

        //add _point2
        rotationAngle = Quaternion.Euler(0, 0, GetAngle(_point2, _point1) - 90);
        vertex.position = rotationAngle * new Vector3(-lineThickness / 2, 0);
        vertex.position += _point2;
        _vh.AddVert(vertex);

        vertex.position = rotationAngle * new Vector3(lineThickness / 2, 0);
        vertex.position += _point2;
        _vh.AddVert(vertex);

        vertex.position = _point2;
        _vh.AddVert(vertex);
    }

    public float GetAngle(Vector2 _point1, Vector2 _point2)
    {
        return (float)(Mathf.Atan2(_point2.y - _point1.y, _point2.x - _point1.x) * (180 / Mathf.PI));
    }
}
