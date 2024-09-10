using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitManager : MonoBehaviour
{
    #region UI변수
    [SerializeField] private Text levelText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text expText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TMP_Text fieldText;
    #endregion

    #region Info변수
    private int level;
    private int gold;
    private int curExp;
    private int maxExp;
    private int[] maxExpList = { 2, 4, 6, 10, 20, 36, 48, 76 }; 
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
    #endregion

    #region Unit변수
    [SerializeField] private Info info;
    [SerializeField] private ShopManager shop;
    [SerializeField] private Tile[] bench;
    private List<Unit> unitList;
    private int numOfBench = 0;
    private int numOfField = 0;
    public bool isFullField => numOfField >= level;
    public bool isFullBench => numOfBench >= 8;
    #endregion

    #region  Info메소드
    private void LevelUp()
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
        fieldText.text = $"{numOfField} / {level}";
    }
    public void PurchaseExp()
    {
        if (level.Equals(9)) return;

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
    #endregion

    #region Unit메소드
    public void RecruitUnit()
    {
        if (Gold < 2) return;
        Gold -= 2;
        shop.SetShopItem();
    }
    public void PurchaseUnit(Unit unit)
    {
        Gold -= unit.Cost;
        numOfBench++;
        unitList.Add(unit);
        unit.gameObject.SetActive(true);
        for (int i = 0; i < bench.Length; i++)
        {
            if (bench[i].unit == null)
            {
                unit.SetTile(bench[i]);
                unit.InitInfo(info.unitData[unit.No]);
                break;
            }
        }
    }
    public void SellUnit(Unit unit)
    {
        Gold += unit.Cost;
        for (int i = unitList.Count - 1; i >= 0; i--)
        {
            if (unit.Equals(unitList[i]))
            {
                unitList.Remove(unit);
                unit.BeSold();
                info.unitPool[unit.No].Enqueue(unit.gameObject);
                unit.gameObject.SetActive(false);
                break;
            }
        }
    }
    public void CheckUnitList()
    {
        numOfBench = 0;
        numOfField = 0;
        for (int i = 0; i < unitList.Count; i++)
        {
            if (unitList[i].isOnField)
            {
                numOfField++;
            }
            else
            {
                numOfBench++;
            }
        }
        fieldText.text = $"{numOfField} / {level}";
        if (numOfField.Equals(level))
        {
            fieldText.color = Color.red;
        }
        else
        {
            fieldText.color = new Color(0.35f, 0.75f, 1);
        }
    }
    #endregion

    private void Start()
    {
        Level = 1;
        Gold = 999;
        curExp = 0;
        maxExp = maxExpList[0];
        unitList = new List<Unit>();
    }
}
