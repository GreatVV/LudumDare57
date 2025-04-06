using DCFApixels.DragonECS;
using UnityEngine;

class ReturnToStartMouthSystem : IEcsRun
{
    [DI] private EcsDefaultWorld _world;
    [DI] private StaticData _staticData;

    class Aspect : EcsAspect
    {
        public EcsPool<ReturnToStartPosition> ReturnToStartPositions = Inc;
        public EcsPool<TentacleRef> TentacleRefs = Inc;
    }


    public void Run()
    {
        foreach (var e in _world.Where(out Aspect a))
        {
            var tentacleRef = a.TentacleRefs.Get(e);
            var monsterTentacle = tentacleRef.SpriteShapeController;
            
            var spline = monsterTentacle.spline;
            var index = spline.GetPointCount() - 1;
            var current = spline.GetPosition(index);
            var target = Vector3.MoveTowards(current, tentacleRef.StartPosition, _staticData.TentacleSpeed * Time.deltaTime);
            if (target == tentacleRef.StartPosition)
            {
                a.ReturnToStartPositions.TryDel(e);
            }

            monsterTentacle.spline.SetPosition(index, target);
            monsterTentacle.BakeMesh();
        }
    }
}