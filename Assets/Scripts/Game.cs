using System;
using DCFApixels.DragonECS;
using UnityEngine;

public class Game : MonoBehaviour
{
    private EcsDefaultWorld _defaultWorld;
    EcsPipeline _pipeline;
    
    void Start()
    {
        _pipeline = EcsPipeline.New()
            .AddUnityDebug(_defaultWorld)
            .Inject(_defaultWorld)
            .BuildAndInit();
    }

    // Update is called once per frame
    void Update()
    {
        _pipeline.Run();
    }

    private void OnDestroy()
    {
        _defaultWorld.Destroy();
        _pipeline.Destroy();
    }
}
