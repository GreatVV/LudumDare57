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
    
    public List<Stage> Stages = new List<Stage>();
}