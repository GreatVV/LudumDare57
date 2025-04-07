using DCFApixels.DragonECS;
using PrimeTween;
using UnityEngine;

class ReturnToStartMouthSystem : IEcsRun
{
    [DI] private EcsDefaultWorld _world;
    [DI] private StaticData _staticData;

    class Aspect : EcsAspect
    {
        public EcsPool<ReturnToStartPosition> ReturnToStartPositions = Inc;
        public EcsPool<TentacleRef> TentacleRefs = Inc;
        public EcsPool<Delay> Dealys = Opt;
    }


    public void Run()
    {
        foreach (var e in _world.Where(out Aspect a))
        {
            var tentacleRef = a.TentacleRefs.Get(e);
            var monsterTentacle = tentacleRef.SpriteShapeController;
            
            var spline = monsterTentacle.spline;
            var start = spline.GetPosition(0);
            var pointCount = spline.GetPointCount();
            var seq = Sequence.Create();
            for (int i = 1; i < pointCount; i++)
            {
                var copiedIndex = i;
                seq.Group(Tween.Custom(spline.GetPosition(copiedIndex), Vector3.Lerp(start, tentacleRef.StartPosition, (float)copiedIndex / (pointCount - 1)), _staticData.TentacleAnimationTime,
                    x =>
                    {
                        spline.SetPosition(copiedIndex, x);
                        monsterTentacle.BakeMesh();
                    }));
            }

            a.Dealys.TryAddOrGet(e).Time = _staticData.TentacleAnimationTime;
            a.ReturnToStartPositions.TryDel(e);
        }
    }
}