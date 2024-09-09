using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private Info info;
    [SerializeField] private Tile[] benchTile;
    [SerializeField] private Player player;
    [SerializeField] private TMP_Text numOfField_T;
    private List<Unit> unitList = new List<Unit>();
    public int numOfBench = 0;
    public int numOfField = 0;
    public int maxNumOfField = 1;

    private void OnEnable()
    {
        player.OnLevelUp += UpMaxNumOfField;
    }
    public void UpMaxNumOfField()
    {
        maxNumOfField++;
        numOfField_T.text = $"{numOfField} / {maxNumOfField}";
    }
    public void AddUnit(int n, GameObject u)
    {
        u.SetActive(true);
        Unit unit = u.GetComponent<Unit>();
        unitList.Add(unit);
        for (int i = 0; i < benchTile.Length; i++)
        {
            if (benchTile[i].isEmpty)
            {
                unit.SetTile(benchTile[i]);
                unit.InitData(info.unitData[n]);
                benchTile[i].isEmpty = false;
                break;
            }
        }
    }
    public void RemoveUnit(Unit unit)
    {
        for (int i = unitList.Count - 1; i >= 0; i--)
        {
            if (unit.Equals(unitList[i]))
            {
    		    unitList.Remove(unit);
                unit.BeSold();
                info.unitPool[unit.info.no].Enqueue(unit.gameObject);
                unit.gameObject.SetActive(false);
                Debug.Log($"{unit.info.no}ÆÇ¸Å {info.unitPool[unit.info.no].Count}");
                break;
            }
    }
}
    public void CheckUnitList()
    {
        numOfBench = 0;
        numOfField = 0;
        for (int i = 0; i< unitList.Count; i++)
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
        numOfField_T.text = $"{numOfField} / {maxNumOfField}";
    }
    private void OnDisable()
    {
        player.OnLevelUp -= UpMaxNumOfField;
    }
}
