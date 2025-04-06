using DCFApixels.DragonECS;
using TMPro;
using UnityEngine;

internal class SpawnShipSystem : IEcsRun,IEcsInit
{
    [DI] private EcsDefaultWorld _world;
    [DI] private RuntimeData _runtimeData;
    [DI] private SceneData _sceneData;
    [DI] private StaticData _staticData;

    
    class Aspect : EcsAspect
    {
        public EcsPool<SpawnShip> SpawnShips = Inc;
        public EcsPool<Delay> Delays = Exc;
    }
    
    public void Run()
    {
        foreach (var e in _world.Where(out Aspect a))
        {
            var spawnShip = a.SpawnShips.Get(e);
            var ship = Object.Instantiate(spawnShip.Prefab);
            ship.transform.position = _sceneData.SpawnShipPosition.position;
            
            var shipEntity = _world.NewEntity();
            _world.GetPool<ShipRef>().Add(shipEntity).View = ship;

            ship.Entity = _world.GetEntityLong(shipEntity);
            
            _world.GetPool<Speed>().Add(shipEntity).Value = spawnShip.Speed;
            ref var health = ref _world.GetPool<Health>().Add(shipEntity);
            health.Current = health.Max = spawnShip.Health;

            ref var spawnHealthBar = ref _world.GetPool<SpawnHealthBar>().Add(shipEntity);
            spawnHealthBar.Parent = ship.HealthBarRoot;
            spawnHealthBar.Prefab = _staticData.HealthBar;
            
            _runtimeData.ActiveShips.Add(ship.Entity);

            _world.DelEntity(e);
        }
    }

    public void Init()
    {
        foreach (var shipInfo in _runtimeData.LevelTarget.Ships)
        {
            var e = _world.NewEntity();
            ref var spawnShip = ref _world.GetPool<SpawnShip>().Add(e);
            spawnShip.Prefab = shipInfo.Ship;
            spawnShip.Speed = shipInfo.Speed;
            spawnShip.Health = shipInfo.Health;
            _world.GetPool<Delay>().Add(e).Time = shipInfo.Time;
        }
        
        
    }
}