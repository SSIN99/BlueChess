using System;
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
    public int Grade;
    [Header("기본 스텟")]
    public float maxHP;
    public float curHP;
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
        Grade = 1;

        maxHP = float.Parse(data["Health"]);
        curHP = maxHP;
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
    public BoxCollider col;
    public StatusBar statusBar;
    public GameObject target;
    public Vector3 pos;
    public bool isDead;
    public bool isBattle;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider>();
    }
    public void DetectTarget()
    {
        agent.isStopped = true;
        LayerMask layer;
        if (transform.CompareTag("Unit"))
        {
            layer = LayerMask.GetMask("Enemy");
        }
        else
        {
            layer = LayerMask.GetMask("Unit");
        }

        Collider[] objs = Physics.OverlapSphere(transform.position, 15f, layer);

        if (objs.Length == 0)
        {
            anim.Play("Win");
            return;
        }

        Array.Sort(objs, (a, b)
            => Vector3.Distance(transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(transform.position, b.transform.position)));

        target = objs[0].gameObject;
        anim.Play("Move");
    }
    public void CheckAttackRange()
    {
        if (target == null || target.activeSelf == false || target.GetComponent<Unit>().isDead)
        {
            anim.Play("Search");
        }
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float radius = 2 * Range;
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= radius)
        {
            transform.LookAt(target.transform.position);
            anim.SetFloat("AttackSpeed", AS);
            if(agent.enabled)
                agent.isStopped = true;
            if (!stateInfo.IsName("Attack"))
                anim.Play("Attack");
        }
        else
        {
            if(agent.enabled)
                agent.isStopped = false;
            agent.SetDestination(target.transform.position);
            if (!stateInfo.IsName("Move"))
                anim.Play("Move");
        }
    }
    public virtual void StartBattle()
    {
        pos = transform.position;
        isBattle = true;
        statusBar.gameObject.SetActive(true);
        statusBar.InitStatus(Grade ,maxHP, maxMP, startMP);
        agent.enabled = true;
        anim.Play("Search");
    }
    public virtual void ReturnIdle()
    {
        transform.position = pos;
        isBattle = false;
        isDead = false;
        col.enabled = true;
        statusBar.InitStatus(Grade ,maxHP, maxMP, startMP);
        statusBar.gameObject.SetActive(false);
        agent.enabled = false;
        anim.Play("Idle");
    }
    public void AutoAttack()
    {
        if (target.activeSelf == true && !target.GetComponent<Unit>().isDead)
        {
            target.GetComponent<Unit>().TakeDamage(AD);
            curMP += 10;
            curMP = Mathf.Clamp(curMP, 0, maxMP);
            statusBar.UpdateStatus(curHP, curMP);
        }
    }
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        float actualDamage = damage * (1 - (Armor / (Armor + 100)));
        curHP -= actualDamage;
        curHP =  Mathf.Clamp(curHP, 0, maxHP);
        curMP += 5;
        curMP = Mathf.Clamp(curMP, 0, maxMP);
        statusBar.UpdateStatus(curHP, curMP);
        if(curHP <= 0)
        {
            isDead = true;
            col.enabled = false;
            agent.enabled = false;
            statusBar.gameObject.SetActive(false);
            anim.Play("Dead");
        }
    }
    public void Dead()
    {
        gameObject.SetActive(false);
    }
}
