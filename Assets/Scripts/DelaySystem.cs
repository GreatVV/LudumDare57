using DCFApixels.DragonECS;
using UnityEngine;

internal class DelaySystem : IEcsRun
{
    [DI] private EcsDefaultWorld _world;
    
    public void Run()
    {
        foreach (var e in _world.Where(out SingleAspect<Delay> aspect))
        {
            ref var delay = ref aspect.pool.Get(e);
            delay.Time -= Time.deltaTime;
            if (delay.Time <= 0)
            {
                aspect.pool.Del(e);
            }
        }
    }
}