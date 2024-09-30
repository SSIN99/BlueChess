using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanicState : StateMachineBehaviour
{
    private Unit unit;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unit = animator.GetComponent<Unit>();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unit.Stuned();
    }
}
