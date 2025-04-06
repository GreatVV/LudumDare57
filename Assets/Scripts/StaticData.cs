using UnityEngine;

[CreateAssetMenu]
public class StaticData : ScriptableObject
{
   
    public LayerMask FigureLayer;
    public float FigureRotationSpeed = 360;
    public LevelTarget[] Levels;
    public Ship[] Ships;
    public float TentacleSpeed = 10;
    public HealthBarView HealthBar;
}