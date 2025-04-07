using System;
using System.Collections.Generic;
using DCFApixels.DragonECS;
using PrimeTween;
using TriInspector;
using UnityEngine;

public class Figure : MonoBehaviour
{
    public int Index;
    public Vector2Int[] Points;
    public Sprite Sprite;

    public List<SpriteRenderer> Sprites = new();
    public Rigidbody2D Rigidbody2D;
    public entlong Entity;
    
    public float DelayBeforeDeath = 2f;
    public float FadeTime = 1f;
    public Ease FadeEase;

    public bool Pulse;
    public float PulseTime;
    public Vector3 PulseSize;
    public float PulseFrequency = 1f;

    private void OnValidate()
    {
        if (!Rigidbody2D)
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        if (!Rigidbody2D)
        {
            Rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    private void Start()
    {
        if (Pulse)
        {
            foreach (var sprite in Sprites)
            {
                Tween.PunchScale(sprite.transform, PulseSize, PulseTime, PulseFrequency, cycles: -1);
            }
           
        }
    }


    [Button]
    public void Generate()
    {
        foreach (var sprite in Sprites)
        {
            if (!sprite)
            {
                continue;
            }
            
            if (Application.isPlaying)
            {
                Destroy(sprite.gameObject);
            }
            else
            {
                DestroyImmediate(sprite.gameObject, true);
            }
        }
        Sprites.Clear();
        
        foreach (var point in Points)
        {
            var instance = new GameObject(point.ToString());
            instance.transform.SetParent(transform);
            instance.transform.localPosition = new(point.x, point.y, 0);
            var spriteRenderer = instance.AddComponent<SpriteRenderer>();
            spriteRenderer.drawMode = SpriteDrawMode.Sliced;
            spriteRenderer.size = new(1, 1);
            spriteRenderer.transform.localScale = new(1, 1, 1);
            spriteRenderer.sprite = Sprite;
            instance.AddComponent<BoxCollider2D>();
            Sprites.Add(spriteRenderer);
        }
    }

    public void Kill()
    {
        foreach (var sprite in Sprites)
        {
            Tween.Alpha(sprite, 0, FadeTime, FadeEase);
            var collider = sprite.GetComponent<Collider2D>();
            if (collider)
            {
                collider.enabled = false;
            }
        }
        Destroy(gameObject, DelayBeforeDeath);
    }
}