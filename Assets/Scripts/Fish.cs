using UnityEngine;

public class Fish : MonoBehaviour
{
    private SpriteRenderer _renderer;
    public void Initialize(FishData fishData, DirectionX directionX)
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sprite = fishData.sprite;
        if (directionX == DirectionX.Left)
        {
            _renderer.flipX = true;
        }
    }

    public void FlipSprite()
    {
        _renderer.flipX = !_renderer.flipX;
    }
}
