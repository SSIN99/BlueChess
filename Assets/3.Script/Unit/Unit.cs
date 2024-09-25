using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    #region Info
    [Header("Static")]
    public int No;
    public string Name;
    public int Origin;
    public int Class;
    public float Range;
    public float Speed;
    
    [Header("Not Static")]
    private int cost;
    public int Cost
    {
        get { return cost; }
        private set
        {
            cost = value;
            OnCostChanged?.Invoke();
        }
    }
    public event Action OnCostChanged;
    private int grade;
    public int Grade
    {
        get { return grade; }
        private set
        {
            grade = value;
            OnGradeChanged?.Invoke();
        }
    }
    public event Action OnGradeChanged;
    private float maxHp;
    public float MaxHp
    {
        get { return maxHp; }
        private set
        {
            maxHp = value;
            OnMaxHpChanged?.Invoke();
        }
    }
    public event Action OnMaxHpChanged;
    private float curHp;
    public float CurHp
    {
        get { return curHp; }
        private set
        {
            curHp = value;
            OnCurHpChanged?.Invoke();
        }
    }
    public event Action OnCurHpChanged;
    private int maxMp;
    public int MaxMp
    {
        get { return maxMp; }
        private set
        {
            maxMp = value;
            OnMaxMpChanged?.Invoke();
        }
    }
    public event Action OnMaxMpChanged;
    private int startMp;
    public int StartMp
    {
        get { return startMp; }
        private set
        {
            startMp = value;
            OnStartMpChanged?.Invoke();
        }
    }
    public event Action OnStartMpChanged;
    private int curMp;
    public int CurMp
    {
        get { return curMp; }
        private set
        {
            curMp = value;
            OnCurMpChanged?.Invoke();
        }
    }
    public event Action OnCurMpChanged;
    private int ad;
    public int AD
    {
        get { return ad; }
        private set
        {
            ad = value;
            OnADChanged?.Invoke();
        }
    }
    public event Action OnADChanged;
    private int ap;
    public int AP
    {
        get { return ap; }
        private set
        {
            ap = value;
            OnAPChanged?.Invoke();
        }
    }
    public event Action OnAPChanged;
    private float attackSpeed;
    public float AS
    {
        get { return attackSpeed; }
        private set
        {
            attackSpeed = value;
            OnASChanged?.Invoke();
        }
    }
    public event Action OnASChanged;
    private float critRatio;
    public float CritRatio
    {
        get { return critRatio; }
        private set
        {
            critRatio = value;
            OnCRChanged?.Invoke();
        }
    }
    public event Action OnCRChanged;
    private float critDamage;
    public float CritDamage
    {
        get { return critDamage; }
        private set
        {
            critDamage = value;
            OnCDChanged?.Invoke();
        }
    }
    public event Action OnCDChanged;
    private float armor;
    public float Armor
    {
        get { return armor; }
        private set
        {
            armor = value;
            OnArmorChanged?.Invoke();
        }
    }
    public event Action OnArmorChanged;
    private float resist;
    public float Resist
    {
        get { return resist; }
        private set
        {
            resist = value;
            OnResistChanged?.Invoke();
        }
    }
    public event Action OnResistChanged;
    private float curShield;
    public float CurShield
    {
        get { return curShield; }
        private set
        {
            curShield = value;
            OnCurShieldChanged?.Invoke();
        }
    }
    public event Action OnCurShieldChanged;
    private float maxShield;
    public float MaxShield
    {
        get { return maxShield; }
        private set
        {
            maxShield = value;
            OnMaxShieldChanged?.Invoke();
        }
    }
    public event Action OnMaxShieldChanged;
    public float lifeSteelRatio;
    #endregion

    #region Etc
    [Header("Etc")]
    public Info info;
    public TextPrinter textPrinter;
    public NavMeshAgent agent;
    public Animator anim;
    public BoxCollider col;
    public GameObject target;
    public Vector3 pos;
    public Quaternion rot;
    public float moveSpeed;
    public float rotSpeed;
    private bool isDead;
    private bool isBattle;
    public bool IsDead
    {
        get { return isDead; }
        private set
        {
            isDead = value;
            if (isDead)
            {
                col.enabled = false;
                agent.enabled = false;
                anim.Play("Dead");
                OnDead?.Invoke();
            }
        }
    }
    public bool IsBattle
    {
        get { return isBattle; }
        set
        {
            isBattle = value;
            if (isBattle)
            {
                pos = transform.position;
                rot = transform.rotation;
                agent.enabled = true;
                anim.Play("Search");
                OnBattleStart?.Invoke();
            }
            else
            {
                IsDead = false;
                transform.position = pos;
                transform.rotation = rot;
                agent.enabled = false;
                col.enabled = true;
                ResetStat();
                anim.Play("Idle");
                OnIdleReturn?.Invoke();
            }
        }
    }
    public event Action OnDead;
    public event Action OnBattleStart;
    public event Action OnIdleReturn;
    #endregion

    protected virtual void Start()
    {
        info = GameObject.FindGameObjectWithTag("Info").GetComponent<Info>();
        textPrinter = GameObject.FindGameObjectWithTag("Damage").GetComponent<TextPrinter>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider>();
        moveSpeed = 3f;
        rotSpeed = 10f;
        agent.speed = moveSpeed;
    }
    public void InitInfo(Dictionary<string, string> data)
    {
        No = int.Parse(data["No"]);
        Name = data["Name"];
        Origin = int.Parse(data["Origin"]);
        Class = int.Parse(data["Class"]);
        Range = float.Parse(data["Range"]);
        Speed = 3f;

        Cost = int.Parse(data["Cost"]);
        Grade = 1;
        MaxHp = float.Parse(data["Health"]);
        CurHp = MaxHp;
        MaxMp = int.Parse(data["MaxMP"]);
        StartMp = int.Parse(data["StartMP"]);
        CurMp = StartMp;
        AD = int.Parse(data["AD"]);
        AP = 100;
        AS = float.Parse(data["AS"]);
        CritRatio = 25f;
        CritDamage = 150f;
        Armor = float.Parse(data["Armor"]);
        Resist = float.Parse(data["Resistance"]);
        MaxShield = 0;
        CurShield = maxShield;
        lifeSteelRatio = 0;

        isDead = false;
        isBattle = false;

        originRank = 0;
        classRank = 0;
    }
    public void ResetStat()
    {
        CurHp = MaxHp;
        CurMp = StartMp;
        CurShield = 0;
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
            layer = LayerMask.GetMask("Field");
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
    public void CheckTargetDead()
    {
        if (target == null || target.activeSelf == false || target.GetComponent<Unit>().IsDead)
        {
            anim.Play("Search");
        }
    }
    public void CheckAttackRange()
    {
        CheckTargetDead();
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float radius = 2 * Range;
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= radius)
        {
            if(agent.enabled)
                agent.isStopped = true;
            anim.SetFloat("AttackSpeed", AS);
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
    public void LookAtTarget()
    {
        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0;  

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }
    }
    public void AutoAttack()
    {
        if (target.activeSelf == true && !target.GetComponent<Unit>().IsDead)
        {
            float finalDamage;
            float rand = Random.Range(0f, 100f);
            bool isCritical;
            if (rand < critRatio)
            {
                finalDamage = ad * (critDamage / 100f);
                isCritical = true;
            }
            else
            {
                finalDamage = ad;
                isCritical = false;
            }
            target.GetComponent<Unit>().TakeDamage(this ,finalDamage, isCritical);
            curMp += 10;
            CurMp = Mathf.Clamp(curMp, 0, MaxMp);
        }
    }
    public void TakeDamage(Unit attacker ,float damage, bool crit)
    {
        if (IsDead) return;

        float actualDamage = damage * (1f - (armor / (armor + 100f)));
        actualDamage = Mathf.Round(actualDamage);
        if(curShield > 0)
        {
            CurShield -= actualDamage;
            if(curShield < 0)
            {
                curHp += curShield;
                CurHp = Mathf.Clamp(curHp, 0, MaxHp);
            }
        }
        else
        {
            curHp -= actualDamage;
            CurHp =  Mathf.Clamp(curHp, 0, MaxHp);
        }
        attacker.LifeSteel(actualDamage);
        curMp += 5;
        CurMp = Mathf.Clamp(curMp, 0, MaxMp);
        if(CurHp <= 0)
        {
            IsDead = true;
        }
        textPrinter.PrintText(actualDamage, gameObject, crit, TextType.Attack);
    }
    public void GetShield(float amount)
    {
        MaxShield = amount;
    }
    public void LifeSteel(float damage)
    {
        if (lifeSteelRatio <= 0 || 
            curHp == maxHp) return;
        float steelAmount = Mathf.Round(damage * (lifeSteelRatio / 100f));
        curHp += steelAmount;
        CurHp = Mathf.Clamp(curHp, 0, maxHp);
        textPrinter.PrintText(steelAmount, gameObject, false, TextType.Heal);
    }
    public void Dead()
    {
        gameObject.SetActive(false);
    }
    #region Trait
    private int originRank;
    private int classRank;
    public void ResetTrait()
    {
        for(int i = 0; i < info.Traits.Count; i++)
        {
            UpdateTrait(i, 0);
        }
    }
    private int CheckTraitRank(int no, int count)
    {
        if (count < int.Parse(info.Traits[no]["Rank1"]))
        {
            return 0;
        }
        else if (count >= int.Parse(info.Traits[no]["Rank1"]) &&
            count < int.Parse(info.Traits[no]["Rank2"]))
        {
            return 1;
        }
        else if (count >= int.Parse(info.Traits[no]["Rank2"]) &&
            count < int.Parse(info.Traits[no]["Rank3"]))
        {
            return 2;
        }
        else if (count >= int.Parse(info.Traits[no]["Rank3"]) &&
           count < int.Parse(info.Traits[no]["Rank4"]))
        {
            return 3;
        }
        else
        {
            return 4;
        }
    }
    public void UpdateTrait(int no, int count)
    {
        int rank = CheckTraitRank(no, count);
        if(no < 10)
        {
            if (originRank == rank) return;
        }
        else
        {
            if (classRank == rank) return;
        }
        switch (no)
        {
            case 0: //�ƺ񵵽�
                SetTrait_0(rank);
                break;
            case 1: //���
                break;
            case 2: //�з��Ͼ�
                break;
            case 3: //Ʈ����Ƽ
                break;
            case 4: //��;���
                break;
            case 5: //���ذ�
                break;
            case 6: //��������
                break;
            case 7: //��Ű��
                break;
            case 8: //SRT
                break;
            case 9: //�Ƹ��콺
                break;
            case 10: //����
                break;
            case 11: //������
                break;
            case 12: //������
                break;
            case 13: //������
                break;
            case 14: //��ȣ��
                break;
            case 15: //�ź����
                break;
            case 16: //�ο��
                break;
            case 17: //������
                break;
            case 18: //���ݼ�
                break;
            case 19: //������
                break;
            case 20: //���ı� 
                break;
            case 21: //�ı���
                break;
            case 22: //å��
                break;
        }
    }
    public void SetTrait_0(int rank) //�ƺ񵵽�
    {
        switch (originRank)
        {
            case 0:
                break;
            case 1:
                lifeSteelRatio -= 30f;
                break;
            case 2:
                lifeSteelRatio -= 60f;
                break;
            case 3:
                break;
            case 4:
                break;
        }
        switch (rank)
        {
            case 0:
                lifeSteelRatio += 0;
                break;
            case 1:
                lifeSteelRatio += 30f;
                break;
            case 2:
                lifeSteelRatio += 60f;
                break;
            case 3:
                break;
            case 4:
                break;
        }
        originRank = rank;
    }
    #endregion
}
