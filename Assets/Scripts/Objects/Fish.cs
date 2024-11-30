using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Fish : MonoBehaviour
{
    [SerializeField] private List<GameObject> variants;
    [NonSerialized] public SpriteRenderer spriteRenderer;
    private GameObject _activeVariant;
    public void Initialize(float size, DirectionX directionX)
    {
        foreach (var variant in variants)
        {
            variant.SetActive(false);
        }
        int randomVariant = Random.Range(0, variants.Count);
        _activeVariant = variants[randomVariant];
        _activeVariant.SetActive(true);
        spriteRenderer = _activeVariant.GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(size, size, transform.localScale.z);
        if (directionX == DirectionX.Left)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void FlipSprite()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
