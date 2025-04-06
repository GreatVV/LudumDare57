
using DCFApixels.DragonECS;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

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
            
            
            .Add(new SpawnSystem())
            .Add(new DragSystem())
            
            .Inject(_defaultWorld)
            .Inject(RuntimeData)
            .Inject(StaticData)
            .Inject(SceneData)
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

internal struct Draggable : IEcsComponent
{
    public Vector3 StartPosition;
    public Vector3 StartMouseWorldPosition;
    public float StartRotation;
}

internal struct FigureRef : IEcsComponent
{
    public Figure View;
}