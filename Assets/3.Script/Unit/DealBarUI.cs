using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealBarUI : MonoBehaviour
{
    [SerializeField] private Info info;
    [SerializeField] private Image portrait;
    [SerializeField] private Image grade;
    [SerializeField] private Text amount;
    [SerializeField] private Slider dealSlider;
    private Unit unit;
    public void InitInfo(Unit target)
    {
        unit = target;
        portrait.sprite = info.portraits[unit.No];
        grade.sprite = info.gradeIcon[unit.Grade - 1];
        amount.text = "0";
        dealSlider.maxValue = 0f;
    }
    public void UpdateMaxDeal(float max)
    {
        dealSlider.maxValue = max;
        dealSlider.value = unit.dealAmount;
        amount.text = $"{unit.dealAmount}";
    }
}
