﻿using DCFApixels.DragonECS;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

internal class SpawnStageSystem : IEcsRun, IEcsInit
{
    [DI] private EcsDefaultWorld _world;
    [DI] private RuntimeData _runtimeData;
    [DI] private StaticData _staticData;
    [DI] private ProfileService _profileService;
    [DI] SceneData _sceneData;

    class Aspect : EcsAspect
    {
        public EcsPool<TentacleRef> TentacleRefs = Inc;
        public EcsPool<Attack> Attacks = Exc;
        public EcsPool<ReturnToStartPosition> ReturnToStartPosition = Exc;
        public EcsPool<MoveToMouth> MoveToMouth = Exc;
        public EcsPool<Delay> Delays = Exc;
    }
    
    public void Run()
    {
        foreach (var e in _world.Where(out SingleAspect<SpawnNextStage> s))
        {
            _runtimeData.CurrentStage++;

            _world.GetPool<SpawnStage>().NewEntity().Value = _runtimeData.LevelTarget.GetStage(_runtimeData.CurrentStage);;

            if (_runtimeData.ActiveShips.Count > 0)
            {
                foreach (var tentacleEntity in _world.Where(out Aspect a))
                {
                    a.Attacks.Add(tentacleEntity).Target =
                        _runtimeData.ActiveShips[Random.Range(0, _runtimeData.ActiveShips.Count)];
                }
            }

            s.pool.Del(e);
        }

        foreach (var e in _world.Where(out SingleAspect<SpawnStage> a))
        {
            var spawnState = a.pool.Get(e);

            var field = _sceneData.GameField;
            foreach (var valuePair in spawnState.Value.Pairs)
            {
                var instance = Object.Instantiate(valuePair.Figure, field.transform);
                instance.transform.localPosition = field.ToLocalPosition(valuePair.Position);
                instance.transform.localScale = new(
                    field.Size.x/field.FieldSize.x, 
                    field.Size.y/field.FieldSize.y, 
                    1);
                var entity = _world.NewEntity();
                _world.GetPool<FigureRef>().Add(entity).View = instance;
                _world.GetPool<Target>().Add(entity).Position = valuePair.Position;
                instance.Rigidbody2D.bodyType = RigidbodyType2D.Static;
                foreach (var s in instance.Sprites)
                {
                    s.GetComponent<Collider2D>().enabled = false;
                }
                instance.Entity = _world.GetEntityLong(entity);
            }

            a.pool.Del(e);
        }
    }

    public void Init()
    {
        _runtimeData.LevelTarget = _staticData.Levels[Mathf.Clamp(_profileService.CurrentLevel,0, _staticData.Levels.Length-1)];
        _runtimeData.CurrentStage = 0;
        _runtimeData.Figures = _runtimeData.LevelTarget.Figures;

        _world.GetPool<SpawnStage>().NewEntity().Value = _runtimeData.LevelTarget.GetStage(0);

        var tentaclesLength = Mathf.Min(_runtimeData.LevelTarget.TentacleNumber, _sceneData.Monster.Tentacles.Length);
        for (var index = 0; index < tentaclesLength; index++)
        {
            var e = _sceneData.Monster.Tentacles[index];
            var tentacleEntity = _world.NewEntity();
            ref var tentacleRef = ref _world.GetPool<TentacleRef>().Add(tentacleEntity);
            tentacleRef.SpriteShapeController = e;
            tentacleRef.SpriteShapeController.gameObject.SetActive(true);
            tentacleRef.StartPosition = e.spline.GetPosition(e.spline.GetPointCount() - 1);
            tentacleRef.MouthPosition = e.transform.InverseTransformPoint(_sceneData.Monster.MouthPosition.position);
            tentacleRef.AttackParticle = Object.Instantiate(_staticData.AttackParticle);

        }
    }
}