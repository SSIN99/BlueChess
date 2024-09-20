using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class UnitControl : Unit
{
    public override void StartBattle() 
    {
        UnitArrange unit = GetComponent<UnitArrange>();
        unit.enabled = false;
        base.StartBattle();
    }
    public override void ReturnIdle()
    {
        UnitArrange unit = GetComponent<UnitArrange>();
        unit.enabled = true;
        unit.ReturnTile();
        transform.rotation = Quaternion.identity;
        base.ReturnIdle();
    }
   
}
