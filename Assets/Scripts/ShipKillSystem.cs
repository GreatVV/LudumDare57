using System.Linq;
using DCFApixels.DragonECS;

internal class ShipKillSystem : IEcsRun
{
    [DI] private EcsDefaultWorld _world;
    [DI] private RuntimeData _runtimeData;
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
            _world.DelEntity(e);
        }
    }
}