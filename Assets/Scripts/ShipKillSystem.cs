using DCFApixels.DragonECS;
using UnityEngine;

internal class ShipKillSystem : IEcsRun
{
    [DI] private EcsDefaultWorld _world;
    [DI] private RuntimeData _runtimeData;
    [DI] SceneData _sceneData;
    class Ship : EcsAspect
    {
        public EcsPool<ShipRef> Ships = Inc;
        public EcsPool<Kill> Kills = Inc;
    }
    public void Run()
    {
        foreach (var e in _world.Where(out Ship aspect))
        {
            var view = aspect.Ships.Get(e).View;
            _runtimeData.ActiveShips.Remove(view.Entity);
            view.PlayDeath();

            _runtimeData.KilledShip++;
            if (_runtimeData.KilledShip + _runtimeData.LostShips >= _runtimeData.TargetToKill)
            {
                ShipMoveSystem.PlayEndLevelSequence(_runtimeData, _sceneData);
            }
            _world.DelEntity(e);
        }
    }
    
}