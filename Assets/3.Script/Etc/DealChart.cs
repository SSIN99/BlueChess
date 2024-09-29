using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealChart : MonoBehaviour
{
    [SerializeField] private Info info;
    [SerializeField] private Image icon;
    [SerializeField] private Image grade;
    [SerializeField] private Sprite[] gradeImage;
    [SerializeField] private Text dealAmount;
    [SerializeField] private Slider dealSlider;
    private Unit unit;
    public void SetChart(Unit target)
    {
        unit = target;
        icon.sprite = info.portraits[unit.No];
        grade.sprite = gradeImage[unit.Grade - 1];
        dealAmount.text = "0";
        dealSlider.maxValue = 0f;
    }
    public void UpdateChart(float max)
    {
        dealSlider.maxValue = max;
        dealSlider.value = unit.AllDealAmount;
        dealAmount.text = $"{unit.AllDealAmount}";
    }
}
