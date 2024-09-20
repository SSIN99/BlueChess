using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LayerMask layer;
        if (animator.CompareTag("Unit"))
        {
            layer = LayerMask.GetMask("Enemy");
        }
        else
        {
            layer = LayerMask.GetMask("Unit");
        }

        Collider[] objs = Physics.OverlapSphere(animator.transform.position, 15f, layer);

        if(objs.Length == 0)
        {
            animator.Play("Win");
            return;
        }

        Array.Sort(objs, (a, b) 
            => Vector3.Distance(animator.transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(animator.transform.position, b.transform.position)));

        animator.GetComponent<Unit>().target = objs[0].gameObject;
        animator.Play("Move");
    }
}
