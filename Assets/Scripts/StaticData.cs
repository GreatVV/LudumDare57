using System;
using PrimeTween;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu]
public class StaticData : ScriptableObject
{
   
    public LayerMask FigureLayer;
    public float FigureRotationSpeed = 360;
    public LevelTarget[] Levels;
    public Ship[] Ships;
    public float TentacleSpeed = 10;
    public HealthBarView HealthBar;
    public float TentacleAnimationTime;
    public float RandomForTentacle;
    public Ease TentacleEase;
    public ParticleSystem AttackParticle;
}