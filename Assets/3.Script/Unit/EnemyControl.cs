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
            ReturnIdle();
        else
            StartBattle();
    }

    public override void ReturnIdle()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        base.ReturnIdle();
    }

    private void OnDisable()
    {
        round.OnStepChange -= SetState;
    }

}
