using DCFApixels.DragonECS;

internal struct SpawnShip : IEcsComponent
{
    public Ship Prefab;
    public float Speed;
    public float Health;
}