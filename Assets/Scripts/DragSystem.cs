using DCFApixels.DragonECS;
using UnityEngine;

internal class DragSystem : IEcsRun
{
    [DI] private EcsDefaultWorld _world;
    [DI] private SceneData _sceneData;
    [DI] private StaticData _staticData;
    class Aspect : EcsAspect
    {
        public EcsPool<FigureRef> Figures = Inc;
        public EcsPool<Draggable> Draggables = Inc;
        public EcsPool<Target> Targets = Exc;
        public EcsPool<InGrid> InGrids = Opt;
        public EcsPool<CheckField> CheckFields = Opt;
    }
    
    public void Run()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var worldPoint = _sceneData.Camera.ScreenToWorldPoint(Input.mousePosition);
            var collider = Physics2D.OverlapPoint(worldPoint, _staticData.FigureLayer);
            if (collider)
            {
                var figure = collider.GetComponentInParent<Figure>();
                if (figure.Entity.TryGetID(out var e))
                {
                    ref var draggable = ref _world.GetPool<Draggable>().TryAddOrGet(e);
                    draggable.StartPosition = figure.transform.position;
                    draggable.StartMouseWorldPosition = worldPoint;
                    draggable.StartRotation = (int) (Mathf.Round(figure.transform.localEulerAngles.z / 90f) * 90f);
                    draggable.CurrentRotation = figure.transform.localEulerAngles;

                    _world.GetPool<FigureRef>().Get(e).View.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

                    var inGrids = _world.GetPool<InGrid>();
                    var gameField = _sceneData.GameField;
                    if (inGrids.Has(e))
                    {
                        foreach (var point in figure.Points)
                        {
                            var rotatedPoint = figure.transform.rotation * (Vector2)point;
                            gameField.SetTaken(0, new Vector2Int(Mathf.RoundToInt(rotatedPoint.x), Mathf.RoundToInt(rotatedPoint.y)) + inGrids.Get(e).Position);
                        }
                        inGrids.Del(e);
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                foreach (var e in _world.Where(out Aspect a))
                {
                    var worldPoint = _sceneData.Camera.ScreenToWorldPoint(Input.mousePosition);
                    ref var draggable = ref a.Draggables.Get(e);
                    var diff = worldPoint - draggable.StartMouseWorldPosition;

                    if (Input.GetMouseButtonDown(1))
                    {
                        draggable.StartRotation -= 90;
                        draggable.StartRotation %= 360;
                    }

                    var figureRef = a.Figures.Get(e);
                    var figureRefView = figureRef.View;
                    figureRefView.transform.position = draggable.StartPosition + diff;
                    draggable.CurrentRotation = Vector3.MoveTowards(
                        draggable.CurrentRotation,
                        new Vector3(0, 0, draggable.StartRotation)
                        , Time.deltaTime * _staticData.FigureRotationSpeed);
                    figureRefView.transform.localEulerAngles = draggable.CurrentRotation;

                    var snap = false;
                    var gameField = _sceneData.GameField;
                    foreach (var point in figureRef.View.Points)
                    {
                        var wp = figureRefView.transform.TransformPoint(new Vector3(point.x, point.y, 0));
                        if (
                            wp.x > gameField.FieldRoot.position.x - gameField.Size.x / 2
                            && wp.x < gameField.FieldRoot.position.x + gameField.Size.x / 2
                            && wp.y > gameField.FieldRoot.position.y - gameField.Size.y / 2
                            && wp.y < gameField.FieldRoot.position.y + gameField.Size.y / 2
                        )
                        {
                            snap = true;
                            break;
                        }
                    }

                    if (snap)
                    {
                        var pos = figureRefView.transform.position;
                        var width = gameField.Size.x / gameField.FieldSize.x;
                        var height = gameField.Size.y / gameField.FieldSize.y;
                        var x = gameField.FieldRoot.position.x % width;
                        var y = gameField.FieldRoot.position.y % height;
                        float snappedX = Mathf.Round(pos.x / width) * width;
                        float snappedY = Mathf.Round(pos.y / height) * height;
                        figureRefView.transform.position = new(snappedX + x, snappedY + y, pos.z);
                    }
                }
            }
            else
            {
                foreach (var e in _world.Where(out Aspect a))
                {
                    var putToGrid = true;
                    var figure = a.Figures.Get(e).View;
                    var figureRef = a.Figures.Get(e);
                    var gameField = _sceneData.GameField;
                    var position = gameField.ToPosition(figure.transform.position);
                    foreach (var point in figureRef.View.Points)
                    {
                        var wp = figure.transform.TransformPoint(new(point.x, point.y, 0));
                        if (
                            !(wp.x > gameField.FieldRoot.position.x - gameField.Size.x / 2)
                            || !(wp.x < gameField.FieldRoot.position.x + gameField.Size.x / 2)
                            || !(wp.y > gameField.FieldRoot.position.y - gameField.Size.y / 2)
                            || !(wp.y < gameField.FieldRoot.position.y + gameField.Size.y / 2)
                        )
                        {
                            putToGrid = false;
                            break;
                        }

                        var rotatedPoint = figure.transform.rotation * (Vector2)point;
                        var p = new Vector2Int(
                            Mathf.RoundToInt(rotatedPoint.x),
                            Mathf.RoundToInt(rotatedPoint.y)) + position;
                            
                        putToGrid = gameField.ItemInPosition(p) == 0 || gameField.ItemInPosition(p) == figure.Index;
                        if (!putToGrid)
                        {
                            break;
                        }
                    }

                  
                    if (putToGrid)
                    {
                        figure.Rigidbody2D.bodyType = RigidbodyType2D.Static;
                        a.InGrids.TryAddOrGet(e).Position = position;
                        figure.transform.position = gameField.CenterPositionFor(position);

                        foreach (var point in figure.Points)
                        {
                            var rotatedPoint = figure.transform.rotation * (Vector2)point;
                            gameField.SetTaken(figure.Index, new Vector2Int(Mathf.RoundToInt(rotatedPoint.x), Mathf.RoundToInt(rotatedPoint.y)) + position);
                        }

                        a.CheckFields.NewEntity();
                    }
                    else
                    {
                        figure.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    }

                    a.Draggables.Del(e);
                }
            }
        }
    }
}