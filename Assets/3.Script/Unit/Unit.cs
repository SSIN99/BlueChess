using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    #region status
    public int No;
    public string Name;
    public int Cost;
    public int Origin;
    public int Class;
    public int Grade;

    private float maxHealth; //최대 체력
    private float curHealth; //현재 체력
    private int maxMana; //최대 마나
    private int startMana; //시작 마나
    private int curMana; //현재 마나
    private int manaRegeneration; //마나 회복량
    private bool isManaBan; //마나 획득?
    private float attackDamage; //공격력
    private float abilityPower; //주문력
    private float attackSpeed; //공격속도
    private float critRatio; //치명확률 
    private float critDamage; //치명피해
    private float armor; //방어력
    private float resist;//마법저항
    private float avoid; //회피율 
    private float maxShield; //최대 쉴드
    private float curShield; //남은 쉴드
    public float range; //사거리
    public float lifeSteel; //생명력 흡수
    public float increasedDeal; //피해량 증가
    public float dealAmount; //전투 딜량
    public bool isPassive; //패시브 스킬?
    private float adRatio = 0; //ad 계수
    private float apRatio = 0; //ap 계수
    private int skillAttackCount = 1; //스킬 공격 횟수
    private float stunDuration; //스턴 지속시간
    private float curStunTime; //현재 스턴시간
    public bool isOnField; //필드배치?
    private bool isBattle; //전투중?
    private bool isDead; //죽음?
    public bool isEnemy; //적?
    public GameObject target; //공격대상

    private float InitHP; //초기 체력 
    private float InitAD; //초기 공격력
    private float InitAS; //초기 공격속도

    private float tempHP;
    private float tempAD;
    private float tempAP;
    private float tempAS;
    private float tempCR;
    private float tempCD;
    private float tempArmor;
    private float tempResist;
    private float tempAvoid;
    private Vector3 tempPos;

    private int trait2Rank;
    private int trait6Rank;
    private int trait10Rank;
    private int trait14Rank;

    //변신 유닛 전용 
    [SerializeField] private GameObject defaultBody;
    [SerializeField] private GameObject tranformationBody;

    public UnitManager unitManager;
    public VFXPrinter vfxPrinter;
    public Animator anim;
    public BoxCollider col;
    public NavMeshAgent agent;
    public List<int> itemList;
    public bool IsItemFull => itemList.Count == 3;
    public int ItemCount => itemList.Count;
    #endregion

    #region property
    public bool IsDead
    {
        get { return isDead; }
        private set
        {
            isDead = value;
            if (isDead)
            {
                anim.Play("Dead");
                col.enabled = false;
                agent.enabled = false;
                OnDead?.Invoke();
                CancelInvoke("IncreaseAsTrait2");
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
                RecordState();
                agent.enabled = true;
                anim.Play("Search");
                OnBattleStart?.Invoke();

                if(trait2Rank != 0)
                    InvokeRepeating("IncreaseAsTrait2", 0f, 4f);
            }
            else
            {
                IsDead = false;
                OnIdleReturn?.Invoke();
                agent.enabled = false;
                col.enabled = true;
                ResetState();
                anim.Play("Idle");

                CancelInvoke("IncreaseAsTrait2");
            }
        }
    }
    public float MaxHp
    {
        get { return maxHealth; }
        private set
        {
            float increase = value - maxHealth;
            maxHealth = value;
            OnMaxHpChanged?.Invoke();
            CurHp += increase;
        }
    }
    public float CurHp
    {
        get { return curHealth; }
        private set
        {
            float hp = Mathf.Clamp(value, 0, maxHealth);
            curHealth = hp;
            OnCurHpChanged?.Invoke();
        }
    }
    public int MaxMp
    {
        get { return maxMana; }
        private set
        {
            maxMana = value;
            if (startMana > maxMana)
            {
                StartMp = maxMana;
            }
            OnMaxMpChanged?.Invoke();
        }
    }
    public int StartMp
    {
        get { return startMana; }
        private set
        {
            int increase = value - startMana;
            startMana = value;
            OnStartMpChanged?.Invoke();
            CurMp = increase;
        }
    }
    public int CurMp
    {
        get { return curMana; }
        private set
        {
            int mp = Mathf.Clamp(value, 0, maxMana);
            curMana = mp;
            OnCurMpChanged?.Invoke();
        }
    }
    public float AD
    {
        get { return attackDamage; }
        private set
        {
            attackDamage = value;
            OnADChanged?.Invoke();
        }
    }
    public float AP
    {
        get { return abilityPower; }
        private set
        {
            abilityPower = value;
            OnAPChanged?.Invoke();
        }
    }
    public float AS
    {
        get { return attackSpeed; }
        private set
        {
            attackSpeed = value;
            OnASChanged?.Invoke();
        }
    }
    public float CritRatio
    {
        get { return critRatio; }
        private set
        {
            float cr = Mathf.Clamp(value, 0, 100f);
            critRatio = cr;
            OnCRChanged?.Invoke();
        }
    }
    public float CritDamage
    {
        get { return critDamage; }
        private set
        {
            critDamage = value;
            OnCDChanged?.Invoke();
        }
    }
    public float Armor
    {
        get { return armor; }
        private set
        {
            armor = value;
            OnArmorChanged?.Invoke();
        }
    }
    public float Resist
    {
        get { return resist; }
        private set
        {
            resist = value;
            OnResistChanged?.Invoke();
        }
    }
    public float Avoid
    {
        get { return avoid; }
        private set
        {
            avoid = value;
            OnAvoidChanged?.Invoke();
        }
    }
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
    public float CurShield
    {
        get { return curShield; }
        private set
        {
            curShield = value;
            OnCurShieldChanged?.Invoke();
        }
    }
    #endregion

    #region event
    public event Action OnMaxHpChanged;
    public event Action OnCurHpChanged;
    public event Action OnMaxMpChanged;
    public event Action OnStartMpChanged;
    public event Action OnCurMpChanged;
    public event Action OnADChanged;
    public event Action OnAPChanged;
    public event Action OnASChanged;
    public event Action OnCRChanged;
    public event Action OnCDChanged;
    public event Action OnArmorChanged;
    public event Action OnResistChanged;
    public event Action OnAvoidChanged;
    public event Action OnMaxShieldChanged;
    public event Action OnCurShieldChanged;

    public event Action OnGradeUp;
    public event Action OnBeSold;
    public event Action OnItemEquiped;

    public event Action OnDead;
    public event Action OnBattleStart;
    public event Action OnIdleReturn;

    public event Action OnSkillUsed;
    public event Action OnRunAutoAttack;
    public delegate void AttackSuccessHandler(Unit enemy);
    AttackSuccessHandler OnSuccessAutoAttack;
    public event Action OnDealAmountChanged;
    #endregion

    private void Awake()
    {
        unitManager = GameObject.FindGameObjectWithTag("Player").GetComponent<UnitManager>();
        vfxPrinter = GameObject.FindGameObjectWithTag("VFX").GetComponent<VFXPrinter>();
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider>();
        agent = GetComponent<NavMeshAgent>();
        itemList = new List<int>();
    }
    public void InitInfo(Dictionary<string, string> data)
    {
        No = int.Parse(data["No"]);
        Name = data["Name"];
        Origin = int.Parse(data["Origin"]);
        Class = int.Parse(data["Class"]);
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
        Avoid = 0f;
        range = float.Parse(data["Range"]);
        MaxShield = 0;
        CurShield = maxShield;
        lifeSteel = 0;
        increasedDeal = 0;
        manaRegeneration = 10;

        InitHP = maxHealth;
        InitAD = attackDamage;
        InitAS = attackSpeed;

        if (isPassive)
            ActiveSkill();
    }
    public void SetArrangeState(bool isField)
    {
        if (isField)
        {
            isOnField = true;
            transform.rotation = Quaternion.identity;
            gameObject.layer = LayerMask.NameToLayer("Field");
        }
        else
        {
            isOnField = false;
            transform.rotation = Quaternion.Euler(0, 135f, 0f);
            gameObject.layer = LayerMask.NameToLayer("Bench");
        }
    }
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
        vfxPrinter.PrintGradeFX(transform, Grade);
        OnGradeUp?.Invoke();
    }
    public void BeSold()
    {
        if (isOnField)
        {
            unitManager.RemoveField(this);
        }
        else
        {
            unitManager.RemoveBench(this);
        }
        GetComponent<Arrangement>().LeaveTile();
        OnBeSold?.Invoke();
    }
    public void EquipItem(int item)
    {
        itemList.Add(item);
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
        OnItemEquiped?.Invoke();
    }
    public void RecordState()
    {
        tempHP = maxHealth;
        tempAD = attackDamage;
        tempAP = abilityPower;
        tempCR = critRatio;
        tempCD = critDamage;
        tempArmor = armor;
        tempResist = resist;
        tempAS = attackSpeed;
        tempAvoid = avoid;
        tempPos = transform.position;
    }
    public void ResetState()
    {
        CurHp = tempHP;
        CurMp = startMana;
        CurShield = 0;
        AD = tempAD;
        AP = tempAP;
        CritRatio = tempCR;
        CritDamage = tempCD;
        Armor = tempArmor;
        Resist = tempResist;
        AS = tempAS;
        Avoid = tempAvoid;
        dealAmount = 0;
        isManaBan = false;
        transform.position = tempPos;
        transform.rotation = isOnField ? Quaternion.identity : Quaternion.Euler(0f, 135f, 0f);
    }

    public void DetectTarget()
    {
        agent.isStopped = true;

        LayerMask layer = transform.CompareTag("Unit") ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("Field");
        Collider[] units = Physics.OverlapSphere(transform.position, 15f, layer);

        if (units.Length == 0)
        {
            transform.rotation = Quaternion.Euler(0f, 135f, 0f);
            anim.Play("Win");
            return;
        }

        Array.Sort(units, (u1, u2)
            => Vector3.Distance(transform.position, u1.transform.position)
            .CompareTo(Vector3.Distance(transform.position, u2.transform.position)));

        target = units[0].gameObject;

        agent.isStopped = false;
        agent.SetDestination(target.transform.position);

        anim.Play("Move");
    }
    public void LookAtTarget()
    {
        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }
    public void CheckTargetDead()
    {
        if (target == null || 
            target.GetComponent<Unit>().IsDead)
        {
            anim.Play("Search");
        }
    }
    public void CheckAttackRange()
    {
        CheckTargetDead();

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        float radius = 2f * range + 0.5f;
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance <= radius)
        {
            if(agent.enabled)
                agent.isStopped = true;
            anim.SetFloat("AttackSpeed", attackSpeed);
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
    public void CheckSkillUsable()
    {
        if (curMana == maxMana &&
            !isPassive)
        {
            CurMp = 0;
            anim.Play("Skill");
            OnSkillUsed?.Invoke();
        }
    }

    public void AutoAttack()
    {
        if (!target.GetComponent<Unit>().IsDead)
        {
            float finalDamage;
            bool isCrit;
            float rand = Random.Range(0f, 100f);
            if (rand < critRatio)
            {
                finalDamage = (attackDamage * (critDamage / 100f)) * (1f + increasedDeal / 100f);
                isCrit = true;
            }
            else
            {
                finalDamage = attackDamage * (1f + increasedDeal / 100f);
                isCrit = false;
            }
            target.GetComponent<Unit>().TakeAutoAttack(this, finalDamage, isCrit);

            UpdateMana(manaRegeneration);
            OnRunAutoAttack?.Invoke();
        }
    }
    public void SkillAttack(float ratio, bool isAP)
    {
        if (!target.GetComponent<Unit>().IsDead)
        {
            float finalDamage;
            if (isAP)
            {
                finalDamage = (AP * ratio / 100f) * (1f + increasedDeal / 100f);
            }
            else
            {
                finalDamage = (attackDamage * ratio / 100f) * (1f + increasedDeal / 100f);

            }
            target.GetComponent<Unit>().TakeSkillAttack(this, finalDamage, isAP);
        }
    }
    public void HybridAttack()
    {
        if (!target.GetComponent<Unit>().IsDead)
        {
            float adFinalDamage;
            float apFinalDamage;
            apFinalDamage = ((AP * apRatio / 100f) * (1f + increasedDeal / 100f)) / skillAttackCount;
            adFinalDamage = (attackDamage * adRatio / 100f) * (1f + increasedDeal / 100f) / skillAttackCount;
            target.GetComponent<Unit>().TakeSkillAttack(this, apFinalDamage, true);
            target.GetComponent<Unit>().TakeSkillAttack(this, adFinalDamage, false);
        }
    }
    public void SplashAttack(GameObject target, float radius, float ratio, bool isAP)
    {
        LayerMask layer = transform.CompareTag("Unit") ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("Field");
        Collider[] enemy = Physics.OverlapSphere(target.transform.position, radius, layer);

        if (enemy.Length == 0)
        {
            return;
        }
        float finalDamage = 0;
        if (isAP)
        {
            finalDamage = (AP * ratio / 100f) * (1f + increasedDeal / 100f);
        }
        else
        {
            finalDamage = (attackDamage * ratio / 100f) * (1f + increasedDeal / 100f);

        }
        foreach (var col in enemy)
        {
            col.GetComponent<Unit>().TakeSkillAttack(this, finalDamage, isAP);
        }
    }
    public void UpdateMana(int amout)
    {
        if (isManaBan) return;

        CurMp += amout;
    }
    public void TakeAutoAttack(Unit attacker, float damage, bool crit)
    {
        if (IsDead) return;

        float rand = Random.Range(0f, 100f);
        if (rand < avoid)
        {
            vfxPrinter.PrintTextFX(string.Empty, transform.position, TextType.Avoid);
            return;
        }

        float actualDamage = Mathf.Round(damage * (1f - (armor / (armor + 100f))));
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
        attacker.GetLifeSteel(damage);
        attacker.RecordDealAmount(damage);

        vfxPrinter.PrintHitFX(transform.position);
        if (crit)
        {
            vfxPrinter.PrintTextFX(actualDamage.ToString(), transform.position, TextType.Crit);
        }
        else
        {
            vfxPrinter.PrintTextFX(actualDamage.ToString(), transform.position, TextType.Attack);
        }

        if (curHealth <= 0)
        {
            IsDead = true;
            return;
        }
        UpdateMana(5);
        attacker.OnSuccessAutoAttack?.Invoke(this);
    }
    public void TakeSkillAttack(Unit attacker, float damage, bool isAP)
    {
        if (IsDead) return;

        float actualDamage;
        if (isAP)
        {
            actualDamage = damage * (1f - (resist / (resist + 100f)));
        }
        else
        {
            actualDamage = damage * (1f - (armor / (armor + 100f)));
        }

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
        attacker.GetLifeSteel(actualDamage);
        attacker.RecordDealAmount(actualDamage);

        vfxPrinter.PrintHitFX(transform.position);
        if (isAP)
        {
            vfxPrinter.PrintTextFX(actualDamage.ToString(), transform.position, TextType.Skill);
        }
        else
        {
            vfxPrinter.PrintTextFX(actualDamage.ToString(), transform.position, TextType.Attack);
        }
        if (CurHp <= 0)
        {
            IsDead = true;
            return;
        }
    }
    public void GetLifeSteel(float damage)
    {
        if (lifeSteel <= 0 ||
            curHealth == maxHealth) 
            return;

        float steelAmount = Mathf.Round(damage * (lifeSteel / 100f));
        CurHp += steelAmount;
        vfxPrinter.PrintTextFX(steelAmount.ToString(), transform.position, TextType.Heal);
    }
    public void RecordDealAmount(float damage)
    {
        dealAmount += damage;
        OnDealAmountChanged?.Invoke();
    }
    public void StartStun(float duration)
    {
        if (duration == 0 || 
            IsDead) return;
        stunDuration = duration;
        curStunTime = 0f;
        anim.Play("Panic");
        agent.isStopped = true;
        StartCoroutine(Stun_Co());
    }
    private IEnumerator Stun_Co()
    {
        while (curStunTime < stunDuration)
        {
            curStunTime += Time.deltaTime;
            yield return null;
        }
        anim.Play("Search");
    }
    public void GetShield(float amount)
    {
        MaxShield = amount;
    }
    public void Transformation()
    {
        defaultBody.SetActive(false);
        tranformationBody.SetActive(true);
    }
    public void ReturnDefaultBody()
    {
        tranformationBody.SetActive(false);
        defaultBody.SetActive(true);
    }
    public void Dead()
    {
        if (isEnemy)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    #region skill
    public void ActiveSkill()
    {
        switch (No)
        {
            case 0:
                OnBattleStart += SkillEffect0;
                break;
            case 1:
                SkillEffect1();
                break;
            case 2:
                SkillEffect2();
                break;
            case 3:
                SkillEffect3();
                break;
            case 4:
                SkillEffect4();
                break;
            case 5:
                SkillEffect5();
                break;
            case 6:
                SkillEffect6();
                break;
            case 7:
                SkillEffect7();
                break;
            case 8:
                SkillEffect8();
                break;
            case 9:
                SkillEffect9();
                break;
            case 10:
                SkillEffect10();
                break;
            case 11:
                SkillEffect11();
                break;
            case 12:
                SkillEffect12();
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
            case 23:
                break;
            case 24:
                break;
            case 25:
                break;
            case 26:
                break;
            case 27:
                break;
            case 28:
                break;
            case 29:
                break;
            case 30:
                break;
            case 31:
                break;
            case 32:
                break;
            case 33:
                break;
            case 34:
                break;
            case 35:
                break;
            case 36:
                break;
            case 37:
                break;
            case 38:
                break;
            case 39:
                break;
            case 40:
                break;
            case 41:
                break;
            case 42:
                break;
            case 43:
                break;
            case 44:
                break;
            case 45:
                break;
            case 46:
                break;
            case 47:
                break;
            case 48:
                break;
            case 49:
                break;
            case 50:
                break;
            case 51:
                break;
            case 52:
                break;
            case 53:
                break;
            case 54:
                break;
            case 55:
                break;
            case 56:
                break;
        }
    }
    private void SkillEffect0() //하루카
    {
        switch (Grade)
        {
            case 1:
                GetShield(Mathf.Round(maxHealth * 0.2f));
                break;
            case 2:
                GetShield(Mathf.Round(maxHealth * 0.3f));
                break;
            case 3:
                GetShield(Mathf.Round(maxHealth * 0.4f));
                break;
        }
    }
    private void SkillEffect1() //히후미
    {
        switch (Grade)
        {
            case 1:
                adRatio = 200f;
                break;
            case 2:
                adRatio = 300f;
                break;
            case 3:
                adRatio = 400f;
                break;
        }
        SplashAttack(target, 2f, adRatio, false);
        vfxPrinter.PrintExplosionFX(target.transform);
    }
    private void SkillEffect2() //카즈사
    {
        switch (Grade)
        {
            case 1:
                CurHp += abilityPower * 150f / 100f;
                break;
            case 2:
                CurHp += abilityPower * 250f / 100f;
                break;
            case 3:
                CurHp += abilityPower * 400f / 100f;
                break;
        }
        AD += attackDamage * 0.15f;
    }
    private void SkillEffect3() //마리나
    {
        switch (Grade)
        {
            case 1:
                Armor += 20f;
                break;
            case 2:
                Armor += 35f;
                break;
            case 3:
                Armor += 60f;
                break;
        }
    }
    private void SkillEffect4() //미치루
    {
        switch (Grade)
        {
            case 1:
                apRatio = 110f;
                break;
            case 2:
                apRatio = 210f;
                break;
            case 3:
                apRatio = 310f;
                break;
        }
        SkillAttack(apRatio, true);
    }
    private void SkillEffect5() //미도리
    {
        skillAttackCount = 5;
        switch (Grade)
        {
            case 1:
                adRatio = 110f;
                apRatio = 70f;
                break;
            case 2:
                adRatio = 160f;
                apRatio = 90f;
                break;
            case 3:
                adRatio = 210f;
                apRatio = 110f;
                break;
        }
    }
    private void SkillEffect6() //모모이
    {
        switch (Grade)
        {
            case 1:
                AD += attackDamage * 0.2f;
                break;
            case 2:
                AD += attackDamage * 0.3f;
                break;
            case 3:
                AD += attackDamage * 0.4f;
                break;
        }
    }
    private void SkillEffect7() //무츠키
    {
        switch (Grade)
        {
            case 1:
                apRatio = 100f;
                break;
            case 2:
                apRatio = 160f;
                break;
            case 3:
                apRatio = 240f;
                break;
        }
        SplashAttack(target, 2f, apRatio, true);
        vfxPrinter.PrintExplosionFX(target.transform);
    }
    private void SkillEffect8() //노도카
    {
        if (target == null) return; 
        switch (Grade)
        {
            case 1:
                apRatio = 100f;
                target.GetComponent<Unit>().StartStun(2f);
                break;
            case 2:
                apRatio = 160f;
                target.GetComponent<Unit>().StartStun(3f);
                break;
            case 3:
                apRatio = 240f;
                target.GetComponent<Unit>().StartStun(4f);
                break;
        }
        SkillAttack(apRatio, true);
    }
    private void SkillEffect9() //사오리
    {
        switch (Grade)
        {
            case 1:
                adRatio = 300f;
                break;
            case 2:
                adRatio = 400f;
                break;
            case 3:
                adRatio = 500f;
                break;
        }
        SkillAttack(adRatio, false);
    }
    private void SkillEffect10() //세리카
    {
        switch (Grade)
        {
            case 1:
                AS += Mathf.Round(AS * 0.2f * 100f) / 100f;
                break;
            case 2:
                AS += Mathf.Round(AS * 0.3f * 100f) / 100f;
                break;
            case 3:
                AS += Mathf.Round(AS * 0.4f * 100f) / 100f;
                break;
        }
    }
    private void SkillEffect11() //슈에린
    {
        switch (Grade)
        {
            case 1:
                apRatio = 110f;
                break;
            case 2:
                apRatio = 210f;
                break;
            case 3:
                apRatio = 310f;
                break;
        }
        SkillAttack(apRatio, true);
    }
    private void SkillEffect12() //츠쿠요
    {
        isManaBan = true;
        switch (Grade)
        {
            case 1:
                Avoid += 10f;
                break;
            case 2:
                Avoid += 15f;
                break;
            case 3:
                Avoid += 20f;
                break;
        }
    }
    #endregion

    #region trait
    public void PrintTraitVFX()
    {
        vfxPrinter.PrintTraitFX(transform);
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
                AS += Mathf.Round(attackSpeed * 0.15f * 100f) / 100f;
                break;
            case 2:
                AS += Mathf.Round(attackSpeed * 0.35f * 100f) / 100f;
                break;
            case 3:
                AS += Mathf.Round(attackSpeed * 0.65f * 100f) / 100f;
                break;
            case 4:
                AS += Mathf.Round(attackSpeed * 0.90f * 100f) / 100f;
                break;
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
        for (int i = 0; i < unitManager.fieldList.Count; i++)
        {
            if (!unitManager.fieldList[i].IsDead &&
                unitManager.fieldList[i].Origin == 6)
            {
                unitManager.fieldList[i].IncreaseAdApTrait6();
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
            OnSuccessAutoAttack += StunAttackTrait7;
        }
        else
        {
            OnSuccessAutoAttack -= StunAttackTrait7;
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
                increasedDeal -= 20f;
                break;
            case 2:
                increasedDeal -= 35f;
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
                increasedDeal += 20f;
                break;
            case 2:
                increasedDeal += 35f;
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
    public void SetTrait_10(int old, int rank) //정찰대
    {
        OnRunAutoAttack -= ExtraAttackTrait10;
        trait10Rank = rank;
        if(rank != 0)
        {
            OnRunAutoAttack += ExtraAttackTrait10;
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
                    finalDamage = (attackDamage * (critDamage / 100f)) * (1f + increasedDeal / 100f);
                    isCritical = true;
                }
                else
                {
                    finalDamage = attackDamage * (1f + increasedDeal / 100f);
                    isCritical = false;
                }
                target.GetComponent<Unit>().TakeAutoAttack(this, finalDamage, isCritical);
            }
        }
    }
    public void SetTrait_11(int old, int rank) //수집가
    {
        OnSuccessAutoAttack -= BurnOutManaTrait11;
        if(rank != 0)
        {
            OnSuccessAutoAttack += BurnOutManaTrait11;
        }
    }
    public void BurnOutManaTrait11(Unit enemy) 
    {
        enemy.UpdateMana(-5);
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
                GetShield(Mathf.Round(maxHealth * 0.3f));
                break;
            case 2:
                GetShield(Mathf.Round(maxHealth * 0.5f));
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
                range -= 1;
                break;
            case 2:
                CritRatio -= 30f;
                range -= 2;
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
                range += 1;
                break;
            case 2:
                CritRatio += 30f;
                range += 2;
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
                AS -= Mathf.Round(InitAS * 0.2f * 100f) / 100f;
                break;
            case 2:
                AS -= Mathf.Round(InitAS * 0.5f * 100f) / 100f;
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
                AS += Mathf.Round(InitAS * 0.2f * 100f) / 100f;
                break;
            case 2:
                AS += Mathf.Round(InitAS * 0.5f * 100f) / 100f;
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
                increasedDeal -= 15;
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
                increasedDeal += 15;
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
        OnSuccessAutoAttack -= DestroyEnemy;
        if(rank != 0)
        {
            OnSuccessAutoAttack += DestroyEnemy;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 15f);
    }
}
