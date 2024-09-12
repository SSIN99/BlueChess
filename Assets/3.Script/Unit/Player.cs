using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
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
    [SerializeField] private RoundManager round;
    [SerializeField] private Tile[] bench;
    private List<UnitArrange> unitList;
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
        fieldText.color = new Color(0.35f, 0.75f, 1);
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
    public void PurchaseUnit(int n, GameObject unit)
    {
        UnitArrange unitArrange = unit.GetComponent<UnitArrange>();
        UnitControl unitInfo = unit.GetComponent<UnitControl>();

        Gold -= unitInfo.Cost;
        numOfBench++;
        unitList.Add(unitArrange);
        unit.SetActive(true);
        for (int i = 0; i < bench.Length; i++)
        {
            if (bench[i].unit == null)
            {
                unitArrange.SetTile(bench[i]);
                break;
            }
        }
    }
    public void SellUnit(GameObject unit)
    {
        UnitArrange unitArrange = unit.GetComponent<UnitArrange>();
        UnitControl unitInfo = unit.GetComponent<UnitControl>();

        Gold += unitInfo.Cost;
        for (int i = unitList.Count - 1; i >= 0; i--)
        {
            if (unitArrange.Equals(unitList[i]))
            {
                unitList.Remove(unitArrange);
                unitArrange.BeSold();
                info.unitPool[unitInfo.No].Enqueue(unit);
                unit.SetActive(false);
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
        fieldText.color = isFullField ? Color.red : new Color(0.35f, 0.75f, 1);
    }
    private void SetArrangeFieldUnit()
    {
        MonoBehaviour arrangeControl;
        if (round.IsBattleStep)
        {
            for (int i = 0; i < unitList.Count; i++)
            {
                if (unitList[i].isOnField)
                {
                    arrangeControl = unitList[i];
                    arrangeControl.enabled = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < unitList.Count; i++)
            {
                if (unitList[i].isOnField)
                {
                    arrangeControl = unitList[i];
                    arrangeControl.enabled = true;
                }
            }
        }
    }
    #endregion
    private void OnEnable()
    {
        round.OnStepChange += SetArrangeFieldUnit;
    }
    private void Start()
    {
        Level = 1;
        Gold = 999;
        curExp = 0;
        maxExp = maxExpList[0];
        unitList = new List<UnitArrange>();
    }
    private void OnDisable()
    {
        round.OnStepChange -= SetArrangeFieldUnit;
    }
}
