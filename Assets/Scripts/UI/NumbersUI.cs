using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumbersUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI current;
    [SerializeField] private TextMeshProUGUI goal;
    private readonly string _goalPreFix = "Goal: ";
    
    private void Start()
    {
        ResetCalculationPath();
        ChangeGoalNumber(0);
    }
    
    public void ResetCalculationPath()
    {
        current.text = "0";
    }

    public void ChangeCalculationPath(string calc)
    {
        current.text = calc;
    }

    public void ChangeGoalNumber(int num)
    {
        goal.text = _goalPreFix + num;
    }
}
