using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : Unit
{
    private Info info;
    private RoundManager round;

    private void Awake()
    {
        info = GameObject.FindGameObjectWithTag("Info").GetComponent<Info>();
        round = GameObject.FindGameObjectWithTag("Round").GetComponent<RoundManager>();
    }
    private void OnEnable()
    {
        round.OnStepChange += SetState;
    }
    protected override void Start()
    {
        base.Start();
        InitInfo(info.enemyData[No]);
    }
    public void SetState()
    {
        if (!round.IsBattleStep)
            IsBattle = false;
        else
            IsBattle = true;
    }

   
    private void OnDisable()
    {
        round.OnStepChange -= SetState;
    }

}
