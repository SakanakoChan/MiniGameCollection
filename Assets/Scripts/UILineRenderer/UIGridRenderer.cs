using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIGridRenderer : Graphic
{
    public float paddingBetweenCellAndGrid = 10f;

    public Vector2Int gridAmount = new Vector2Int(1, 1);
    private float cellWidth;
    private float cellHeight;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        cellWidth = width / gridAmount.x;
        cellHeight = height / gridAmount.y;

        //draw those seperation sqaures between "grids" to make it looks like there are many grids
        int count = 0;
        for (int y = 0; y < gridAmount.y; y++)
        {
            for (int x = 0; x < gridAmount.x; x++)
            {
                DrawCell(vh, x, y, count);
                count++;
            }
        }
    }

    private void DrawCell(VertexHelper vh, int x, int y, int index)
    {
        float xPos = cellWidth * x;
        float yPos = cellHeight * y;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = new Vector3(xPos, yPos);
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos, yPos + cellHeight);
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos + cellWidth, yPos + cellHeight);
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos + cellWidth, yPos);
        vh.AddVert(vertex);

        //vh.AddTriangle(0, 1, 2);
        //vh.AddTriangle(0, 3, 2);

        float padding = paddingBetweenCellAndGrid;

        vertex.position = new Vector3(xPos + padding, yPos + padding);
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos + padding, yPos + cellHeight - padding);
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos + cellWidth - padding, yPos + cellHeight - padding);
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos + cellWidth - padding, yPos + padding);
        vh.AddVert(vertex);

        int offset = index * 8;

        //left edge
        vh.AddTriangle(0 + offset, 1 + offset, 5 + offset);
        vh.AddTriangle(5 + offset, 4 + offset, 0 + offset);

        //top edge
        vh.AddTriangle(1 + offset, 2 + offset, 6 + offset);
        vh.AddTriangle(6 + offset, 5 + offset, 1 + offset);

        //right edge
        vh.AddTriangle(2 + offset, 3 + offset, 7 + offset);
        vh.AddTriangle(7 + offset, 6 + offset, 2 + offset);

        //bottom edge
        vh.AddTriangle(3 + offset, 0 + offset, 4 + offset);
        vh.AddTriangle(4 + offset, 7 + offset, 3 + offset);
    }
}
