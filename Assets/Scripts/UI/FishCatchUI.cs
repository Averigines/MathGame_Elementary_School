using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FishCatchUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textValue;
    [SerializeField] private TextMeshProUGUI textResult;
    [SerializeField] private float fadeDuration;

    private void Start()
    {
        textValue.enabled = false;
        textResult.enabled = false;
    }

    public IEnumerator ShowValue(string value)
    {
        textValue.enabled = true;
        textValue.text = value;
        Color color = textValue.color;
        color.a = 0f;
        textValue.color = color;

        yield return textValue.DOFade(1, fadeDuration).WaitForCompletion();
    }

    public IEnumerator HideValue(string value)
    {
        textValue.text = value;
        Color color = textValue.color;
        color.a = 1f;
        textValue.color = color;

        yield return textValue.DOFade(0, fadeDuration).WaitForCompletion();
        textValue.enabled = false;
    }

    public IEnumerator ShowResult(string result)
    {
        textResult.enabled = true;
        textResult.text = result;
        Color color = textResult.color;
        color.a = 0f;
        textResult.color = color;

        yield return textResult.DOFade(1, fadeDuration).WaitForCompletion();
    }

    public IEnumerator HideResult(string result)
    {
        textResult.text = result;
        Color color = textResult.color;
        color.a = 1f;
        textResult.color = color;

        yield return textResult.DOFade(0, fadeDuration).WaitForCompletion();
        textResult.enabled = false;
    }
}
