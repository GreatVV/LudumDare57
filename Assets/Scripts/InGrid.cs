using System;
using DCFApixels.DragonECS;
using UnityEngine;

[Serializable]

internal struct InGrid : IEcsComponent
{
    public Vector2Int Position;
}