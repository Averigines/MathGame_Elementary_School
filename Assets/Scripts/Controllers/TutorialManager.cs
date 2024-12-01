using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialContainer;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip[] videosInOrder;
    [SerializeField] private string[] textInOrder;
    
    [SerializeField] private Button buttonLeft;
    [SerializeField] private Button buttonRight;

    [SerializeField] private GameObject toggleContainer;
    [SerializeField] private GameObject buttonsContainer;

    [SerializeField] private GameObject videoContainer;

    private int _currentVideoIndex = 0;
    private Tuple<VideoClip, string>[] _videosWithStrings;

    private void Start()
    {
        tutorialContainer.SetActive(false);
        _videosWithStrings = new Tuple<VideoClip, string>[videosInOrder.Length];

        for (int i = 0; i < videosInOrder.Length; i++)
        {
            _videosWithStrings[i] = new Tuple<VideoClip, string>(videosInOrder[i], textInOrder[i]);
        }

        RectTransform tf = videoContainer.GetComponent<RectTransform>();
        float newHeight = tf.rect.width * 1.4f;
        tf.rect.Set(tf.rect.x, tf.rect.y, tf.rect.width, newHeight);
    }

    public void CloseTutorial()
    {
        videoPlayer.Stop();
        tutorialContainer.SetActive(false);
        toggleContainer.SetActive(true);
        buttonsContainer.SetActive(true);
        _currentVideoIndex = 0;
    }

    public void OpenTutorial()
    {
        _currentVideoIndex = 0;
        
        tutorialContainer.SetActive(true);
        toggleContainer.SetActive(false);
        buttonsContainer.SetActive(false);

        buttonLeft.enabled = false;
        buttonRight.enabled = true;
        
        tutorialText.text = _videosWithStrings[_currentVideoIndex].Item2;
        videoPlayer.clip = _videosWithStrings[_currentVideoIndex].Item1;
        
        videoPlayer.Play();
    }

    public void ShowPreviousTutorial()
    {
        if (_currentVideoIndex == _videosWithStrings.Length - 1)
        {
            buttonRight.enabled = true;
        }
        
        _currentVideoIndex--;
        
        if (_currentVideoIndex == 0)
        {
            buttonLeft.enabled = false;
        }
        
        tutorialText.text = _videosWithStrings[_currentVideoIndex].Item2;
        videoPlayer.clip = _videosWithStrings[_currentVideoIndex].Item1;
        
        videoPlayer.Play();
    }
    
    public void ShowNextTutorial()
    {
        if (_currentVideoIndex == 0)
        {
            buttonLeft.enabled = true;
        }
        
        _currentVideoIndex++;
        
        if (_currentVideoIndex >= _videosWithStrings.Length - 1)
        {
            buttonRight.enabled = false;
        }
        
        tutorialText.text = _videosWithStrings[_currentVideoIndex].Item2;
        videoPlayer.clip = _videosWithStrings[_currentVideoIndex].Item1;
        
        videoPlayer.Play();
    }
    
}
