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
    }

    public void Run()
    {
        foreach (var e in _world.Where(out TentacleAspect a))
        {
            var monsterTentacle = a.TentacleRefs.Get(e).SpriteShapeController;
            if (a.Attacks.Get(e).Target.TryGetID(out var shipEntity))
            {
                var shipPosition = a.ShipRef.Get(shipEntity).View.AttackTarget.position;
                var current = monsterTentacle.spline.GetPosition(1);
                var targetPosition =
                    monsterTentacle.transform.InverseTransformPoint(shipPosition);
                var target = Vector3.MoveTowards(current, targetPosition, _staticData.TentacleSpeed * Time.deltaTime);
                if (target == targetPosition)
                {
                    a.Attacks.Del(e);
                }

                monsterTentacle.spline.SetPosition(1, target);
                monsterTentacle.BakeMesh();
            }
        }
    }
}

internal struct TentacleRef : IEcsComponent
{
    public SpriteShapeController SpriteShapeController;
}