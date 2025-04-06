using System;
using System.Collections.Generic;
using UnityEngine;

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
    public List<Stage> Stages = new List<Stage>();

    [Serializable]
    public class ShipInfo
    {
        public Ship Ship;
        public float Time;
        public float Speed;
        public float Health;
    }
    
    public ShipInfo[] Ships;
}