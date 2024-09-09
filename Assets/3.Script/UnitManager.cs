using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private Info info;
    [SerializeField] private Tile[] benchTile;
    public int numOfBench = 0;
    private List<Unit> unitList = new List<Unit>();

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
    public void CheckBench()
    {
        numOfBench = 0;
        for(int i = 0; i< benchTile.Length; i++)
        {
            if (!benchTile[i].isEmpty)
            {
                numOfBench++;
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
                CheckBench();
                unit.BeSold();
                info.unitPool[unit.info.no].Enqueue(unit.gameObject);
                unit.gameObject.SetActive(false);
                Debug.Log($"{unit.info.no} {info.unitPool[unit.info.no].Count}");
                break;
            }
    }
}
}
