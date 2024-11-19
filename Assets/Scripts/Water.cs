using System;
using UnityEngine;

public class Water : MonoBehaviour
{
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        MatchScreenSize();
    }

    private void MatchScreenSize()
    {
        
    }
}
