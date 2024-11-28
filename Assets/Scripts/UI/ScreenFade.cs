using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    private RawImage _fadeImage;
    [SerializeField] private float _fadeDuration = 1.0f;
    
    private static bool _isFirstScene = true; 

    private void Start()
    {
        _fadeImage = GetComponent<RawImage>();

        if (_isFirstScene)
        {
            _fadeImage.enabled = false;
            _isFirstScene = false;
        }
        else
        {
            StartCoroutine(FadeFromBlack());
        }
    }
    
    public IEnumerator FadeToBlack()
    {
        _fadeImage.enabled = true;
        Color color = _fadeImage.color;
        color.a = 0f;
        _fadeImage.color = color;

        yield return _fadeImage.DOFade(1, _fadeDuration).WaitForCompletion();

    }

    public IEnumerator FadeFromBlack()
    {
        Color color = _fadeImage.color;
        color.a = 1f;
        _fadeImage.color = color;

        yield return _fadeImage.DOFade(0, _fadeDuration).WaitForCompletion();
        _fadeImage.enabled = false;
    }
}
