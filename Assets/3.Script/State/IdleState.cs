using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateMachineBehaviour
{
    private Unit unit;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unit = animator.GetComponentInParent<Unit>();
        Debug.Log("IdleStart");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(unit.isOnField &&
            unit.roundManager.isBattle)
        {
            Collider[] targets = Physics.OverlapSphere(animator.transform.position, 9f, LayerMask.GetMask("Enemy"));
            //unit.SetEnemyTarget(FindNearestTarget(targets));
            animator.SetTrigger("Move");
            Debug.Log($"target:{targets[0].name}");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            Debug.Log("IdleExit");
    }

    private Unit FindNearestTarget(Collider[] targets)
    {
        float max = 50;
        Collider nearest = null;

        foreach(var t in targets)
        {
            float distance = Vector3.Distance(unit.transform.position, t.transform.position);
            if (distance < max)
            {
                max = distance;
                nearest = t;
            }
        }
        return nearest.GetComponent<Unit>();
    }
}
