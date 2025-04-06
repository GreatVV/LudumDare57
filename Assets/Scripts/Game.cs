using System;
using DCFApixels.DragonECS;
using UnityEngine;
using Debug = System.Diagnostics.Debug;
using Random = Unity.Mathematics.Random;

public class Game : MonoBehaviour
{
    private EcsDefaultWorld _defaultWorld;
    EcsPipeline _pipeline;

    public SceneData SceneData;
    public RuntimeData RuntimeData;
    public StaticData StaticData;
    
    void Start()
    {
        _defaultWorld = new EcsDefaultWorld();
        
        _pipeline = EcsPipeline.New()
            .AddUnityDebug(_defaultWorld)
            
            .Add(new SpawnStageSystem())
            .Add(new SpawnShipSystem())
            .Add(new HealthBarSystem())
            .Add(new SpawnPlayerFiguresSystem())
            .Add(new ShipMoveSystem())
            .Add(new DragSystem())
            .Add(new CheckFieldSystem())
            .Add(new KillSystem())
            .Add(new AttackSystem())
            .Add(new ShipKillSystem())
            .Add(new DelaySystem())
            
            .Inject(_defaultWorld)
            .Inject(RuntimeData)
            .Inject(StaticData)
            .Inject(SceneData)
            .Inject(ProfileService.Instance)
            .AutoInject()
            .BuildAndInit();
    }
    
    void Update()
    {
        _pipeline?.Run();
    }

    private void OnDestroy()
    {
        _defaultWorld.Destroy();
        _pipeline.Destroy();
    }
}