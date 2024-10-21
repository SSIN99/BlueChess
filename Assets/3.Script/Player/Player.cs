using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Linq;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private Text levelText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text expText;
    [SerializeField] private Slider expSlider;

    private int level;
    private int gold;
    private int curExp;
    private int maxExp;
    private int[] maxExpList = { 2, 4, 6, 10, 20, 36, 48, 76 };

    public event Action OnLevelChanged;
    public event Action OnGoldChanged;

    private void Start()
    {
        LevelUp();
        Gold = 99;
        curExp = 0;
        maxExp = maxExpList[0];
    }
    public int Level
    {
        get { return level; }
        private set
        {
            level = value;
            OnLevelChanged?.Invoke();
        }
    }
    public void GetExp(int exp)
    {
        if (level == 9) return;
        curExp += exp;
        if (curExp >= maxExp)
        {
            curExp -= maxExp;
            LevelUp();
        }
        else
        {
            expSlider.value = curExp;
            expText.text = $"{curExp}/{maxExp}";
        }
    }
    private void LevelUp()
    {
        Level += 1;
        levelText.text = $"Lv.{level}";

        if (level == 9)
        {
            expSlider.value = expSlider.maxValue;
            expText.text = "Max";
        }
        else
        {
            maxExp = maxExpList[level - 1];
            expSlider.maxValue = maxExp;
            expSlider.value = curExp;
            expText.text = $"{curExp}/{maxExp}";
        }
    }
    public int Gold
    {
        get { return gold; }
        private set
        {
            gold = value;
            OnGoldChanged?.Invoke();
        }
    }
    public void UpdateGold(int gold)
    {
        Gold += gold;
        goldText.text = $"{Gold}";
    }

}
