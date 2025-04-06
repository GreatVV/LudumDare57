using System;
using UnityEngine;

public class GameField : MonoBehaviour
{
    public Transform FieldRoot;
    public Vector2 Size;
    public Vector2Int FieldSize;

    public int[] Taken = new int[0];

    private void OnValidate()
    {
        if (!FieldRoot)
        {
            FieldRoot = transform;
        }

        if (Taken == default || FieldSize.x * FieldSize.y != Taken.Length)
        {
            Taken = new int[FieldSize.x * FieldSize.y];
        }
    }

    public Vector3 CenterPositionFor(Vector2Int pos)
    {
        return CenterPositionFor(pos.x, pos.y);
    }

    public Vector3 CenterPositionFor(int x, int y)
    {
        var leftBottom = FieldRoot.position - (Vector3)(Size * .5f);
        var width = Size.x / FieldSize.x;
        var height = Size.y / FieldSize.y;
        var position =
            leftBottom + new Vector3(x * width + width * .5f, y * height + height * .5f, 0);
        return position;
    }

    public void SetTaken(int itemId, Vector2Int pos)
    {
        Taken[Index(pos.x, pos.y)] = itemId;
    }

    public int Index(int x, int y)
    {
        return x * FieldSize.y + y;
    }

    private void OnDrawGizmos()
    {
        var leftBottom = FieldRoot.position -(Vector3)(Size * .5f);
        var width = Size.x / FieldSize.x;
        var height = Size.y / FieldSize.y;
        
        for (int x = 0; x < FieldSize.x; x++)
        {
            for (int y = 0; y < FieldSize.y; y++)
            {
                var position = 
                    leftBottom + new Vector3(x * width + width * .5f, y * height+height*.5f, 0);

                Gizmos.color = Taken[Index(x, y)] != 0 ? Color.red : Color.green;
                Gizmos.DrawCube(position, new(width, height, 1));
            }
        }
    }

    public Vector2Int ToPosition(Vector3 point)
    {
        var leftBottom = FieldRoot.position -(Vector3)(Size * .5f);
        var diff = point - leftBottom;
        var width = Size.x / FieldSize.x;
        var height = Size.y / FieldSize.y;
        return new(Mathf.FloorToInt(diff.x /width ), Mathf.FloorToInt(diff.y / height));
    }

    public int IsTaken(Vector2Int vector2Int)
    {
        return Taken[Index(vector2Int.x, vector2Int.y)];
    }
}