using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonoBehaviour,IState
{
    EnemyControl enemy;

    public IdleState(EnemyControl enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
       
    }

    public void Stay()
    {
       
    }

    public void Exit()
    {
      
    }
}
