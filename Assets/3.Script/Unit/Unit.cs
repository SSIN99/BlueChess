using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [Header("유닛 신상정보")]
    public int No;
    public string Name;
    public int Origin;
    public int Class;
    public int Cost;
    [Header("기본 스텟")]
    public float maxHealth;
    public float curHealth;
    public int maxMP;
    public int startMP;
    public int curMP;
    [Header("공격관련")]
    public int AD;
    public int AP;
    public float AS;
    public float CritRatio;
    public float CritDamage;
    [Header("방어관련")]
    public int Armor;
    public int Resistance;
    [Header("기타능력치")]
    public float Range;
    public float Speed;
    public void InitInfo(Dictionary<string, string> data)
    {
        No = int.Parse(data["No"]);
        Name = data["Name"];
        Origin = int.Parse(data["Origin"]);
        Class = int.Parse(data["Class"]);
        Cost = int.Parse(data["Cost"]);

        maxHealth = float.Parse(data["Health"]);
        curHealth = maxHealth;
        maxMP = int.Parse(data["MaxMP"]);
        startMP = int.Parse(data["StartMP"]);
        curMP = startMP;

        AD = int.Parse(data["AD"]);
        AP = 100;
        AS = float.Parse(data["AS"]);
        CritRatio = 25f;
        CritRatio = 150f;
        Armor = int.Parse(data["Armor"]);
        Resistance = int.Parse(data["Resistance"]);

        Range = float.Parse(data["Range"]);
        Speed = 3f;
    }

    [Header("Etc")]
    public NavMeshAgent agent;
    public Animator anim;
    public GameObject statusBar;
    public GameObject target;
    public bool isDead;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    public void CheckAttackRange()
    {
        float radius = 2 * Range;
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= radius)
        {
            transform.LookAt(target.transform.position);
            agent.enabled = false;
            anim.Play("Attack");
        }
        else
        {
            agent.enabled = true;
            agent.SetDestination(target.transform.position);
        }
    }
    public virtual void StartBattle()
    {
        statusBar.SetActive(true);
        statusBar.GetComponent<StatusBar>().InitStatusBar(maxHealth, maxMP, startMP);
        anim.Play("Search");
    }
    public virtual void ReturnIdle()
    {
        isDead = false;
        statusBar.SetActive(false);
        agent.enabled = false;
        anim.Play("Idle");
    }
    public void AutoAttack()
    {
        if(target != null && target.activeSelf == true && !target.GetComponent<Unit>().isDead)
            target.GetComponent<Unit>().TakeDamage(AD);
    }
    public void TakeDamage(float damage)
    {
        float actualDamage = damage * (1 - (Armor / (Armor + 100)));
        curHealth -= actualDamage;
        Mathf.Clamp(curHealth, 0, maxHealth);
        statusBar.GetComponent<StatusBar>().UpdateStatusBar(curHealth);
        if(curHealth <= 0)
        {
            isDead = true;
            anim.Play("Dead");
        }
    }
}
