using DCFApixels.DragonECS;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

internal class SpawnPlayerFiguresSystem : IEcsRun, IEcsInit
{

    [DI] private EcsDefaultWorld _world;
    [DI] private SceneData _sceneData; 
    [DI] private StaticData _staticData; 
    [DI] private RuntimeData _runtimeData;
    private Aspect _aspect;

    public int Index;

    class Aspect : EcsAspect
    {
        public EcsPool<FigureRef> FigureRefs = Inc;
        public EcsPool<InGrid> InGrids = Exc;
        public EcsPool<Target> Targets = Exc;
    }
    
    public void Run()
    {
        if (Time.time - _runtimeData.LastSpawnTime > _sceneData.SpawnDelay && _world.Where(out Aspect _).Count < _sceneData.MaxFigures)
        {
            _runtimeData.LastSpawnTime = Time.time;

            var e = _world.NewEntity();
            var point = _sceneData.SpawnPoints[UnityEngine.Random.Range(0, _sceneData.SpawnPoints.Length)];
            ref var figureRef = ref _aspect.FigureRefs.Add(e);
            
            figureRef.View = Object.Instantiate(_runtimeData.Figures[UnityEngine.Random.Range(0, _runtimeData.Figures.Length)], 
                point.position,
                Quaternion.Euler(0,0, Random.Range(0, 360))
            );
            figureRef.View.transform.localScale = new(
                _sceneData.GameField.Size.x/_sceneData.GameField.FieldSize.x, 
                _sceneData.GameField.Size.y/_sceneData.GameField.FieldSize.y, 
                1);

            figureRef.View.Entity = _world.GetEntityLong(e);
            figureRef.View.Index = ++Index;

            _runtimeData.IndexToFigure[figureRef.View.Index] = figureRef.View.Entity;
        }
    }

    public void Init()
    {
        _aspect = _world.GetAspect<Aspect>();
    }
}