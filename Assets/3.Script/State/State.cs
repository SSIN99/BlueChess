using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour 
{


    public abstract void Enter(EnemyControl enemy);
    public abstract void Excute(EnemyControl enemy);
    public abstract void Exit(EnemyControl enemy);
}
