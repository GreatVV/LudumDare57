using DCFApixels.DragonECS;
using UnityEngine;

internal class HealthBarSystem : IEcsRun
{
    [DI] private EcsDefaultWorld _world;
    [DI] private StaticData _staticData;
    [DI] private SceneData _sceneData;

    class NoHealthBarAspect : EcsAspect
    {
        public EcsPool<Health> Healths = Inc;
        public EcsPool<SpawnHealthBar> SpawnHealthBars = Inc;
        public EcsPool<HealthBarRef> HealthBarRefs = Exc;
    }

    class Aspect : EcsAspect
    {
        public EcsPool<Health> Healths = Inc;
        public EcsPool<HealthBarRef> HealthBarRefs = Inc;
    }
    public void Run()
    {
        foreach (var e in _world.Where(out NoHealthBarAspect a))
        {
            ref var healthBarRef = ref a.HealthBarRefs.Add(e);
            ref var spawnHealthBar = ref a.SpawnHealthBars.Get(e);
            var healthBar = Object.Instantiate(spawnHealthBar.Prefab, spawnHealthBar.Parent);
            healthBarRef.View = healthBar;
            healthBarRef.PrevValue = 1f;
            healthBarRef.LastChangeTime = Time.time;
            healthBar.gameObject.SetActive(false);
            a.SpawnHealthBars.Del(e);
        }

        foreach (var e in _world.Where(out Aspect a))
        {
            ref var healthBarRef = ref a.HealthBarRefs.Get(e);
            ref var health = ref a.Healths.Get(e);
            var healthBar = healthBarRef.View;
            healthBar.transform.rotation = _sceneData.Camera.transform.rotation;
            if (health.Current <= 0 || Mathf.Approximately(health.Current, health.Max))
            {
                healthBar.gameObject.SetActive(false);
            }
            else
            {
                healthBar.gameObject.SetActive(true);
                healthBar.Set(health);
                    
                if (healthBar.AdditionalSpriteRenderer && !Mathf.Approximately(healthBarRef.PrevValue, health.Current/health.Max))
                {
                    healthBarRef.PrevValue = health.Current/health.Max;
                    healthBarRef.LastChangeTime = Time.time;
                }
                if (healthBar.AdditionalSpriteRenderer && Time.time - healthBarRef.LastChangeTime > healthBar.WaitTime)
                {
                    healthBar.AdditionalSpriteRenderer.size = new(Mathf.Max(Mathf.MoveTowards(healthBar.AdditionalSpriteRenderer.size.x, health.Current/health.Max, Time.deltaTime * healthBar.Speed), 0), 1);
                }
            }
        }
    }
}