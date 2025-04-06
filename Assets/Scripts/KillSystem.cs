using DCFApixels.DragonECS;
using UnityEngine;

internal class KillSystem : IEcsRun
{
    [DI] private EcsDefaultWorld _world;
    [DI] private SceneData _sceneData;
    class Aspect : EcsAspect
    {
        public EcsPool<FigureRef> FigureRefs = Inc;
        public EcsPool<Kill> Kills = Inc;
        public EcsPool<InGrid> InGrids = Opt;

    }
    
    public void Run()
    {
        foreach (var e in _world.Where(out Aspect a))
        {
            var figure = a.FigureRefs.Get(e).View;
            if (a.InGrids.Has(e))
            {
                var gameField = _sceneData.GameField;
                foreach (var point in figure.Points)
                {
                    var rotatedPoint = figure.transform.rotation * (Vector2)point;
                    gameField.SetTaken(0, new Vector2Int(Mathf.RoundToInt(rotatedPoint.x), Mathf.RoundToInt(rotatedPoint.y)) + a.InGrids.Get(e).Position);
                }
            }
            
            figure.Kill();
            _world.DelEntity(e);
        }
    }
}