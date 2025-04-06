using System.Collections.Generic;
using DCFApixels.DragonECS;
using UnityEngine;

internal class CheckFieldSystem : IEcsRun
{
    [DI] EcsDefaultWorld _world;
    [DI] public RuntimeData _runtimeData;
    [DI] public StaticData _staticData;
    [DI] SceneData _sceneData;

    class Aspect : EcsAspect
    {
        public EcsPool<FigureRef> Figures = Inc;
        public EcsPool<Target> Targets = Inc;
        public EcsPool<Kill> Kills = Exc;
    }
    
    List<int> FiguresToKill = new();
    public void Run()
    {
        foreach (var e in _world.Where(out SingleAspect<CheckField> a))
        {
            foreach (var tf in _world.Where(out Aspect targetAspect))
            {
                var figureRef = targetAspect.Figures.Get(tf);
                var figure = figureRef.View;
                var position = targetAspect.Targets.Get(tf).Position;
                var gameField = _sceneData.GameField;
                var completed = true;
                FiguresToKill.Clear();
                foreach (var point in figureRef.View.Points)
                {
                    var rotatedPoint = figure.transform.rotation * (Vector2)point;
                    var itemInPosition = gameField.ItemInPosition( new Vector2Int(Mathf.RoundToInt(rotatedPoint.x), Mathf.RoundToInt(rotatedPoint.y)) + position);
                    if (itemInPosition == 0)
                    {
                        completed = false;
                        break;
                    }

                    FiguresToKill.Add(itemInPosition);
                }

                if (completed)
                {
                    foreach (var figureIndex in FiguresToKill)
                    {
                        var entlong = _runtimeData.IndexToFigure[figureIndex];
                        if (entlong.TryGetID(out var cover))
                        {
                            targetAspect.Kills.TryAddOrGet(cover);
                        }
                    }
                    
                    targetAspect.Kills.TryAddOrGet(tf);
                }
            }

            if (_world.Where(out Aspect _).Count == 0)
            {
                _world.GetPool<SpawnNextStage>().NewEntity();
            }
            
            a.pool.Del(e);
        }
    }
}