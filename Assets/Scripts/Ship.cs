using DCFApixels.DragonECS;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public Transform AttackTarget;
    public entlong Entity;
    public Transform HealthBarRoot;

    public ParticleSystem DeathParticles;
    public float DelayBeforeDeath = 1f;

    public async void PlayDeath()
    {
        if (DeathParticles) DeathParticles.Play();
        await Awaitable.WaitForSecondsAsync(DelayBeforeDeath);
        Destroy(gameObject);
    }
}