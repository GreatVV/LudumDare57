using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu]
public class LevelTarget : ScriptableObject
{
    [Serializable]
    public class Stage
    {
        public List<Pair> Pairs = new List<Pair>();
    }
    
    [Serializable]
    public class Pair
    {
        public Vector2Int Position;
        public Figure Figure;
    }
    
    public Figure[] Figures;
    [SerializeField]
    private List<Stage> Stages = new List<Stage>();

    public int TentacleNumber;

    [Serializable]
    public class ShipInfo
    {
        public Ship Ship;
        public float Time;
        public float Speed;
        public float Health;
    }
    
    [SerializeField]
    private ShipInfo[] Ships;

    public bool GeneratedLevel;
    public int MinNumberOfShips;
    public int MaxNumberOfShips;
    public float DelayBetweenShips = 1f;
    public float MinSpeed;
    public float MaxSpeed;
    public Ship[] PossibleShips;
    public Figure[] TargetFigures;

    public Stage GetStage(int index)
    {
        if (GeneratedLevel)
        {
            var stage = new Stage();
            stage.Pairs = new();
            var targetFigure = TargetFigures[Random.Range(0, TargetFigures.Length)];
            stage.Pairs.Add(new()
            {
                Position = new Vector2Int(Random.Range(1, 7), Random.Range(1, 7)),
                Figure = targetFigure
            });
            return stage;
        }
        
        return Stages[index % Stages.Count ];
    }

    public ShipInfo[] GetShips()
    {
        if (GeneratedLevel)
        {
            var shipsInfo = new ShipInfo[Random.Range(MinNumberOfShips, MaxNumberOfShips)];
            for (int i = 0; i < shipsInfo.Length; i++)
            {
                shipsInfo[i] = new ShipInfo()
                {
                    Ship = PossibleShips[Random.Range(0, Ships.Length)],
                    Health = Random.Range(1, 3),
                    Speed = Random.Range(MinSpeed, MaxSpeed),
                    Time = i * DelayBetweenShips
                };
            }

            return shipsInfo;
        }

        return Ships;
    }
}