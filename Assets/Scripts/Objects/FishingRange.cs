using System;
using UnityEngine;

public class FishingRange : MonoBehaviour
{
    [SerializeField] private Color colorInactive;
    [SerializeField] private Color colorActive;

    private SpriteRenderer _renderer;
    
    public bool CanBeReeledIn { get; private set; }

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = colorInactive;
        CanBeReeledIn = false;
    }

    public void SetToActive()
    {
        _renderer.color = colorActive;
        CanBeReeledIn = true;
    }

    public void SetToInactive()
    {
        _renderer.color = colorInactive;
        CanBeReeledIn = false;
    }
}
