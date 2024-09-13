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
    private List<GameObject> benchList;
    private List<GameObject> fieldList;
    [SerializeField] private int numOfBench = 0;
    [SerializeField] private int numOfField = 0;
    public int NumOfField
    {
        get { return numOfField; }
        private set
        {
            numOfField = value;
            fieldText.text = $"{numOfField} / {level}";
            fieldText.color = isFullField ? Color.red : new Color(0.35f, 0.75f, 1);
        }
    }
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
        UnitArrange arrange = unit.GetComponent<UnitArrange>();
        UnitControl unitInfo = unit.GetComponent<UnitControl>();

        Gold -= unitInfo.Cost;
        AddBench(unit);
        for (int i = 0; i < bench.Length; i++)
        {
            if (bench[i].unit == null)
            {
                arrange.InitTile(bench[i]);
                unit.transform.parent = transform;
                unit.SetActive(true);
                break;
            }
        }
    }
    public void SellUnit(GameObject unit)
    {
        UnitArrange arrange = unit.GetComponent<UnitArrange>();
        UnitControl unitInfo = unit.GetComponent<UnitControl>();

        Gold += unitInfo.Cost;
        if (arrange.isOnField)
        {
            RemoveField(unit);
        }
        else
        {
            RemoveBench(unit);
        }
        arrange.BeSold();
        info.unitPool[unitInfo.No].Enqueue(unit);
        unit.transform.parent = info.transform;
        unit.SetActive(false);
    }
    private void SetFieldUnitState()
    {
        if (round.IsBattleStep)
        {
            for (int i = 0; i < fieldList.Count; i++)
            {
                fieldList[i].GetComponent<UnitArrange>().enabled = false;
                fieldList[i].GetComponentInChildren<Animator>().SetTrigger("Search");
            }
        }
        else
        {
            for (int i = 0; i < fieldList.Count; i++)
            {
                fieldList[i].GetComponent<UnitArrange>().enabled = true;
                fieldList[i].GetComponent<UnitArrange>().ReturnTile();
            }
        }
    }
    public void AddBench(GameObject unit)
    {
        benchList.Add(unit);
        numOfBench++;
    }
    public void RemoveBench(GameObject unit)
    {
        benchList.Remove(unit);
        numOfBench--;
    }
    public void AddField(GameObject unit)
    {
        fieldList.Add(unit);
        NumOfField++;
    }
    public void RemoveField(GameObject unit)
    {
        fieldList.Remove(unit);
        NumOfField--;
    }
    #endregion
    private void OnEnable()
    {
        round.OnStepChange += SetFieldUnitState;
    }
    private void Start()
    {
        Level = 1;
        Gold = 999;
        curExp = 0;
        maxExp = maxExpList[0];
        benchList = new List<GameObject>();
        fieldList = new List<GameObject>();
    }
    private void OnDisable()
    {
        round.OnStepChange -= SetFieldUnitState;
    }
}
