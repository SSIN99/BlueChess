using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillState : StateMachineBehaviour
{
    private Unit unit;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unit = animator.GetComponent<Unit>();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unit.CheckTargetDead();
        unit.LookAtTarget();
        if(stateInfo.normalizedTime >= 1)
        {
            animator.Play("Attack");
        }
    }
}
