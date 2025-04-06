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
                    draggable.StartRotation = Mathf.Round(figure.transform.localEulerAngles.z / 90f) * 90f; ;

                    _world.GetPool<FigureRef>().Get(e).View.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
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

                    var figureRef = a.Figures.Get(e);
                    var figureRefView = figureRef.View;
                    figureRefView.transform.position = draggable.StartPosition + diff;
                    figureRefView.transform.localEulerAngles = Vector3.MoveTowards(
                        figureRefView.transform.localEulerAngles,
                        new Vector3(0, 0, draggable.StartRotation)
                        , Time.deltaTime * _staticData.FigureRotationSpeed);
                }
            }
            else
            {
                foreach (var e in _world.Where(out Aspect a))
                {
                    a.Figures.Get(e).View.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    a.Draggables.Del(e);
                }
            }
        }
    }
}