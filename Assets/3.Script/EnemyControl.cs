using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    
    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent agent;


    private void Start()
    {
        agent.SetDestination(target.position);
    }
    

}
