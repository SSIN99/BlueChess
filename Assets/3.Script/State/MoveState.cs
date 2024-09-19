using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : StateMachineBehaviour
{
    private UnitControl unit;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unit = animator.GetComponent<UnitControl>();
        unit.agent.enabled = true;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unit.agent.SetDestination(unit.enemy.transform.position);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
