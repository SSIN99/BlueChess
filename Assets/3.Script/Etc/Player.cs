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
    #region UI
    [SerializeField] private Text levelText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text expText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TMP_Text fieldText;
    [SerializeField] private Image lockImage;
    [SerializeField] private Sprite[] lockSprite;
    #endregion

    #region Player
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
    [SerializeField] private GameObject statusBar;
    [SerializeField] private Transform canvas;
    private List<Unit> benchList;
    private List<Unit> fieldList;
    private int numOfBench = 0;
    private int numOfField = 0;
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
    public event Action OnPurchaseUnit;
    public event Action OnSellUnit;
    public void ToggleLock()
    {
        isLocked = !isLocked;
        lockImage.sprite = isLocked ? lockSprite[0] : lockSprite[1];
    }
    public void RecruitUnit()
    {
        if (isLocked)
        {
            ToggleLock();
        }
        if (Gold < 2) return;
        Gold -= 2;
        shop.SetShopItem();
    }
    public void PurchaseUnit(int no)
    {
        GameObject newbie = Instantiate(info.prefabs[no]);
        Unit unit = newbie.GetComponent<Unit>();
        GameObject status = Instantiate(statusBar);
        status.GetComponent<StatusBar>().SetUnit(unit);
        status.transform.SetParent(canvas);
        unit.InitInfo(info.Units[no]);
        Gold -= unit.Cost;
        AddBench(unit);
        for (int i = 0; i < bench.Length; i++)
        {
            if (bench[i].unit == null)
            {
                newbie.GetComponent<ArrangeControl>().InitTile(bench[i]);
                break;
            }
        }
        if (UnitGrade1.ContainsKey(no))
        {
            UnitGrade1[no]++;
        }
        else
        {
            UnitGrade1.Add(no, 1);
        }
        CheckGradePossible(no, 1);
        OnPurchaseUnit?.Invoke();
    }
    public void SellUnit(GameObject subject)
    {
        ArrangeControl arrange = subject.GetComponent<ArrangeControl>();
        UnitControl unit = subject.GetComponent<UnitControl>();
        unit.BeSold();
        switch (unit.Grade)
        {
            case 1:
                Gold += unit.Cost;
                info.unitCount[unit.No]++;
                UnitGrade1[unit.No]--;
                break;
            case 2:
                Gold += (unit.Cost * 3) - 1;
                info.unitCount[unit.No] += 3;
                UnitGrade2[unit.No]--;
                break;
            case 3:
                Gold += (unit.Cost * 9) - 1;
                info.unitCount[unit.No] += 9;
                break;
        }
        for(int i =0; i < unit.ItemCount; i++)
        {
            AddItem(unit.itemList[i]);
        }

        if (arrange.IsOnField)
        {
            RemoveField(unit);
        }
        else
        {
            RemoveBench(unit);
        }
        arrange.LeaveTile();
        Destroy(subject);
        Debug.Log($"{unit.No}�� ���� �Ǹ�, { info.unitCount[unit.No]}�� �ܿ�");
        OnSellUnit?.Invoke();
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
                if(fieldList[i].gameObject.activeSelf == false)
                {
                    fieldList[i].gameObject.SetActive(true);
                }
                fieldList[i].GetComponent<UnitControl>().IsBattle = false;
                
            }
            CheckAllGradeUp();
        }
    }
    public void AddBench(Unit unit)
    {
        unit.gameObject.layer = LayerMask.NameToLayer("Bench");
        benchList.Add(unit);
        numOfBench++;
    }
    public void RemoveBench(Unit unit)
    {
        benchList.Remove(unit);
        numOfBench--;
    }
    #endregion

    #region Trait
    [SerializeField] private TraitBar[] traitBars;
    public Dictionary<int, int> traitCount; 
    public Dictionary<int, int> traitRank;
    public void AddField(Unit unit)
    {
        unit.gameObject.layer = LayerMask.NameToLayer("Field");
        if (!CheckDuplicateUnit(unit.No))
        {
            traitCount[unit.Origin]++;
            traitCount[unit.Class]++;
            CheckTraitChanged(unit.Origin);
            CheckTraitChanged(unit.Class);
        }
        fieldList.Add(unit);
        NumOfField++;
        UpdateNewUnitTrait(unit);
        UpdateTraitBar();
    }
    public void RemoveField(Unit unit)
    {
        fieldList.Remove(unit);
        NumOfField--;
        RemoveUnitTrait(unit);
        if (!CheckDuplicateUnit(unit.No))
        {
            traitCount[unit.Origin]--;
            traitCount[unit.Class]--;
            CheckTraitChanged(unit.Origin);
            CheckTraitChanged(unit.Class);
        }
        UpdateTraitBar();
    }
    private bool CheckDuplicateUnit(int no)
    {
        foreach(var u in fieldList)
        {
            if(u.No == no)
            {
                return true;
            }
        }
        return false;
    }
    private int CheckTraitRank(int no, int count)
    {
        if (count < int.Parse(info.Traits[no]["Rank1"]))
        {
            return 0;
        }
        else if (count >= int.Parse(info.Traits[no]["Rank1"]) &&
            count < int.Parse(info.Traits[no]["Rank2"]))
        {
            return 1;
        }
        else if (count >= int.Parse(info.Traits[no]["Rank2"]) &&
            count < int.Parse(info.Traits[no]["Rank3"]))
        {
            return 2;
        }
        else if (count >= int.Parse(info.Traits[no]["Rank3"]) &&
           count < int.Parse(info.Traits[no]["Rank4"]))
        {
            return 3;
        }
        else
        {
            return 4;
        }
    }
    private void CheckTraitChanged(int no)
    {
        int rank = CheckTraitRank(no, traitCount[no]);
        if (traitRank[no] != rank)
        {
            UpdateAllUnitTrait(no, traitRank[no], rank);
            traitRank[no] = rank;
        }
    }
    public bool CheckUnitOnField(int no)
    {
        foreach (var u in fieldList)
        {
            if (u.No == no)
                return true;
        }
        return false;
    }
    private void UpdateNewUnitTrait(Unit unit)
    {
        for(int i = 0; i< traitRank.Count; i++)
        {
            if(traitRank[i] > 0)
            {
                unit.UpdateTrait(i, 0, traitRank[i]);
            }
        }
    }
    private void RemoveUnitTrait(Unit unit)
    {
        for (int i = 0; i < traitRank.Count; i++)
        {
            if (traitRank[i] > 0)
            {
                unit.UpdateTrait(i, traitRank[i], 0);
            }
        }
    }
    private void UpdateAllUnitTrait(int no, int old, int newR)
    {
        foreach (var u in fieldList)
        {
            u.UpdateTrait(no ,old, newR);
        }
    }
    private void UpdateTraitBar()
    {
        List<KeyValuePair<int, int>> activeTrait = new List<KeyValuePair<int, int>>();
        List<KeyValuePair<int, int>> nonActiveTrait = new List<KeyValuePair<int, int>>();
        var sortedList = traitRank.OrderByDescending(kvp => kvp.Value).ToList();
        foreach(var kvp in sortedList)
        {
            if (traitCount[kvp.Key] == 0) continue;
            if(traitRank[kvp.Key] != 0)
            {
                activeTrait.Add(kvp);
            }
            else
            {
                nonActiveTrait.Add(kvp);
            }
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

    #endregion

    #region Item
    [Header("Item")]
    [SerializeField] private RectTransform[] pivots;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private RectTransform inventory;
    [SerializeField] private Text itemCount;
    [SerializeField] private Image bagIcon;
    [SerializeField] private Sprite[] bagImage;
    private bool isOpend = false;
    private List<Item> itemList;
  
    public void OpenInventory()
    {
        if (isOpend)
        {
            isOpend = false;
            inventory.DOAnchorPosX(-375f, 0.2f);
            bagIcon.sprite = bagImage[0];
        }
        else
        {
            isOpend = true;
            inventory.DOAnchorPosX(20f, 0.2f);
            bagIcon.sprite = bagImage[1];
        }
    }
    public void AddItem(int no)
    {
        if (itemList.Count >= 12) return;
        GameObject temp = Instantiate(itemPrefab);
        temp.transform.SetParent(inventory);
        temp.transform.position = pivots[itemList.Count].position;
        Item item = temp.GetComponent<Item>();
        itemList.Add(item);
        item.SetItem(no);
        itemCount.text = itemList.Count.ToString();
    }
    public void RemoveItem(Item item)
    {
        itemList.Remove(item);
        itemCount.text = itemList.Count.ToString();
        SortInventory();
    }
    public void SortInventory()
    {
        for(int i = 0; i < itemList.Count; i++)
        {
            itemList[i].transform.position = pivots[i].position;       }
    }
    public void GetRandomItem()
    {
        int rand = Random.Range(0, info.Items.Count);
        AddItem(rand);
    }
    #endregion

    #region Grade
    private Dictionary<int, int> UnitGrade1;
    private Dictionary<int, int> UnitGrade2;
    private void CheckAllGradeUp()
    {
        foreach(var kvp in UnitGrade1.ToList())
        {
            CheckGradePossible(kvp.Key, 1);
        }
        foreach (var kvp in UnitGrade2.ToList())
        {
            CheckGradePossible(kvp.Key, 2);
        }
    }
    private void CheckGradePossible(int no, int grade)
    {
        if(grade == 1)
        {
            if(UnitGrade1[no] >= 3)
            {
                if (round.IsBattleStep)
                {
                    if(CheckBenchUnitCount(no, grade) >= 3)
                    {
                        GradeUp(no, 1, true);
                    }
                }
                else
                {
                    GradeUp(no, 1, false);
                }
            }
        }
        else
        {
            if (UnitGrade2[no] >= 3)
            {
                if (round.IsBattleStep)
                {
                    if (CheckBenchUnitCount(no, grade) >= 3)
                    {
                        GradeUp(no, 2, true);
                    }
                }
                else
                {
                    GradeUp(no, 2, false);
                }
            }
        }
    }
    private int CheckBenchUnitCount(int no, int grade)
    {
        int count = 0;
        for (int i = 0; i < benchList.Count; i++)
        {
            if (benchList[i].No == no &&
                benchList[i].Grade == grade)
                count++;
        }
        return count;
    }
    private void GradeUp(int no, int grade, bool bench)
    {
        Unit target = null;
        Unit offer1 = null;
        Unit offer2 = null;
        if (bench)
        {
            for (int i = 0; i < benchList.Count; i++)
            {
                if (benchList[i].No == no &&
                    benchList[i].Grade == grade)
                {
                    if (target == null)
                    {
                        target = benchList[i];
                    }
                    else
                    {
                        if (offer1 == null)
                        {
                            offer1 = benchList[i];
                        }
                        else
                        {
                            offer2 = benchList[i];
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < fieldList.Count; i++)
            {
                if (fieldList[i].No == no &&
                    fieldList[i].Grade == grade)
                {
                    if (target == null)
                    {
                        target = fieldList[i];
                    }
                    else
                    {
                        if (offer1 == null)
                        {
                            offer1 = fieldList[i];
                        }
                        else
                        {
                            offer2 = fieldList[i];
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < benchList.Count; i++)
            {
                if (benchList[i].No == no &&
                    benchList[i].Grade == grade)
                {
                    if (target == null)
                    {
                        target = benchList[i];
                    }
                    else
                    {
                        if (offer1 == null)
                        {
                            offer1 = benchList[i];
                        }
                        else
                        {
                            offer2 = benchList[i];
                            break;
                        }
                    }
                }
            }
        }
        Offering(offer1, target);
        Offering(offer2, target);
        target.GradeUp();

        if (grade == 1)
        {
            UnitGrade1[no] -= 3;
            if (UnitGrade2.ContainsKey(no))
            {
                UnitGrade2[no]++;
                CheckGradePossible(no, 2);
            }
            else
            {
                UnitGrade2.Add(no, 1);
            }
        }
        else
        {
            UnitGrade2[no] -= 3;
        }
    }
    private void Offering(Unit offer, Unit target)
    {
        ArrangeControl arrange = offer.GetComponent<ArrangeControl>();
        offer.BeSold();
        for (int i = 0; i < offer.ItemCount; i++)
        {
            if (!target.IsItemFull)
            {
                target.EquipItem(offer.itemList[i]);
            }
            else
            {
                AddItem(offer.itemList[i]);
            }
        }

        if (arrange.IsOnField)
        {
            RemoveField(offer);
        }
        else
        {
            RemoveBench(offer);
        }
        arrange.LeaveTile();
        Destroy(offer.gameObject);
    }
    public bool CheckHaveUnit(int no)
    {
        for(int i = 0; i < benchList.Count; i++)
        {
            if (benchList[i].No == no) return true;
        }
        for (int i = 0; i < fieldList.Count; i++)
        {
            if (fieldList[i].No == no) return true;
        }
        return false;
    }
    public int CheckUnitGrade(int no)
    {
        if(UnitGrade2.ContainsKey(no) && 
            UnitGrade2[no] == 2 &&
            UnitGrade1.ContainsKey(no) && 
            UnitGrade1[no] == 2)

        {
            return 2;
        }
        else
        {
            if(UnitGrade1.ContainsKey(no) && 
                UnitGrade1[no] == 2)
            {
                return 1;
            }
        }
        return 0;
    }

    #endregion

    private void Start()
    {
        Level = 1;
        Gold = 999;
        curExp = 0;
        maxExp = maxExpList[0];
        benchList = new List<Unit>();
        fieldList = new List<Unit>();
        traitCount = new Dictionary<int, int>();
        traitRank = new Dictionary<int, int>();
        for(int i =0; i< info.Traits.Count; i++)
        {
            traitCount.Add(i, 0);
            traitRank.Add(i, 0);
        }
        itemList = new List<Item>();
        UnitGrade1 = new Dictionary<int, int>();
        UnitGrade2 = new Dictionary<int, int>();
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
