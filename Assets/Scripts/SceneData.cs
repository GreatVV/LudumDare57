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
    public GameObject Win;
    public float WinDelay = 2f;
    public GameObject Lose;
}