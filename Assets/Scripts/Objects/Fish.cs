using UnityEngine;

public class Fish : MonoBehaviour
{
    private SpriteRenderer _renderer;
    public void Initialize(float size, DirectionX directionX)
    {
        _renderer = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(size, size, transform.localScale.z);
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
