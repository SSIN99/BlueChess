using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Step
{
    Prepare,
    Battle
}

public class RoundManager : MonoBehaviour
{
    [SerializeField] private GameObject fieldText;
    public Step step;
    public bool IsBattleStep => step == Step.Battle;

    public void TransitionStep()
    {
        step = step == Step.Battle ? Step.Prepare : Step.Battle;
        if(step == Step.Battle)
        {
            fieldText.SetActive(false);
        }
        else
        {
            fieldText.SetActive(true);
        }
    }
}
