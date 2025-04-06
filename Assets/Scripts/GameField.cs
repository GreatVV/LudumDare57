using UnityEngine;

public class GameField : MonoBehaviour
{
    public Transform FieldRoot;
    public Vector2 Size;
    public Vector2Int FieldSize;

    private void OnValidate()
    {
        if (!FieldRoot)
        {
            FieldRoot = transform;
        }
    }

    public Vector3 CenterPositionFor(int x, int y)
    {
        var leftBottom = FieldRoot.position -(Vector3)(Size * .5f);
        var width = Size.x / FieldSize.x;
        var height = Size.y / FieldSize.y;
        var position = 
            leftBottom + new Vector3(x * width + width * .5f, y * height+height*.5f, 0);
        return position;
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
                Gizmos.DrawWireCube(position, new(width, height, 1));
            }
        }
    }
}