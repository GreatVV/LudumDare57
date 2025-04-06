using DCFApixels.DragonECS;
using UnityEngine;

internal class ShipMoveSystem : IEcsRun
{
    [DI] private EcsDefaultWorld _world;
    [DI] private SceneData _sceneData;
    class Aspect : EcsAspect
    {
        public EcsPool<ShipRef> ShipRefs = Inc;
        public EcsPool<Speed> Speeds = Inc;
    }
    
    public void Run()
    {
        foreach (var e in _world.Where(out Aspect a))
        {
            a.ShipRefs.Get(e).View.transform.position += 
                (_sceneData.EndShipPosition.position - _sceneData.SpawnShipPosition.position).normalized
                * a.Speeds.Get(e).Value * Time.deltaTime;
            ;
        }
    }
}