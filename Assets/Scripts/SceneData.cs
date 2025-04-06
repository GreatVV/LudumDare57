using System.Collections.Generic;
using DCFApixels.DragonECS;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public GameField GameField;

    public float SpawnDelay = 2f;
    public int MaxFigures = 5;
    public Camera Camera;

    public Monster Monster;

    public Transform SpawnShipPosition;
    public Transform EndShipPosition;

    
}