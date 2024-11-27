using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    private TextMeshProUGUI textResult;
    [SerializeField] private float fadeDuration;
    [SerializeField] private Color highlightColorNew;
    [SerializeField] private Color highlightColorCorrect;
    [SerializeField] private Color highlightColorIncorrect;

    private struct HighlightColorHexCodes
    {
        public string hexCodeNew;
        public string hexCodeCorrect;
        public string hexCodeIncorrect;
    }
    private HighlightColorHexCodes _highlightColors;
    
    private void Start()
    {
        textResult = GetComponent<TextMeshProUGUI>();
        var color = textResult.color;
        color.a = 0;
        textResult.color = color;
        
        _highlightColors.hexCodeNew = ColorUtility.ToHtmlStringRGB(highlightColorNew);
        _highlightColors.hexCodeCorrect = ColorUtility.ToHtmlStringRGB(highlightColorCorrect);
        _highlightColors.hexCodeIncorrect = ColorUtility.ToHtmlStringRGB(highlightColorIncorrect);

        textResult.enabled = false;
    }

    public IEnumerator ShowResult(string result, int lastCatch)
    {
        textResult.enabled = true;
        
        int firstIndexToColor = result.Length > 2 ? result.Length - 3 : result.Length - 1;
        if (lastCatch > 9 || result.Length == 2) firstIndexToColor--;
        int lastIndexToColor = result.Length - 1;

        string textToShow = InsertColorTags(result, firstIndexToColor, lastIndexToColor, _highlightColors.hexCodeNew);
        
        textResult.text = textToShow;

        yield return textResult.DOFade(1, fadeDuration).WaitForCompletion();
    }
    
    public IEnumerator ShowResult(string result, bool correctResult)
    {
        textResult.enabled = true;

        int firstIndexToColor = 0;
        int lastIndexToColor = result.Length - 1;

        string textToShow = null;
        if (correctResult)
        {
            textToShow = InsertColorTags(result, firstIndexToColor, lastIndexToColor, _highlightColors.hexCodeCorrect);
        }
        else
        {
            textToShow = InsertColorTags(result, firstIndexToColor, lastIndexToColor, _highlightColors.hexCodeIncorrect);
        }

        textResult.text = textToShow;

        yield return textResult.DOFade(1, fadeDuration).WaitForCompletion();
    }

    public IEnumerator HideResult()
    {
        yield return textResult.DOFade(0, fadeDuration).WaitForCompletion();
        textResult.enabled = false;
    }
    
    private string InsertColorTags(string text, int startIndex, int endIndex, string colorHexCode)
    {
        string openTag = $"<color=#{colorHexCode}>";
        string closeTag = "</color>";

        int subStringLength = endIndex - startIndex + 1;
        
        string before = text.Substring(0, startIndex);
        string target = text.Substring(startIndex, subStringLength);

        return before + openTag + target + closeTag;
    }

    public void ResetForNewLevel()
    {
        var color = textResult.color;
        color.a = 0;
        textResult.color = color;
        textResult.enabled = false;
    }
}
