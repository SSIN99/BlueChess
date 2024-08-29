using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Dead
    }

    private State curState;


    [SerializeField] private Transform route;
    [SerializeField] private Animator anim;
    private NavMeshAgent agent;

    private Waypoint[] waypoints;

    private int index = 0;
    private float timer = 0f;
    private bool isWait = false;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        waypoints = new Waypoint[route.childCount];
        for(int i =0; i < route.childCount; i++)
        {
            waypoints[i] = route.GetChild(i).GetComponent<Waypoint>();
        }

        agent.SetDestination(waypoints[index].transform.position);
        anim.Play("Move");
    }

    private void Update()
    {
        if (index >= waypoints.Length) return;

        if (isWait)
        {
            agent.isStopped = true;
            anim.Play("Idle");

            if(timer < waypoints[index].waitTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                index++;
                agent.SetDestination(waypoints[index].transform.position);
                isWait = false;
                timer = 0;
            }
        }
        else
        {
            agent.isStopped = false;
            anim.Play("Move");

            if (Vector3.Distance(transform.position, waypoints[index].transform.position) < 0.1f)
            {
                if (!waypoints[index].waitTime.Equals(0))
                {
                    isWait = true;
                }
                else
                {
                    index++;
                    if(index < waypoints.Length)
                    {
                        agent.SetDestination(waypoints[index].transform.position);
                    }
                    else
                    {
                        agent.isStopped = true;
                        anim.Play("Idle");
                    }
                }
            }
        }
    }
}
