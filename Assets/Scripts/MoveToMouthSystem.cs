using System;
using DCFApixels.DragonECS;
using PrimeTween;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

class MoveToMouthSystem : IEcsRun
{
    [DI] private EcsDefaultWorld _world;
    [DI] private StaticData _staticData;

    class Aspect : EcsAspect
    {
        public EcsPool<MoveToMouth> MoveToMouth = Inc;
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
            var targetGameObject = a.MoveToMouth.Get(e).Target;
            for (int i = 1; i < pointCount; i++)
            {
                var copiedIndex = i;
                seq.Group(Tween.Custom(spline.GetPosition(copiedIndex), Vector3.Lerp(start, tentacleRef.MouthPosition + i * Vector3.one * 0.01f, (float)copiedIndex / (pointCount - 1)), _staticData.TentacleAnimationTime,
                    x =>
                    {
                        try
                        {
                            spline.SetPosition(copiedIndex, x);
                            monsterTentacle.BakeMesh();
                        }
                        catch (Exception ex)
                        {
                            Debug.LogException(ex);
                        }

                        if (copiedIndex == pointCount - 1)
                        {
                            targetGameObject.position = monsterTentacle.transform.TransformPoint(x);
                        }
                    }));
            }
            
            seq.OnComplete(() =>
            {
                Object.Destroy(targetGameObject.gameObject);
            });
            a.Dealys.TryAddOrGet(e).Time = _staticData.TentacleAnimationTime;
            
            a.MoveToMouth.TryDel(e);
        }
    }
}