using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private Info info;
    [SerializeField] private Tile[] benchTile;
    public int numOfBench = 0;
    private List<Unit> unitList = new List<Unit>();

    public void PlaceBenchUnit(int n, GameObject u)
    {
        u.transform.parent = transform;
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
                numOfBench++;
                break;
            }
        }
    }

}
