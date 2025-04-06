using System;
using System.Collections.Generic;
using DCFApixels.DragonECS;

[Serializable]
public class RuntimeData
{
    public float LastSpawnTime;
    public Dictionary<int, entlong> IndexToFigure = new();
    public LevelTarget LevelTarget;
    public int CurrentStage;

    public float MonsterAttack = 1;
    
    public List<entlong> ActiveShips = new();
} 