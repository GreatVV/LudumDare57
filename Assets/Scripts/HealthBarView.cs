using UnityEngine;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [SerializeField] public SpriteRenderer AdditionalSpriteRenderer;

    public float Speed = 0.2f;
    public float WaitTime = 1f;

    public void Set(in Health health)
    {
        Set(health.Current, health.Max);
    }
        
    public void Set(float current, float max)
    {
        SpriteRenderer.size = new(Mathf.Max(current / max, 0), 1);
    }
}