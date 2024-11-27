using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumbersUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI current;
    [SerializeField] private TextMeshProUGUI goal;
    private readonly string _goalPreFix = "Goal: ";

    private struct SubmitButtons
    {
        public Sprite defaultBtn;
        public Sprite correctBtn;
        public Sprite incorrectBtn;
    }
    
    [SerializeField] private Image submitBtnImage;
    [SerializeField] private Sprite submitBtnDefault;
    [SerializeField] private Sprite submitBtnCorrect;
    [SerializeField] private Sprite submitBtnIncorrect;
    private SubmitButtons _submitButtons;
    
    private void Awake()
    {
        _submitButtons.defaultBtn = submitBtnDefault;
        _submitButtons.correctBtn = submitBtnCorrect;
        _submitButtons.incorrectBtn = submitBtnIncorrect;
    }
    
    private void ResetCalculationPath()
    {
        current.text = "0";
    }

    public void ChangeCalculationPath(string calc)
    {
        current.text = calc;
    }

    private void ChangeGoalNumber(int num)
    {
        goal.text = _goalPreFix + num;
    }

    public void ChangeSubmitButton(bool correct)
    {
        if (correct) submitBtnImage.sprite = _submitButtons.correctBtn;
        else submitBtnImage.sprite = _submitButtons.incorrectBtn;
    }

    private void ResetSubmitButton()
    {
        submitBtnImage.sprite = _submitButtons.defaultBtn;
    }

    public void ResetForNewLevel(int goalNumber)
    {
        ResetCalculationPath();
        ChangeGoalNumber(goalNumber);
        ResetSubmitButton();
    }
}
