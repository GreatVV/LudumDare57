using DCFApixels.DragonECS;
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
    }

    public void Run()
    {
        foreach (var e in _world.Where(out TentacleAspect a))
        {
            var monsterTentacle = a.TentacleRefs.Get(e).SpriteShapeController;
            if (a.Attacks.Get(e).Target.TryGetID(out var shipEntity))
            {
                var view = a.ShipRef.Get(shipEntity).View;
                var shipPosition = view.AttackTarget.position;
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
                        a.MoveToMouth.TryAddOrGet(e).Target = view.transform;
                    } 
                    else 
                    {
                        a.ReturnToStartPosition.TryAddOrGet(e);
                    }
                    a.Attacks.Del(e);
                }

                monsterTentacle.spline.SetPosition(index, target);
                monsterTentacle.BakeMesh();
            }
            else
            {
                a.Attacks.Del(e);
                a.ReturnToStartPosition.TryAddOrGet(e);
            }
        }
    }
}