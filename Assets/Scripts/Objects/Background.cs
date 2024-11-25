using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

public class Background : MonoBehaviour
{
    [SerializeField] private Sprite[] backgroundVariants;
    private SpriteRenderer _renderer;
    private Sprite _currBackground;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _currBackground = null;
    }

    public void ChangeBackground()
    {
        List<Sprite> backgrounds = new List<Sprite>();
        foreach (var bg in backgroundVariants)
        {
            if (bg == _currBackground) continue;
                
            backgrounds.Add(bg);
        }
        var backgroundIndex = Random.Range(0, backgrounds.Count - 1);
        _renderer.sprite = backgrounds[backgroundIndex];
        _currBackground = backgroundVariants[backgroundIndex];
    }
}
