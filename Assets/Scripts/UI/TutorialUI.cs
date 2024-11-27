using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private RectTransform tutorialBackgroundRT;
    private TextMeshProUGUI tutorialText;
    [SerializeField] private int offSetTextY;

    private void Start()
    {
        tutorialText = GetComponentInChildren<TextMeshProUGUI>();

        tutorialBackgroundRT.sizeDelta =
            new Vector2(tutorialBackgroundRT.sizeDelta.x, tutorialText.rectTransform.sizeDelta.y + offSetTextY * 2);
    }

    private void Update()
    {
        tutorialBackgroundRT.sizeDelta =
            new Vector2(tutorialBackgroundRT.sizeDelta.x, tutorialText.rectTransform.sizeDelta.y + offSetTextY * 2);
    }
}
