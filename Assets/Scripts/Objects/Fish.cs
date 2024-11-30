using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private List<GameObject> variants;
    public SpriteRenderer renderer;
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
        renderer = _activeVariant.GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(size, size, transform.localScale.z);
        if (directionX == DirectionX.Left)
        {
            renderer.flipX = true;
        }
    }

    public void FlipSprite()
    {
        renderer.flipX = !renderer.flipX;
    }
}
