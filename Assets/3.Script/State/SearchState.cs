using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LayerMask layer = LayerMask.GetMask("Enemy");

        Collider[] objs = Physics.OverlapSphere(animator.transform.position, 10f, layer);

        Array.Sort(objs, (a, b) 
            => Vector3.Distance(animator.transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(animator.transform.position, b.transform.position)));

        animator.GetComponent<UnitControl>().enemy = objs[0].gameObject;
        animator.SetTrigger("Move");
    }
}
