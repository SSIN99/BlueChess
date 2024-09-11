using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Step
{
    Preparation,
    Battle
}

public class RoundManager : MonoBehaviour
{
    [SerializeField] GameObject fieldText; 

    private Step roundStep;
    public bool isBattle => roundStep == Step.Battle;
    public Step RoundStep
    {
        get { return roundStep; }
        private set
        {
            roundStep = value;
        }
    }

    public void Test()
    {
        if (roundStep == Step.Battle)
        {
            roundStep = Step.Preparation;
            fieldText.SetActive(true);
        }
        else
        {
            roundStep = Step.Battle;
            fieldText.SetActive(false);
        }
    }


}
