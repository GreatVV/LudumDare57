using DCFApixels.DragonECS;
using UnityEngine;

internal class ShipMoveSystem : IEcsRun
{
    [DI] private EcsDefaultWorld _world;
    [DI] private SceneData _sceneData;
    [DI] private RuntimeData _runtimeData;
    class Aspect : EcsAspect
    {
        public EcsPool<ShipRef> ShipRefs = Inc;
        public EcsPool<Speed> Speeds = Inc;
        public EcsPool<Lost> Lost = Exc;
    }
    
    public void Run()
    {
        foreach (var e in _world.Where(out Aspect a))
        {
            var normalized = (_sceneData.EndShipPosition.position - _sceneData.SpawnShipPosition.position).normalized;
            var shipTransform = a.ShipRefs.Get(e).View.transform;
            var transformPosition = shipTransform.position;
            transformPosition += normalized * a.Speeds.Get(e).Value * Time.deltaTime;
            if (Vector3.Distance(transformPosition, _sceneData.SpawnShipPosition.position) > Vector3.Distance(_sceneData.EndShipPosition.position, _sceneData.SpawnShipPosition.position))
            {
                a.Lost.TryAddOrGet(e);
                _runtimeData.LostShips++;
                if (_runtimeData.LostShips + _runtimeData.KilledShip >= _runtimeData.TargetToKill)
                {
                    PlayEndLevelSequence(_runtimeData, _sceneData);
                }
            }
            shipTransform.position = transformPosition;
        }
    }

    public static async void PlayEndLevelSequence(RuntimeData _runtimeData, SceneData _sceneData)
    {
        if (_runtimeData.KilledShip > _runtimeData.LostShips)
        {
            ProfileService.Instance.CurrentLevel++;
            _sceneData.Win.SetActive(true);
        }
        else
        {
            _sceneData.Lose.SetActive(true);
        }

        await Awaitable.WaitForSecondsAsync(_sceneData.WinDelay);
        LoadingScreen.Instance.FadeIn();
    }
}