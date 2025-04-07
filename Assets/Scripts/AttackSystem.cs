using DCFApixels.DragonECS;
using PrimeTween;
using UnityEngine;

internal class AttackSystem : IEcsRun
{
    [DI] private EcsDefaultWorld _world;
    [DI] private SceneData _sceneData;
    [DI] private RuntimeData _runtimeData;
    [DI] private StaticData _staticData;

    
    class TentacleAspect : EcsAspect
    {
        public EcsPool<Attack> Attacks = Inc;
        public EcsPool<TentacleRef> TentacleRefs = Inc;
        public EcsPool<ShipRef> ShipRef = Opt;
        public EcsPool<Health> HealthRef = Opt;
        public EcsPool<Kill> Kill = Opt;
        public EcsPool<ReturnToStartPosition> ReturnToStartPosition = Opt;
        public EcsPool<MoveToMouth> MoveToMouth = Opt;
        public EcsPool<Delay> Dealys = Opt;
    }

    public void Run()
    {
        foreach (var e in _world.Where(out TentacleAspect a))
        {
            var tentacleRef = a.TentacleRefs.Get(e);
            var monsterTentacle = tentacleRef.SpriteShapeController;
            if (a.Attacks.Get(e).Target.TryGetID(out var shipEntity))
            {
                var view = a.ShipRef.Get(shipEntity).View;
                var shipPosition = view.AttackTarget.position;
                var spline = monsterTentacle.spline;
                var pointCount = spline.GetPointCount();
                var targetPosition = monsterTentacle.transform.InverseTransformPoint(shipPosition);
                var start = spline.GetPosition(0);
                var seq = Sequence.Create();
                var normal = Vector2.Perpendicular((Vector2)(targetPosition - start));
              
                for (int i = 1; i < pointCount; i++)
                {
                    var copiedIndex = i;
                    var target = targetPosition + (Vector3)((Random.value - 0.5f) * _staticData.RandomForTentacle * normal) ;
                    if (i == pointCount - 1)
                    {
                        target = targetPosition;
                    }
                    seq.Group(Tween.Custom(spline.GetPosition(copiedIndex), Vector3.Lerp(start, target, (float)copiedIndex / (pointCount - 1)), 
                        _staticData.TentacleAnimationTime,
                        x =>
                        {
                            spline.SetPosition(copiedIndex, x);
                            monsterTentacle.BakeMesh();
                        },
                        _staticData.TentacleEase));
                }

                a.Dealys.TryAddOrGet(e).Time = _staticData.TentacleAnimationTime;

                seq.OnComplete(() =>
                {
                    ref var health = ref a.HealthRef.Get(shipEntity);
                    health.Current -= _runtimeData.MonsterAttack;
                    if (health.Current <= 0)
                    {
                        a.Kill.TryAddOrGet(shipEntity);
                        a.MoveToMouth.TryAddOrGet(e).Target = view.transform;
                    }
                    else
                    {
                        
                        a.ReturnToStartPosition.TryAddOrGet(e);
                    }

                    tentacleRef.AttackParticle.transform.position = shipPosition;
                    tentacleRef.AttackParticle.Play();
                    
                });
                a.Attacks.Del(e);
            }
            else
            {
                a.Attacks.Del(e);
                a.ReturnToStartPosition.TryAddOrGet(e);
            }
        }
    }
}