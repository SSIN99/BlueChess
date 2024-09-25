using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Player : MonoBehaviour
{
    #region UI
    [SerializeField] private Text levelText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text expText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TMP_Text fieldText;
    [SerializeField] private Image lockImage;
    [SerializeField] private Sprite[] lockSprite;
    #endregion

    #region Info
    private int level;
    private int gold;
    private int curExp;
    private int maxExp;
    private int[] maxExpList = { 2, 4, 6, 10, 20, 36, 48, 76 };
    private bool isLocked;
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

    #region Unit
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
    public void ToggleLock()
    {
        isLocked = !isLocked;
        lockImage.sprite = isLocked ? lockSprite[0] : lockSprite[1];
    }
    public void RecruitUnit()
    {
        if (Gold < 2) return;
        Gold -= 2;
        shop.SetShopItem();
    }
    public void PurchaseUnit(int n)
    {
        GameObject unit = info.unitPool[n].Dequeue();
        ArrangeControl arrange = unit.GetComponent<ArrangeControl>();
        UnitControl unitInfo = unit.GetComponent<UnitControl>();
        unit.SetActive(true);
        unitInfo.InitInfo(info.Units[n]);

        Gold -= unitInfo.Cost;
        AddBench(unit);
        for (int i = 0; i < bench.Length; i++)
        {
            if (bench[i].unit == null)
            {
                arrange.InitTile(bench[i]);
                unit.transform.parent = transform;
                break;
            }
        }
    }
    public void SellUnit(GameObject unit)
    {
        ArrangeControl arrange = unit.GetComponent<ArrangeControl>();
        UnitControl unitInfo = unit.GetComponent<UnitControl>();

        switch (unitInfo.Grade)
        {
            case 1:
                Gold += unitInfo.Cost;
                info.unitCount[unitInfo.No]++;
                break;
            case 2:
                Gold += (unitInfo.Cost * 3) - 1;
                info.unitCount[unitInfo.No] += 3;
                break;
            case 3:
                Gold += (unitInfo.Cost * 9) - 1;
                info.unitCount[unitInfo.No] += 9;
                break;
        }

        if (arrange.IsOnField)
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
        Debug.Log($"{unitInfo.No}번 유닛 판매, { info.unitCount[unitInfo.No]}개 잔여");
    }
    private void SetFieldUnitState()
    {
        if (round.IsBattleStep)
        {
            for (int i = 0; i < fieldList.Count; i++)
            {
                fieldList[i].GetComponent<UnitControl>().IsBattle = true;
            }
        }
        else
        {
            for (int i = 0; i < fieldList.Count; i++)
            {
                if(fieldList[i].activeSelf == false)
                {
                    fieldList[i].SetActive(true);
                }
                fieldList[i].GetComponent<UnitControl>().IsBattle = false;
            }
        }
    }
    public void AddBench(GameObject unit)
    {
        unit.layer = LayerMask.NameToLayer("Bench");
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
        unit.layer = LayerMask.NameToLayer("Field");
        fieldList.Add(unit);
        NumOfField++;

        Unit u = unit.GetComponent<Unit>();
        if (!onFieldUnit.ContainsKey(u.No))
        { //처음유닛 넣을때
            onFieldUnit.Add(u.No, 1);
            if (!traitList.ContainsKey(u.Origin))
            { //소속유닛이 처음일때
                traitList.Add(u.Origin, 1);
            }
            else
            { //소속유닛이 이미있을때
                traitList[u.Origin]++;
            }
            if (!traitList.ContainsKey(u.Class))
            { //직업유닛이 처음일때
                traitList.Add(u.Class, 1);
            }
            else
            { //직업유닛이 이미있을때
                traitList[u.Class]++;
            }
        }
        else
        { //중복유닛 넣을때
            onFieldUnit[u.No]++;
        }
        UpdateTraitBar();
    }
    public void RemoveField(GameObject unit)
    {
        fieldList.Remove(unit);
        NumOfField--;

        Unit u = unit.GetComponent<Unit>();
        onFieldUnit[u.No]--;
        if(onFieldUnit[u.No] == 0)
        { //완전히 유닛이 없을때
            onFieldUnit.Remove(u.No);
            traitList[u.Origin]--;
            if(traitList[u.Origin] == 0)
            { //소속유닛이 하나도 없을때
                traitList.Remove(u.Origin);
            }
            traitList[u.Class]--;
            if (traitList[u.Class] == 0)
            { // 직업유닛이 하나도 없을때
                traitList.Remove(u.Class);
            }
        }
        UpdateTraitBar();
        u.ResetTrait();
    }
    #endregion

    #region Trait
    public Dictionary<int, int> onFieldUnit;
    public Dictionary<int, int> traitList;
    [SerializeField] private TraitBar[] traitBars;

    private bool CheckTraitActive(int no, int num)
    {
        if(num >= int.Parse(info.Traits[no]["Rank1"]))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void UpdateTraitBar()
    {
        List<KeyValuePair<int, int>> activeTrait = new List<KeyValuePair<int, int>>();
        List<KeyValuePair<int, int>> nonActiveTrait = new List<KeyValuePair<int, int>>();
        var sortedList = traitList.OrderByDescending(kvp => kvp.Value).ToList();
        foreach(var kvp in sortedList)
        {
            if(CheckTraitActive(kvp.Key, kvp.Value))
            {
                activeTrait.Add(kvp);
            }
            else
            {
                nonActiveTrait.Add(kvp);
            }
            SetUnitTrait(kvp);
        }
        for (int i = 0; i < traitBars.Length; i++)
        {
            if (i < activeTrait.Count)
            {
                traitBars[i].gameObject.SetActive(true);
                traitBars[i].InitInfo(activeTrait[i], true);
            }
            else if (i < activeTrait.Count + nonActiveTrait.Count)
            {
                traitBars[i].gameObject.SetActive(true);
                traitBars[i].InitInfo(nonActiveTrait[i - activeTrait.Count], false);
            }
            else
            {
                traitBars[i].gameObject.SetActive(false);
            }
        }
    }
    private void SetUnitTrait(KeyValuePair<int, int> trait)
    {
        for(int i = 0; i< fieldList.Count; i++)
        {
            Unit unit = fieldList[i].GetComponent<Unit>();
            unit.UpdateTrait(trait.Key, trait.Value);
        }
    }
    #endregion

    private void Start()
    {
        Level = 1;
        Gold = 999;
        curExp = 0;
        maxExp = maxExpList[0];
        benchList = new List<GameObject>();
        fieldList = new List<GameObject>();
        onFieldUnit = new Dictionary<int, int>();
        traitList = new Dictionary<int, int>();
    }
    private void OnEnable()
    {
        round.OnStepChange += SetFieldUnitState;
    }
    private void OnDisable()
    {
        round.OnStepChange -= SetFieldUnitState;
    }
}
