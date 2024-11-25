using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    private RawImage _fadeImage;
    [SerializeField] private float _fadeDuration = 1.0f;

    private void Start()
    {
        _fadeImage = GetComponent<RawImage>();
    }
    
    public IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;
        Color color = _fadeImage.color;
        color.a = 0f;
        _fadeImage.color = color;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / _fadeDuration);
            _fadeImage.color = color;
            yield return null;
        }
    }

    public IEnumerator FadeFromBlack()
    {
        float elapsedTime = 0f;
        Color color = _fadeImage.color;
        color.a = 1f;
        _fadeImage.color = color;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1 - Mathf.Clamp01(elapsedTime / _fadeDuration);
            _fadeImage.color = color;
            yield return null;
        }
    }
}
