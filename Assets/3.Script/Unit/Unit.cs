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
    private float ad;
    public float AD
    {
        get { return ad; }
        private set
        {
            ad = value;
            OnADChanged?.Invoke();
        }
    }
    public event Action OnADChanged;
    private float ap;
    public float AP
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
    private float avoid;
    public float Avoid
    {
        get { return avoid; }
        private set
        {
            avoid = value;
            OnAvoidChanged?.Invoke();
        }
    }
    public event Action OnAvoidChanged;
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
    public float lifeSteel;
    #endregion

    #region Etc
    [Header("Etc")]
    public Info info;
    public TextPrinter textPrinter;
    public SFXPrinter sfxPrinter;
    public NavMeshAgent agent;
    public Animator anim;
    public BoxCollider col;
    public GameObject target;
    public Vector3 pos;
    public Quaternion rot;
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
                OnIdleReturn?.Invoke();
                transform.position = pos;
                transform.rotation = rot;
                agent.enabled = false;
                col.enabled = true;
                ResetStat();
                anim.Play("Idle");
            }
        }
    }
    public event Action OnDead;
    public event Action OnBattleStart;
    public event Action OnIdleReturn;
    public event Action OnBeSold;
    #endregion

    #region Action
    protected virtual void Start()
    {
        info = GameObject.FindGameObjectWithTag("Info").GetComponent<Info>();
        textPrinter = GameObject.FindGameObjectWithTag("Text").GetComponent<TextPrinter>();
        sfxPrinter = GameObject.FindGameObjectWithTag("SFX").GetComponent<SFXPrinter>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider>();
        rotSpeed = 10f;
        itemList = new List<int>();
    }
    public void InitInfo(Dictionary<string, string> data)
    {
        No = int.Parse(data["No"]);
        Name = data["Name"];
        Origin = int.Parse(data["Origin"]);
        Class = int.Parse(data["Class"]);
        Range = float.Parse(data["Range"]);

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
        avoid = 0;
        MaxShield = 0;
        CurShield = maxShield;
        lifeSteel = 0;
    }
    public void BeSold()
    {
        OnBeSold?.Invoke();
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
            bool isCritical;
            float rand = Random.Range(0f, 100f);
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

        float rand = Random.Range(0f, 100f);
        if (rand < avoid)
        {
            textPrinter.PrintText(string.Empty, transform.position, TextType.Avoid);
            return;
        }
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
        if (crit)
        {
            textPrinter.PrintText(actualDamage.ToString(), transform.position, TextType.Crit);
        }
        else
        {
            textPrinter.PrintText(actualDamage.ToString(), transform.position, TextType.Attack);
        }
        sfxPrinter.PrintHitFx(transform.position);
    }
    public void GetShield(float amount)
    {
        MaxShield = amount;
    }
    public void LifeSteel(float damage)
    {
        if (lifeSteel <= 0 || 
            curHp == maxHp) return;
        float steelAmount = Mathf.Round(damage * (lifeSteel / 100f));
        curHp += steelAmount;
        CurHp = Mathf.Clamp(curHp, 0, maxHp);
        textPrinter.PrintText(steelAmount.ToString(), transform.position, TextType.Heal);
    }
    public void Dead()
    {
        gameObject.SetActive(false);
    }
    #endregion

    #region Trait
    public void UpdateTrait(int no, int old, int rank)
    {
        switch (no)
        {
            case 0:
                SetTrait_0(old, rank);
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:
                break;
            case 13:
                break;
            case 14:
                break;
            case 15:
                break;
            case 16:
                break;
            case 17:
                break;
            case 18:
                break;
            case 19:
                break;
            case 20:
                break;
            case 21:
                break;
            case 22:
                break;
            default:
                break;
        }
    }
    public void SetTrait_0(int old ,int rank) //아비도스
    {
        switch (old)
        {
            case 0:
                break;
            case 1:
                lifeSteel -= 30f;
                break;
            case 2:
                lifeSteel -= 60f;
                break;
            case 3:
                break;
            case 4:
                break;
        }
        switch (rank)
        {
            case 0:
                lifeSteel += 0;
                break;
            case 1:
                lifeSteel += 30f;
                break;
            case 2:
                lifeSteel += 60f;
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
    #endregion

    #region Item
    public List<int> itemList;
    public bool IsItemFull => itemList.Count == 3;
    public int ItemCount => itemList.Count;
    public void EquipItem(int item)
    {
        itemList.Add(item);
        SetItemEffect(item);
        OnItemEquiped?.Invoke();
    }
    public event Action OnItemEquiped;
    private void SetItemEffect(int item)
    {

    }
    #endregion

    #region Grade
    public void GradeUp()
    {
        Grade += 1;
        transform.localScale += Vector3.one * 20f;
        MaxHp += Mathf.Round(maxHp * 0.8f);
        CurHp = maxHp;
        AD += Mathf.Round(ad * 0.8f);
        sfxPrinter.PrintGradeFx(transform, grade);
    }
    #endregion
}
