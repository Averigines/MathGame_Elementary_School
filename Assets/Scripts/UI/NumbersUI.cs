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

    [SerializeField] private Button submitBtn;
    private Image submitBtnImage;
    [SerializeField] private Sprite submitBtnDefault;
    [SerializeField] private Sprite submitBtnCorrect;
    [SerializeField] private Sprite submitBtnIncorrect;
    private SubmitButtons _submitButtons;
    
    private void Awake()
    {
        _submitButtons.defaultBtn = submitBtnDefault;
        _submitButtons.correctBtn = submitBtnCorrect;
        _submitButtons.incorrectBtn = submitBtnIncorrect;

        submitBtnImage = submitBtn.GetComponent<Image>();
    }
    
    private void ResetCalculationPath(int startNumber)
    {
        current.text = startNumber.ToString();
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

        submitBtn.enabled = false;
    }

    public void ResetSubmitButton()
    {
        submitBtn.enabled = true;
        
        submitBtnImage.sprite = _submitButtons.defaultBtn;
    }

    public void ResetForNewLevel(int startNumber, int goalNumber)
    {
        ResetCalculationPath(startNumber);
        ChangeGoalNumber(goalNumber);
        ResetSubmitButton();
    }
}
