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
            float differ = value - maxHp;
            maxHp = value;
            OnMaxHpChanged?.Invoke();
            CurHp += differ;
        }
    }
    public event Action OnMaxHpChanged;
    private float curHp;
    public float CurHp
    {
        get { return curHp; }
        private set
        {
            float hp = Mathf.Clamp(value, 0, maxHp);
            curHp = hp;
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
            if(startMp > maxMp)
            {
                StartMp = maxMp;
            }
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
            int differ = value - startMp;
            startMp = value;
            OnStartMpChanged?.Invoke();
            CurMp = differ;
        }
    }
    public event Action OnStartMpChanged;
    private int curMp;
    public int CurMp
    {
        get { return curMp; }
        private set
        {
            int mp = Mathf.Clamp(value, 0, maxMp);
            curMp = mp;
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
            CurShield = maxShield;
            OnMaxShieldChanged?.Invoke();
        }
    }
    public event Action OnMaxShieldChanged;
    public float lifeSteel;
    public float increasedDamage;

    private float InitHP;
    private float InitAD;
    private float InitAS;

    private float tempHP;
    private float tempAD;
    private float tempAP;
    private float tempAS;
    private float tempCR;
    private float tempCD;
    private float tempArmor;
    private float tempResist;
    private float tempAvoid;
    private float tempLS;
    private float tempID;

    public float AllDealAmount;
    public event Action OnDealAmountChanged;
    #endregion

    #region Etc
    [Header("Etc")]
    public Info info;
    public Player player;
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
                RecordStat();
                pos = transform.position;
                rot = transform.rotation;
                agent.enabled = true;
                anim.Play("Search");
                OnBattleStart?.Invoke();

                InvokeRepeating("IncreaseAsTrait2", 0f, 4f);
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

                CancelInvoke("IncreaseAsTrait2");
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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
        increasedDamage = 0;
        manaRegeneration = 10;

        InitAD = ad;
        InitHP = maxHp;
        InitAS = attackSpeed;
    }
    public void BeSold()
    {
        OnBeSold?.Invoke();
    }
    public void RecordStat()
    {
        tempHP = maxHp;
        tempAD = ad;
        tempAP = ap;
        tempCR = critRatio;
        tempCD = critDamage;
        tempArmor = armor;
        tempResist = resist;
        tempAS = attackSpeed;
        tempAvoid = avoid;
        tempLS = lifeSteel;
        tempID = increasedDamage;

        AllDealAmount = 0;
    }
    public void ResetStat()
    {
        CurHp = tempHP;
        CurMp = StartMp;
        CurShield = 0;
        AD = tempAD;
        AP = tempAP;
        CritRatio = tempCR;
        CritDamage = tempCD;
        Armor = tempArmor;
        Resist = tempResist;
        AS = tempAS;
        Avoid = tempAvoid;
        lifeSteel = tempLS;
        increasedDamage = tempID;
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
            if (agent.enabled)
                agent.isStopped = true;
            anim.SetFloat("AttackSpeed", AS);
            if (!stateInfo.IsName("Attack"))
                anim.Play("Attack");
        }
        else
        {
            if (agent.enabled)
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
        if (!target.GetComponent<Unit>().IsDead)
        {
            float finalDamage;
            bool isCritical;
            float rand = Random.Range(0f, 100f);
            if (rand < critRatio)
            {
                finalDamage = (ad * (critDamage / 100f)) * (1f + increasedDamage / 100f);
                isCritical = true;
            }
            else
            {
                finalDamage = ad * (1f + increasedDamage / 100f);
                isCritical = false;
            }
            target.GetComponent<Unit>().TakeDamage(this, finalDamage, isCritical);
            CurMp += manaRegeneration;

            OnAttackOccurred?.Invoke();
        }
    }
    public event Action OnAttackOccurred;
    public delegate void AttackSuccessHandler(Unit enemy);
    AttackSuccessHandler OnAttackSuccess;
    private float PanicDuration;
    private float elaspedTime;
    public void StartStun(float duration)
    {
        if (duration == 0) return;
        PanicDuration = duration;
        elaspedTime = 0f;
        agent.isStopped = true;
        anim.Play("Panic");
    }
    public void Stuned()
    {
        while(elaspedTime < PanicDuration)
        {
            elaspedTime += Time.deltaTime;
        }
        anim.Play("Search");
    }
    public void RecordDealAmount(float deal)
    {
        AllDealAmount += deal;
        OnDealAmountChanged?.Invoke();
    }
    public void TakeDamage(Unit attacker, float damage, bool crit)
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
        if (curShield > 0)
        {
            CurShield -= actualDamage;
            if (curShield < 0)
            {
                CurHp += curShield;
            }
        }
        else
        {
            CurHp -= actualDamage;
        }
        attacker.LifeSteel(actualDamage);
        attacker.RecordDealAmount(actualDamage);
        CurMp += 5;
        if (crit)
        {
            textPrinter.PrintText(actualDamage.ToString(), transform.position, TextType.Crit);
        }
        else
        {
            textPrinter.PrintText(actualDamage.ToString(), transform.position, TextType.Attack);
        }
        sfxPrinter.PrintHitFx(transform.position);
        if (CurHp <= 0)
        {
            IsDead = true;
            return;
        }
        attacker.OnAttackSuccess?.Invoke(this);
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
    private int trait2Rank;
    private int trait6Rank;
    private int trait10Rank;
    private int trait14Rank;
    private int manaRegeneration;
    public void PrintTraitEffect()
    {
        sfxPrinter.PrintTraitFx(transform);
    }
    public void UpdateTrait(int no, int old, int rank)
    {
        switch (no)
        {
            case 0:
                SetTrait_0(old, rank);
                break;
            case 1:
                if (Origin != 1) return;
                SetTrait_1(old, rank);
                break;
            case 2:
                SetTrait_2(old, rank);
                break;
            case 3:
                if (Origin != 3) return;
                SetTrait_3(old, rank);
                break;
            case 4:
                if (Origin != 4) return;
                SetTrait_4(old, rank);
                break;
            case 5:
                if (Origin != 5) return;
                SetTrait_5(old, rank);
                break;
            case 6:
                SetTrait_6(old, rank);
                break;
            case 7:
                if (Origin != 7) return;
                SetTrait_7(old, rank);
                break;
            case 8:
                if (Origin != 8) return;
                SetTrait_8(old, rank);
                break;
            case 9:
                if (Origin != 9) return;
                SetTrait_9(old, rank);
                break;
            case 10:
                if (Class != 10) return;
                SetTrait_10(old, rank);
                break;
            case 11:
                if (Class != 11) return;
                SetTrait_11(old, rank);
                break;
            case 12:
                SetTrait_12(old, rank);
                break;
            case 13:
                if (Class != 13) return;
                SetTrait_13(old, rank);
                break;
            case 14:
                if (Class != 14) return;
                SetTrait_14(old, rank);
                break;
            case 15:
                SetTrait_15(old, rank);
                break;
            case 16:
                if (Class != 16) return;
                SetTrait_16(old, rank);
                break;
            case 17:
                if (Class != 17) return;
                SetTrait_17(old, rank);
                break;
            case 18:
                if (Class != 18) return;
                SetTrait_18(old, rank);
                break;
            case 19:
                if (Class != 19) return;
                SetTrait_19(old, rank);
                break;
            case 20:
                if (Class != 20) return;
                SetTrait_20(old, rank);
                break;
            case 21:
                if (Class != 21) return;
                SetTrait_21(old, rank);
                break;
            case 22:
                SetTrait_22(old, rank);
                break;
        }
    }
    public void SetTrait_0(int old, int rank) //아비도스
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
    public void SetTrait_1(int old, int rank) //게헨나
    {
        switch (old)
        {
            case 0:
                break;
            case 1:
                AD -= 20f;
                AP -= 20f;
                break;
            case 2:
                AD -= 35f;
                AP -= 35f;
                break;
            case 3:
                AD -= 60f;
                AP -= 60f;
                break;
            case 4:
                break;
        }
        switch (rank)
        {
            case 0:
                break;
            case 1:
                AD += 20f;
                AP += 20f;
                break;
            case 2:
                AD += 35f;
                AP += 35f;
                break;
            case 3:
                AD += 60f;
                AP += 60f;
                break;
            case 4:
                break;
        }
    }
    public void SetTrait_2(int old, int rank) //밀레니엄
    {
        trait2Rank = rank;
    }
    public void IncreaseAsTrait2()
    {
        switch (trait2Rank)
        {
            case 1:
                AS += Mathf.Round(attackSpeed * 0.15f);
                break;
            case 2:
                AS += Mathf.Round(attackSpeed * 0.35f);
                break;
            case 3:
                AS += Mathf.Round(attackSpeed * 0.65f);
                break;
            case 4:
                AS += Mathf.Round(attackSpeed * 0.90f);
                break;
            default:
                return;
        }
    }
    public void SetTrait_3(int old, int rank) //트리니티
    {
        switch (old)
        {
            case 0:
                break;
            case 1:
                Armor -= 20f;
                Resist -= 20f;
                break;
            case 2:
                Armor -= 50f;
                Resist -= 50f;
                break;
            case 3:
                break;
            case 4:
                break;
        }
        switch (rank)
        {
            case 0:
                break;
            case 1:
                Armor += 20f;
                Resist += 20f;
                break;
            case 2:
                Armor += 50f;
                Resist += 50f;
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
    public void SetTrait_4(int old, int rank) //백귀야행
    {
        switch (old)
        {
            case 0:
                break;
            case 1:
                manaRegeneration -= 4;
                break;
            case 2:
                manaRegeneration -= 7;
                break;
            case 3:
                manaRegeneration -= 10;
                break;
            case 4:
                break;
        }
        switch (rank)
        {
            case 0:
                break;
            case 1:
                manaRegeneration += 4;
                break;
            case 2:
                manaRegeneration += 7;
                break;
            case 3:
                manaRegeneration += 10;
                break;
            case 4:
                break;
        }
    }
    public void SetTrait_5(int old, int rank) //산해경
    {
        switch (old)
        {
            case 0:
                break;
            case 1:
                MaxHp -= 200f;
                AP -= 20f;
                break;
            case 2:
                MaxHp -= 200f;
                AP -= 20f;
                break;
            case 3:
                MaxHp -= 200f;
                AP -= 20f;
                break;
            case 4:
                break;
        }
        switch (rank)
        {
            case 0:
                break;
            case 1:
                MaxHp += 200f;
                AP += 20f;
                break;
            case 2:
                MaxHp += 300f;
                AP += 30f;
                break;
            case 3:
                MaxHp += 400f;
                AP += 40f;
                break;
            case 4:
                break;
        }
    }
    public void SetTrait_6(int old, int rank) //레드윈터
    {
        OnDead -= deadEventTrait6;
        trait6Rank = rank;
        if(rank != 0)
        {
            OnDead += deadEventTrait6;
        }
    }
    public void deadEventTrait6()
    {
        for (int i = 0; i < player.fieldList.Count; i++)
        {
            if (!player.fieldList[i].IsDead &&
                player.fieldList[i].Origin == 6)
            {
                player.fieldList[i].IncreaseAdApTrait6();
            }
        }
    }
    public void IncreaseAdApTrait6()
    {
        switch (trait6Rank)
        {
            case 1:
                AD += 8;
                AP += 8;
                break;
            case 2:
                AD += 18;
                AP += 18;
                break;
            case 3:
                AD += 28;
                AP += 28;
                break;
        }
    }
    public void SetTrait_7(int old, int rank) //발키리
    {
        if(rank == 1)
        {
            OnAttackSuccess += StunAttackTrait7;
        }
        else
        {
            OnAttackSuccess -= StunAttackTrait7;
        }
    }
    public void StunAttackTrait7(Unit enemy)
    {
        float rand = Random.Range(0, 100);
        if(rand < 30)
        {
            enemy.StartStun(1.5f);
        }
    }
    public void SetTrait_8(int old, int rank) //SRT
    {
        if (rank == 1)
        {
            MaxMp -= 30;
        }
        else
        {
            MaxMp += 30;
        }
    }
    public void SetTrait_9(int old, int rank) //아리우스
    {
        switch (old)
        {
            case 0:
                break;
            case 1:
                increasedDamage -= 20f;
                break;
            case 2:
                increasedDamage -= 35f;
                break;
            case 3:
       
                break;
            case 4:
                break;
        }
        switch (rank)
        {
            case 0:
                break;
            case 1:
                increasedDamage += 20f;
                break;
            case 2:
                increasedDamage += 35f;
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
    public void SetTrait_10(int old, int rank) //정찰대
    {
        OnAttackOccurred -= ExtraAttackTrait10;
        trait10Rank = rank;
        if(rank != 0)
        {
            OnAttackOccurred += ExtraAttackTrait10;
        }
    }
    public void ExtraAttackTrait10()
    {
        float rand = Random.Range(0, 100);
        float ratio = 0;
        switch (trait10Rank)
        {
            case 1:
                ratio = 30;
                break;
            case 2:
                ratio = 45;
                break;
            case 3:
                ratio = 65;
                break;
        }
        if(rand < ratio)
        {
            if (!target.GetComponent<Unit>().IsDead)
            {
                float finalDamage;
                bool isCritical;
                rand = Random.Range(0f, 100f);
                if (rand < critRatio)
                {
                    finalDamage = (ad * (critDamage / 100f)) * (1f + increasedDamage / 100f);
                    isCritical = true;
                }
                else
                {
                    finalDamage = ad * (1f + increasedDamage / 100f);
                    isCritical = false;
                }
                target.GetComponent<Unit>().TakeDamage(this, finalDamage, isCritical);
            }
        }
    }
    public void SetTrait_11(int old, int rank) //수집가
    {
        OnAttackSuccess -= BurnOutManaTrait11;
        if(rank != 0)
        {
            OnAttackSuccess += BurnOutManaTrait11;
        }
    }
    public void BurnOutManaTrait11(Unit enemy) 
    {
        enemy.DecreaseMana(5);
    }
    public void DecreaseMana(int amout)
    {
        CurMp -= amout;
    }
    public void SetTrait_12(int old, int rank) //마법사
    {
        switch (old)
        {
            case 0:
                break;
            case 1:
                AP -= 20f;
                break;
            case 2:
                AP -= 40f;
                break;
            case 3:
                AP -= 60f;
                break;
            case 4:
                break;
        }
        switch (rank)
        {
            case 0:
                break;
            case 1:
                AP += 20f;
                break;
            case 2:
                AP += 40f;
                break;
            case 3:
                AP += 60f;
                break;
            case 4:
                break;
        }
    }
    public void SetTrait_13(int old, int rank) //선봉대
    {
        switch (old)
        {
            case 0:
                break;
            case 1:
                Armor -= 70f;
                break;
            case 2:
                Armor -= 150f;
                break;
            case 3:
                Armor -= 350f;
                break;
            case 4:
                break;
        }
        switch (rank)
        {
            case 0:
                break;
            case 1:
                Armor += 70f;
                break;
            case 2:
                Armor += 150f;
                break;
            case 3:
                Armor += 350f;
                break;
            case 4:
                break;
        }
    }
    public void SetTrait_14(int old, int rank) //수호자
    {
        OnBattleStart -= GetShieldTratit14;
        trait14Rank = rank;
        if(rank != 0)
        {
            OnBattleStart += GetShieldTratit14;
        }
    }
    public void GetShieldTratit14()
    {
        switch (trait14Rank)
        {
            case 1:
                GetShield(Mathf.Round(maxHp * 0.3f));
                break;
            case 2:
                GetShield(Mathf.Round(maxHp * 0.5f));
                break;
        }
    }
    public void SetTrait_15(int old, int rank) //마법사
    {
        switch (old)
        {
            case 0:
                break;
            case 1:
                Resist -= 50f;
                break;
            case 2:
                Resist -= 120f;
                break;
            case 3:
                break;
            case 4:
                break;
        }
        switch (rank)
        {
            case 0:
                break;
            case 1:
                Resist += 50f;
                break;
            case 2:
                Resist += 120f;
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
    public void SetTrait_16(int old, int rank) //싸움꾼
    {
        switch (old)
        {
            case 0:
                break;
            case 1:
                MaxHp -= 350f;
                break;
            case 2:
                MaxHp -= 600f;
                break;
            case 3:
                break;
            case 4:
                break;
        }
        switch (rank)
        {
            case 0:
                break;
            case 1:
                MaxHp += 350f;
                break;
            case 2:
                MaxHp += 600f;
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
    public void SetTrait_17(int old, int rank) //잠입자
    {
        switch (old)
        {
            case 0:
                break;
            case 1:
                CritRatio -= 20f;
                CritDamage -= 75f;
                break;
            case 2:
                CritRatio -= 40f;
                CritDamage -= 150f;
                break;
            case 3:
                break;
            case 4:
                break;
        }
        switch (rank)
        {
            case 0:
                break;
            case 1:
                CritRatio += 20f;
                CritDamage += 75f;
                break;
            case 2:
                CritRatio += 40f;
                CritDamage += 150f;
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
    public void SetTrait_18(int old, int rank) //저격수
    {
        switch (old)
        {
            case 0:
                break;
            case 1:
                CritRatio -= 15f;
                Range -= 1;
                break;
            case 2:
                CritRatio -= 30f;
                Range -= 2;
                break;
            case 3:
                break;
            case 4:
                break;
        }
        switch (rank)
        {
            case 0:
                break;
            case 1:
                CritRatio += 15f;
                Range += 1;
                break;
            case 2:
                CritRatio += 30f;
                Range += 2;
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
    public void SetTrait_19(int old, int rank) //총잡이
    {
        switch (old)
        {
            case 0:
                break;
            case 1:
                AS -= Mathf.Round(InitAS * 0.2f);
                break;
            case 2:
                AS -= Mathf.Round(InitAS * 0.5f);
                break;
            case 3:
                break;
            case 4:
                break;
        }
        switch (rank)
        {
            case 0:
                break;
            case 1:
                AS += Mathf.Round(InitAS * 0.2f);
                break;
            case 2:
                AS += Mathf.Round(InitAS * 0.5f);
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
    public void SetTrait_20(int old, int rank) //폭파광
    {
        switch (old)
        {
            case 0:
                break;
            case 1:
                increasedDamage -= 15;
                CritDamage -= 50;
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
        switch (rank)
        {
            case 0:
                break;
            case 1:
                increasedDamage += 15;
                CritDamage += 50;
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
    public void SetTrait_21(int old, int rank) //파괴자
    {
        OnAttackSuccess -= DestroyEnemy;
        if(rank != 0)
        {
            OnAttackSuccess += DestroyEnemy;
        }
    }
    public void DestroyEnemy(Unit enemy)
    {
        if(enemy.CurHp < Mathf.Round(enemy.CurHp * 0.15f))
        {
            enemy.Excution();
        }
    }
    public void Excution()
    {
        CurHp = 0;
        IsDead = true;
    }
    public void SetTrait_22(int old, int rank) //책사
    {
        if(rank == 1)
        {
            Avoid += 20f;
        }
        else
        {
            Avoid -= 20f;
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
        switch (item)
        {
            case 0:
                AD += 20f;
                break;
            case 1:
                AS += Mathf.Round(InitAS * 0.15f * 100f) / 100f;
                break;
            case 2:
                Avoid += 15f;
                break;
            case 3:
                MaxHp += 200f;
                break;
            case 4:
                Armor += 20f;
                break;
            case 5:
                Resist += 20f;
                break;
            case 6:
                StartMp += 15;
                break;
            case 7:
                AP += 20f;
                break;
            case 8:
                CritRatio += 20f;
                break;
        }
    }
    #endregion

    #region Grade
    public void GradeUp()
    {
        Grade += 1;
        transform.localScale += Vector3.one * 20f;
        float hp = Mathf.Round(InitHP * 0.8f);
        MaxHp += hp;
        InitHP += hp;
        float ad = Mathf.Round(InitAD * 0.8f);
        AD += ad;
        InitAD += ad;
        sfxPrinter.PrintGradeFx(transform, grade);
    }
    #endregion

    #region Skill
    public void CheckSkillUsable()
    {
        if (curMp == maxMp &&
            maxMp != 0)
        {
            CurMp = 0;
            anim.Play("Skill");
            OnSkillUsed?.Invoke();
        }
    }
    public event Action OnSkillUsed;
    public void SkillEffect()
    {

    }
    #endregion
}
