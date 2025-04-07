using DCFApixels.DragonECS;
using UnityEngine;
using UnityEngine.U2D;

internal struct TentacleRef : IEcsComponent
{
    public SpriteShapeController SpriteShapeController;
    public Vector3 StartPosition;
    public Vector3 MouthPosition;
    public ParticleSystem AttackParticle;
}