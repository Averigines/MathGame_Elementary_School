using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FishCatchUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textResult;
    [SerializeField] private float fadeDuration;
    [SerializeField] private Color highlightColor;
    private string highlightColorHexCode;

    private void Start()
    {
        var color = textResult.color;
        color.a = 0;
        textResult.color = color;
        
        highlightColorHexCode = ColorUtility.ToHtmlStringRGB(highlightColor);

        textResult.enabled = false;
    }

    public IEnumerator ShowResult(string result, int lastCatch)
    {
        textResult.enabled = true;
        
        int firstIndexToColor = result.Length > 2 ? result.Length - 3 : result.Length - 1;
        if (lastCatch > 9 || result.Length == 2) firstIndexToColor--;
        int lastIndexToColor = result.Length - 1;

        string textToShow = InsertColorTags(result, firstIndexToColor, lastIndexToColor, highlightColorHexCode);
        
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
