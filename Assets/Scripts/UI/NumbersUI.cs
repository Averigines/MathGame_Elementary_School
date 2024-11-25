using TMPro;
using UnityEngine;

public class NumbersUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI current;
    [SerializeField] private TextMeshProUGUI goal;
    private readonly string _currentPreFix = "Current Number: ";
    private readonly string _goalPreFix = "Goal Number: ";
    
    private void Start()
    {
        ChangeCurrentNumber(0);
        ChangeGoalNumber(0);
    }

    public void ChangeCurrentNumber(int num)
    {
        current.text = _currentPreFix + num;
    }

    public void ChangeGoalNumber(int num)
    {
        goal.text = _goalPreFix + num;
    }
}
