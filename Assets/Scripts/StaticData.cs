using UnityEngine;

[CreateAssetMenu]
public class StaticData : ScriptableObject
{
    public Figure[] Figures;
    public LayerMask FigureLayer;
    public float FigureRotationSpeed = 360;
    public LevelTarget[] Levels;
}