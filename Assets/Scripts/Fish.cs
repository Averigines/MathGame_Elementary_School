using UnityEngine;

public class Fish : MonoBehaviour
{
    private SpriteRenderer _renderer;
    public void Initialize(DirectionX directionX)
    {
        _renderer = GetComponent<SpriteRenderer>();
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
