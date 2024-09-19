using System;
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
    private Step step;
    public Step curStep
    {
        get { return step; }
        private set
        {
            step = value;
            OnStepChange?.Invoke();
        }
    }
    public bool IsBattleStep => step == Step.Battle;

    public event Action OnStepChange;

    public void TransitionStep()
    {
        curStep = step == Step.Battle ? Step.Prepare : Step.Battle;
        if(IsBattleStep)
        {
            fieldText.SetActive(false);
        }
        else
        {
            fieldText.SetActive(true);
        }
    }
}
