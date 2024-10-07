using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private Info info;
    [SerializeField] private Player player;
    [SerializeField] private RoundManager round;
    [SerializeField] private ShopManager shop;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform statusBarCanvas;
    [SerializeField] private GameObject statusBarUI;
    [SerializeField] private TraitBarUI[] traitBars;
    [SerializeField] private Tile[] benchTile;
    [SerializeField] private TMP_Text fieldText;

    public List<Unit> benchList;
    public List<Unit> fieldList;
    private int numOfBench = 0;
    private int numOfField = 0;
    public bool isFullBench => numOfBench >= 8;
    public bool isFullField => numOfField >= player.Level;

    private Dictionary<int, int> UnitGrade1;
    private Dictionary<int, int> UnitGrade2;
    public Dictionary<int, int> traitCount;
    public Dictionary<int, int> traitRank;

    public event Action OnGetUnit;
    public event Action OnSellUnit;
    public event Action OnBenchChanged;

    private void Start()
    {
        benchList = new List<Unit>();
        fieldList = new List<Unit>();
        traitCount = new Dictionary<int, int>();
        traitRank = new Dictionary<int, int>();
        for (int i = 0; i < info.Traits.Count; i++)
        {
            traitCount.Add(i, 0);
            traitRank.Add(i, 0);
        }
        UnitGrade1 = new Dictionary<int, int>();
        UnitGrade2 = new Dictionary<int, int>();
    }
    private void OnEnable()
    {
        player.OnLevelChanged += UpdateFieldText;
    }
    private void OnDisable()
    {
        player.OnLevelChanged -= UpdateFieldText;
    }
    public void ArrangeMap(Transform pivot, Tile[] bench, TMP_Text text)
    {
        Camera.main.transform.position = pivot.position;
        benchTile = bench;
        fieldText = text;
    }

    #region Unit
    public void GetUnit(int no)
    {
        GameObject newUnit = Instantiate(info.prefabs[no]);
        Unit unit = newUnit.GetComponent<Unit>();

        GameObject statusBar = Instantiate(statusBarUI, statusBarCanvas);
        statusBar.GetComponent<StatusBarUI>().InitInfo(unit);
        unit.InitInfo(info.Units[no]);

        for (int i = 0; i < benchTile.Length; i++)
        {
            if (benchTile[i].unit == null)
            {
                newUnit.GetComponent<Arrangement>().InitTile(benchTile[i]);
                break;
            }
        }
        if (UnitGrade1.ContainsKey(no))
        {
            UnitGrade1[no] += 1;
        }
        else
        {
            UnitGrade1.Add(no, 1);
        }
        AddBench(unit);
        CheckPossibleGradeUp(no, 1);
        OnGetUnit?.Invoke();
    }
    public void SellUnit(GameObject subject)
    {
        Unit unit = subject.GetComponent<Unit>();
        unit.BeSold();

        int price = 0;
        switch (unit.Grade)
        {
            case 1:
                price = unit.Cost;
                UnitGrade1[unit.No]--;
                break;
            case 2:
                price = (unit.Cost * 3) - 1;
                UnitGrade2[unit.No]--;
                break;
            case 3:
                price = (unit.Cost * 9) - 1;
                break;
        }
        player.UpdateGold(price);

        for (int i = 0; i < unit.ItemCount; i++)
        {
            inventory.AddItem(unit.itemList[i]);
        }

        Destroy(subject);
        OnSellUnit?.Invoke();
    }
    private void Offering(Unit offer, Unit target)
    {
        offer.BeSold();
        for (int i = 0; i < offer.ItemCount; i++)
        {
            if (!target.IsItemFull)
            {
                target.EquipItem(offer.itemList[i]);
            }
            else
            {
                inventory.AddItem(offer.itemList[i]);
            }
        }
        Destroy(offer.gameObject);
    }
    public void BattleStart()
    {
        for (int i = 0; i < fieldList.Count; i++)
        {
            fieldList[i].IsBattle = true;
        }
    }
    public void PrepareStart()
    {
        for (int i = 0; i < fieldList.Count; i++)
        {
            if (!fieldList[i].gameObject.activeSelf)
            {
                fieldList[i].gameObject.SetActive(true);
            }
            fieldList[i].IsBattle = false;
        }
        CheckAllUnitPossibleGradeUp();
    }
    public void UpdateFieldText()
    {
        fieldText.text = $"{numOfField} / {player.Level}";
        fieldText.color = isFullField ? Color.red : new Color(0.35f, 0.75f, 1);
    }
    public void AddBench(Unit unit)
    {
        unit.SetArrangeState(false);
        benchList.Add(unit);
        numOfBench += 1;
        OnBenchChanged?.Invoke();
    }
    public void RemoveBench(Unit unit)
    {
        benchList.Remove(unit);
        numOfBench -= 1;
        OnBenchChanged?.Invoke();
    }
    public void AddField(Unit unit)
    {
        unit.SetArrangeState(true);
        if (!CheckDuplicateUnit(unit.No))
        {
            traitCount[unit.Origin]++;
            traitCount[unit.Class]++;
            if (CheckTraitChanged(unit.Origin))
                PrintActiveTraitVFX(unit.Origin, unit);
            if (CheckTraitChanged(unit.Class))
                PrintActiveTraitVFX(unit.Class, unit);
        }
        fieldList.Add(unit);
        numOfField += 1;
        SetNewUnitTrait(unit);
        UpdateFieldText();
        UpdateTraitBar();
    }
    public void RemoveField(Unit unit)
    {
        fieldList.Remove(unit);
        numOfField -= 1;
        UpdateFieldText();
        ResetUnitTrait(unit);
        if (!CheckDuplicateUnit(unit.No))
        {
            traitCount[unit.Origin]--;
            traitCount[unit.Class]--;
            CheckTraitChanged(unit.Origin);
            CheckTraitChanged(unit.Class);
        }
        UpdateTraitBar();
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
    private bool CheckDuplicateUnit(int no)
    {
        foreach (var u in fieldList)
        {
            if (u.No == no)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Trait
    public int CheckTraitRank(int no)
    {
        int count = traitCount[no];
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
    private bool CheckTraitChanged(int no)
    {
        int rank = CheckTraitRank(no);
        if (traitRank[no] != rank)
        {
            UpdateAllUnitTrait(no, traitRank[no], rank);
            traitRank[no] = rank;
            return true;
        }
        return false;
    }
    private void ResetUnitTrait(Unit unit)
    {
        for (int i = 0; i < traitRank.Count; i++)
        {
            if (traitRank[i] > 0)
            {
                unit.UpdateTrait(i, traitRank[i], 0);
            }
        }
    }
    private void SetNewUnitTrait(Unit unit)
    {
        for (int i = 0; i < traitRank.Count; i++)
        {
            if (traitRank[i] > 0)
            {
                unit.UpdateTrait(i, 0, traitRank[i]);
            }
        }
    }
    private void UpdateAllUnitTrait(int no, int oldR, int newR)
    {
        foreach (var u in fieldList)
        {
            u.UpdateTrait(no, oldR, newR);
        }
    }
    private void UpdateTraitBar()
    {
        List<KeyValuePair<int, int>> activeTrait = new List<KeyValuePair<int, int>>();
        List<KeyValuePair<int, int>> nonActiveTrait = new List<KeyValuePair<int, int>>();
        var sortedList = traitCount.OrderByDescending(kvp => kvp.Value).ToList();
        foreach (var kvp in sortedList)
        {
            if (traitCount[kvp.Key] == 0) continue;
            if (traitCount[kvp.Key] >= int.Parse(info.Traits[kvp.Key]["Rank1"]))
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
    private void PrintActiveTraitVFX(int no, Unit newUnit)
    {
        for (int i = 0; i < fieldList.Count; i++)
        {
            if (fieldList[i].Origin == no ||
                fieldList[i].Class == no)
            {
                fieldList[i].PrintTraitVFX();
            }
        }
        newUnit.PrintTraitVFX();
    }
    #endregion

    #region Grade
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
                CheckPossibleGradeUp(no, 2);
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
    private void CheckAllUnitPossibleGradeUp()
    {
        foreach (var kvp in UnitGrade1)
        {
            CheckPossibleGradeUp(kvp.Key, 1);
        }
        foreach (var kvp in UnitGrade2)
        {
            CheckPossibleGradeUp(kvp.Key, 2);
        }
    }
    private void CheckPossibleGradeUp(int no, int grade)
    {
        if (grade == 1)
        {
            if (UnitGrade1[no] >= 3)
            {
                if (round.IsBattleStep)
                {
                    if (CheckBenchUnitCount(no, grade) >= 3)
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
    public bool CheckHaveUnit(int no)
    {
        for (int i = 0; i < benchList.Count; i++)
        {
            if (benchList[i].No == no) return true;
        }
        for (int i = 0; i < fieldList.Count; i++)
        {
            if (fieldList[i].No == no) return true;
        }
        return false;
    }
    public int CheckShopItemVFX(int no)
    {
        if (UnitGrade2.ContainsKey(no) &&
            UnitGrade2[no] == 2 &&
            UnitGrade1.ContainsKey(no) &&
            UnitGrade1[no] == 2)
        {
            return 2;
        }
        else
        {
            if (UnitGrade1.ContainsKey(no) &&
                UnitGrade1[no] == 2)
            {
                return 1;
            }
        }
        return 0;
    }
    #endregion
}
