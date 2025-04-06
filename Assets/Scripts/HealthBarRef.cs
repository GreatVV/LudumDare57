using DCFApixels.DragonECS;

internal struct HealthBarRef : IEcsComponent
{
    public HealthBarView View;
    public float PrevValue;
    public float LastChangeTime;
}