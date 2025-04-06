using DCFApixels.DragonECS;
using UnityEngine;

internal struct SpawnHealthBar : IEcsComponent
{
    public HealthBarView Prefab;
    public Transform Parent;
}