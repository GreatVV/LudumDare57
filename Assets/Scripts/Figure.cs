using System;
using System.Collections.Generic;
using DCFApixels.DragonECS;
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
        Destroy(gameObject);
    }
}