using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanicState : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("panic");
    }
}
