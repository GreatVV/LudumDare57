using UnityEngine;

public class SceneData : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public GameField GameField;

    public float SpawnDelay = 2f;
    public int MaxFigures = 5;
    public int MaxTargetFigures = 5;
    public Camera Camera;
}