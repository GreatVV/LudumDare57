using DCFApixels.DragonECS;
using UnityEngine;
using UnityEngine.U2D;

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
    }

    public void Run()
    {
        foreach (var e in _world.Where(out TentacleAspect a))
        {
            var monsterTentacle = a.TentacleRefs.Get(e).SpriteShapeController;
            if (a.Attacks.Get(e).Target.TryGetID(out var shipEntity))
            {
                var shipPosition = a.ShipRef.Get(shipEntity).View.AttackTarget.position;
                var spline = monsterTentacle.spline;
                var index = spline.GetPointCount() - 1;
                var current = spline.GetPosition(index);
                var targetPosition = monsterTentacle.transform.InverseTransformPoint(shipPosition);
                var target = Vector3.MoveTowards(current, targetPosition, _staticData.TentacleSpeed * Time.deltaTime);
                if (target == targetPosition)
                {
                    ref var health = ref a.HealthRef.Get(shipEntity);
                    health.Current -= _runtimeData.MonsterAttack;
                    if (health.Current <= 0)
                    {
                        a.Kill.TryAddOrGet(shipEntity);
                    }
                    
                    a.Attacks.Del(e);
                }

                monsterTentacle.spline.SetPosition(index, target);
                monsterTentacle.BakeMesh();
            }
        }
    }
}

internal struct TentacleRef : IEcsComponent
{
    public SpriteShapeController SpriteShapeController;
}