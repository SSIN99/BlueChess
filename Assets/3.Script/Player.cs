using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Text levelText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text expText;
    [SerializeField] private Slider expSlider;

    [SerializeField] private ShopManager shop;
    [SerializeField] private Info info;
    [SerializeField] private Transform waitingSeat;

    private int level;
    private int gold;
    private int maxExp;
    private int curExp;
    private int[] maxExpList = { 2, 4, 6, 10, 20, 36, 48, 76 };

    private List<GameObject> unitList;
    public event Action OnLevelUp;
    public int Level
    {
        get { return level; }
        private set
        {
            level = value;
            LevelUp();
            OnLevelUp?.Invoke();
        }
    }
    public int Gold
    {
        get { return gold; }
        private set
        {
            gold = value;
            goldText.text = gold.ToString();
        }
    }
 
    private void Start()
    {
        Level = 1;
        curExp = 0;
        Gold = 1000;
        unitList = new List<GameObject>();
    }
    public void PayCost(int cost)
    {
        Gold -= cost;
    }
    public void LevelUp()
    {
        levelText.text = $"Lv.{level.ToString()}";
        if (level >= 9)
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
    public void PurchaseExp()
    {
        if(level.Equals(9))
        {
            Debug.Log("최대 레벨");
            return;
        }

        if (gold >= 4)
        {
            Gold -= 4;
            curExp += 4;
            if (curExp >= maxExp)
            {
                curExp -= maxExp;
                Level++;
            }
            else
            {
                expSlider.value = curExp;
                expText.text = $"{curExp}/{maxExp}";
            }
        }
    }
    public void PurchaseUnit(int n, GameObject unit, int cost)
    {
        Gold -= cost;
        unitList.Add(unit);
        unit.transform.parent = waitingSeat;
        unit.SetActive(true);
        unit.GetComponent<Unit>().InitData(info.unitData[n]);
    }
}
