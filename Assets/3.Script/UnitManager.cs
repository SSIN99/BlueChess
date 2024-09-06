using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private Tile[] waitingSeat;
    public int numOfWaitingUnit = 0;

    public void PlaceWaitingUnit(GameObject unit)
    {
        for (int i = 0; i < waitingSeat.Length; i++)
        {
            if (waitingSeat[i].isEmpty)
            {
                unit.transform.position = waitingSeat[i].pos;
                waitingSeat[i].isEmpty = false;
                numOfWaitingUnit++;
                break;
            }
        }
    }

}
