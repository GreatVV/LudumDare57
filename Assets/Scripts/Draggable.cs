using DCFApixels.DragonECS;
using UnityEngine;

internal struct Draggable : IEcsComponent
{
    public Vector3 StartPosition;
    public Vector3 StartMouseWorldPosition;
    public int StartRotation;
    public Vector3 CurrentRotation;
}