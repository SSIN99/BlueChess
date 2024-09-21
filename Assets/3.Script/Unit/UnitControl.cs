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
        base.StartBattle();
    }
    public override void ReturnIdle()
    {
        transform.rotation = Quaternion.identity;
        base.ReturnIdle();
    }
   
}
