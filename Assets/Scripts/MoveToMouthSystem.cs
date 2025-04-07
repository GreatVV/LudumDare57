using System;
using DCFApixels.DragonECS;
using UnityEngine;
using Object = UnityEngine.Object;

class MoveToMouthSystem : IEcsRun
{
    [DI] private EcsDefaultWorld _world;
    [DI] private StaticData _staticData;

    class Aspect : EcsAspect
    {
        public EcsPool<MoveToMouth> MoveToMouth = Inc;
        public EcsPool<TentacleRef> TentacleRefs = Inc;
    }


    public void Run()
    {
        foreach (var e in _world.Where(out Aspect a))
        {
            var tentacleRef = a.TentacleRefs.Get(e);
            var monsterTentacle = tentacleRef.SpriteShapeController;

            var targetTransform = a.MoveToMouth.Get(e).Target;
            var spline = monsterTentacle.spline;
            var index = spline.GetPointCount() - 1;
            var current = spline.GetPosition(index);
            var targetPosition = monsterTentacle.transform.InverseTransformPoint(tentacleRef.MouthPosition);
            var target = Vector3.MoveTowards(current, targetPosition, _staticData.TentacleSpeed * Time.deltaTime);

            if (targetTransform)
            {
                targetTransform.position = monsterTentacle.transform.TransformPoint(target);
            }
            
            if (target == targetPosition)
            {
                Object.Destroy(a.MoveToMouth.Get(e).Target.gameObject);
                a.MoveToMouth.TryDel(e);
            }

            try
            {
                monsterTentacle.spline.SetPosition(index, target);
            }
            catch (Exception)
            {
                
            }

            monsterTentacle.BakeMesh();
        }
    }
}