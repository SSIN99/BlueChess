using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Unit>().DetectTarget();
    }
}
