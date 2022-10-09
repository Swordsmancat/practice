using GameFramework.Fsm;
using System;
using System.Collections;
using UnityGameFramework.Runtime;
using UnityEngine;
using GameFramework;
using System.Collections.Generic;
using DG.Tweening;
using FIMSpace.FLook;
using GameFramework.Event;

namespace Farm.Hotfix
{
    public class PlayerLogic : TargetableObject
    {
        public static PlayerLogic Instance;
        [SerializeField]
        private PlayerData m_PlayerData = null;

        public PlayerData PlayerData => m_PlayerData;


        public WeaponInfo m_WeaponInfo;
        public SkillInfo m_SkillInfo;

        private List<WeaponData> weaponDatas;

        [SerializeField]
        private List<Armor> m_Armors = new List<Armor>();

        public IFsm<PlayerLogic> fsm;                     //状态机
        public List<FsmState<PlayerLogic>> stateList;     //状态机状态列表
        public List<Armor> Armors => m_Armors;

        //  private IFsm<PlayerLogic> m_PlayerLogic;

        //  private List<FsmState<PlayerLogic>> stateList;

        public WeaponLogicLeftHand LeftHand;
        public WeaponLogicRightHand RightHand;


        public GameObject m_Attacker;               //攻击者

        public float MoveX = 0;

        public float MoveY = 0;

        public bool isAttack = false;               //攻击
        public bool isDodge = false;                //翻滚
        //public bool isDefense = false;              //防御
        public bool isRun = false;                  //跑
        public bool isMotion = false;               //运动状态
        public bool isWeaponState = false;          //角色进入武器切换状态
        public bool isKnockedDown = false;          //被击倒
        public bool isFourthAtk = false;            //角色技能（当时是把普通攻击第四招扯出来撤出来设为特殊攻击 后面改为技能）
        public bool isSecondSkill = false;          //角色第二段技能
        public bool isHurt = false;                 //角色受伤
        public bool isChangeWeapon = false;         //更换武器
        public bool isStep = false;                 //角色滑步（垫步）
        public bool IsParry = false;                //是否招架(拼刀)
        public bool isCanStepAtk = false;           //角色垫步后是否可以执行垫步攻击
        public bool isAim = false;                  //瞄准怪物
        public bool isThump = false;                //重击(现在的技能 没改名字)
        public bool isShoulder = false;             //肩撞无敌
        public bool isPushAttack = false;
        public bool isGunAim = false;               //角色是否举枪瞄准
        public bool isBehindAtked = false;          //角色是否从背后受到攻击
        public bool isFocusEngy = false;            //角色蓄力 按下攻击键为true 抬起后为false
        public int attackCount = 0;                 //第几招普攻
        public bool isSpecialAtk = false;           //特殊大招？(用于顿帧)
        private bool isHands = true;                //武器是否在手上
        public bool IsHands { get => isHands; set => isHands = value; }
        private bool isShift = false;               //角色是否冲刺
        public float shiftSpeed;                   //冲刺速度
        public Vector3 attackDir;
        public bool m_AnimationEventGetUp;
        public BuffType m_BuffType; //受击状态
        public bool isStoic;  //受否为霸体
        public bool underAttack;  //防御受击

        public MoveBehaviour m_moveBehaviour;

        public BoxCollider m_LeftWeaponCollider;

        public BoxCollider m_RightWeaponCollider;

        public AnimatorStateInfo currentStateInfo;

        private FLookAnimator m_lookAnimator;       //头看向怪物插件


        //===================== Attack =====================//
        private bool m_WeaponAttack;
        private bool m_WeaponDefense;
        private WaitForSeconds m_AttackInputWait;
        private WaitForSeconds m_AttackHoldWait;
        private Coroutine m_AttackWaitCoroutine;
        private Coroutine m_AttackHoldWaitCoroutine;

        private float m_AttackInputDuration = 0.5f;
        private float m_AttackHoldDuration = 1f;
        public bool m_Attack;
        public bool m_IsAttackTap;
        public bool m_IsAttackThump;
        public bool m_Explosion;
        public bool m_DoubleClick;
        public bool m_IsAttackJump;
        public bool Whirlwind;
        private int m_FocusCount =0;

        public float m_ClickAttackBtnDuration;      // 按下攻击键的持续时间
        public bool m_IsStartCount;                 // 是否开始计时
        private float comboTime;                    //记录连续攻击时间
        private bool isCombo = false;               //是否可以连击 用于大剑肩撞之后一段时间内可衔接普攻
        private float R_HoldTime;
        private float L_HoldTime;
        private float NeedHoldTime = 0.2f;

        private float m_ClickDouble;
        [SerializeField]
        private WeaponAttackPoint m_WeaponAttackPoint_L = null;
        [SerializeField]
        private WeaponAttackPoint m_WeaponAttackPoint_R = null;
        public WeaponAttackPoint WeaponAttackPoint_R => m_WeaponAttackPoint_R;

        #region LockField
        private int currentLockEnemyIndex = 0;
        private int chnageLockCount = 1;

        private List<GameObject> m_FrontEnemyObj;

        private int frontEnemyNum = 0;
        public EnemyLogic currentLockEnemy;
        private Transform m_LockPosition;

        public bool m_LockEnemy;

        #endregion 

        private ProcedureMain m_ProcedureMain;



        private EquiState m_EquiState = EquiState.SwordShield;

        public EquiState EquiState
        {
            get
            {
                return m_EquiState;
            }
        }

        //    public int playerEquipState = 1;            // 角色当前装备的武器类型

        private bool firstShow = true;

        private readonly string NPCTag = "NPC";
        private GameObject[] NPCS;
        private int WalkLayerMask = 1 << 9;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Instance = this;
            m_moveBehaviour = GetComponent<MoveBehaviour>();
            stateList = new List<FsmState<PlayerLogic>>();
            m_AttackInputWait = new WaitForSeconds(m_AttackInputDuration);
            m_AttackHoldWait = new WaitForSeconds(m_AttackHoldDuration);
           // m_CapsuleCollider = GetComponent<CapsuleCollider>();
           // m_Radius = m_CapsuleCollider.radius * 0.9f;
        }

        protected override void OnDead(Entity attacker, Vector3 point)
        {
            base.OnDead(attacker,point);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_PlayerData = userData as PlayerData;
            if (m_PlayerData == null)
            {
                Log.Error("Player data is invalid.");
                return;
            }
            Name = Utility.Text.Format("Player {0}", Id);
            weaponDatas = m_PlayerData.GetAllWeaponDatas();
            //for (int i = 0; i < weaponDatas.Count; i++)
            //{
            //   // GameEntry.Entity.ShowWeapon(weaponDatas[i]);
            //    GameEntry.Entity.ShowWeapon(weaponDatas[i],EntityExtension.WeaponHand.Right);

            //}

            // GameEntry.Entity.ShowWeapon(weaponDatas[0], EntityExtension.WeaponHand.Right);
            // GameEntry.Entity.ShowWeapon(weaponDatas[1], EntityExtension.WeaponHand.Left);
            m_ProcedureMain = (ProcedureMain)GameEntry.Procedure.CurrentProcedure;
            List<ArmorData> armorDatas = m_PlayerData.GetAllArmorDatas();
            m_FrontEnemyObj = new List<GameObject>();
            for (int i = 0; i < armorDatas.Count; i++)
            {
                GameEntry.Entity.ShowArmor(armorDatas[i]);
            }
            SetWeaponInfo();
            CreateFsm();
            m_lookAnimator = GetComponent<FLookAnimator>();
            m_moveBehaviour.SetCameraGruop();
            // ShowRunEffect();
        }

        private void SetWeaponInfo()
        {
            m_WeaponInfo = gameObject.AddComponent<WeaponInfo>();
            m_WeaponInfo.Init();

            m_SkillInfo = gameObject.AddComponent<SkillInfo>();
            m_SkillInfo.Init();
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
            isChangeWeapon = true;

            if (childEntity is WeaponLogicLeftHand)
            {
                Weapons.Add((WeaponLogicLeftHand)childEntity);
                m_WeaponAttackPoint_L = childEntity.GetComponent<WeaponAttackPoint>();
                m_LeftWeaponCollider = childEntity.GetComponent<BoxCollider>();
                LeftHand = (WeaponLogicLeftHand)childEntity;
                return;
            }
            if (childEntity is WeaponLogicRightHand)
            {
                Weapons.Add((WeaponLogicRightHand)childEntity);
                m_WeaponAttackPoint_R = childEntity.GetComponent<WeaponAttackPoint>();
                m_RightWeaponCollider = childEntity.GetComponent<BoxCollider>();
                RightHand = (WeaponLogicRightHand)childEntity;
                return;
            }
            if (childEntity is Armor)
            {
                m_Armors.Add((Armor)childEntity);
                return;
            }
        }

        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
            if (childEntity is WeaponLogic)
            {
                Weapons.Remove((WeaponLogic)childEntity);
                return;
            }

            if (childEntity is Armor)
            {
                m_Armors.Remove((Armor)childEntity);
                return;
            }
        }
        private static readonly int IsLockEnemy = Animator.StringToHash("IsLockEnemy");

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);           
            if (m_LockEnemy)
            {
                m_Animator.SetBool(IsLockEnemy, true);
            }

            else
            {

                m_Animator.SetBool(IsLockEnemy, false);
            }
            if (firstShow)
            {
                GameEntry.Entity.ShowWeapon(weaponDatas[0], EntityExtension.WeaponHand.Right);
                GameEntry.Entity.ShowWeapon(weaponDatas[1], EntityExtension.WeaponHand.Left);
                firstShow = false;
            }
            // if (OnGround())
            // {
            PlayerInteractive();
            // }
            // Log.Info(fsm.CurrentState);
            //if (Physics.Raycast(, Vector3.down, 0.2f, LayerMask)&&fsm.CurrentState.)
            //{
            //    GameEntry.Sound.PlaySound();
            //}

            PlayerShiftInUpdate(isShift, shiftSpeed);
        
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            firstShow = true;
            DestroyFsm();
        }

         float m_DodgeX, m_DodgeY;
        private void PlayerInteractive()
        {

            //if (Input.GetMouseButtonDown(1))
            //{
            //    Log.Info("按下");
            //}
            // 采用GetAxisRaw 因为锁定后 需要灵活控制方向移动 GetAxis使用起来方向不灵活
            m_DodgeX = Input.GetAxisRaw("Horizontal");
            m_DodgeY = Input.GetAxisRaw("Vertical");
            MoveX = Input.GetAxis("Horizontal");
            MoveY = Input.GetAxis("Vertical"); 

            m_Animator.SetFloat("DodgeX", m_DodgeX);
            m_Animator.SetFloat("DodgeY", m_DodgeY);

            m_Animator.SetFloat("MoveX", MoveX);
            m_Animator.SetFloat("MoveY", MoveY);

            if (m_moveBehaviour.m_Horizontal != 0 || m_moveBehaviour.m_Vertical != 0)
            {
                if (InputManager.IsAccelerate() && !IsDefense && m_EquiState != EquiState.None && !isGunAim)
                {
                    isRun = true;
                    isFourthAtk = false;
                }
                else
                {
                    if (isRun)
                    {
                        isRun = false;
                    }
                }
            }

            switch (EquiState)
            {
                case EquiState.None:
                    break;
                case EquiState.SwordShield:
                    SwordShieldAttack();
                    SwordShieldDefense();
                    break;
                case EquiState.GiantSword:
                    GiantSwordAttack();
                    SwordShieldDefense();
                    break;
                case EquiState.Dagger:
                    DaggerAttack();
                    SwordShieldDefense();
                    break;
                case EquiState.DoubleBlades:
                    DoubleBladesAttack();
                    break;
                case EquiState.RevengerDoubleBlades:
                    DoubleBladesAttack();
                    break;
                case EquiState.Pistol:
                    BowAttack();
                    break;
                default:
                    break;
            }
            //================= Dod&Step =================//
            if (!IsDefense && !isHurt)
            {
                if (InputManager.IsClickDodge())
                {
                    isDodge = true;
                }
            }
            if (InputManager.IsClickStep())
            {
                isStep = true;
            }
            //================= Defense =================//
           // m_Animator.SetBool("Defense", IsDefense);

            ////================= Aim瞄准(火枪 后面要改成弓箭) =================//
            //if (InputManager.IsClickHoldMouseRight() && m_EquiState == EquiState.Pistol && IsHands)
            //{
            //    if (isGunAim)
            //    {
            //        m_Animator.SetBool("IsAim", false);
            //        m_LockEnemy = false;
            //        isGunAim = false;
            //    }
            //    else
            //    {
            //        m_Animator.SetBool("IsAim", true);
            //        m_LockEnemy = true;
            //        isGunAim = true;
            //    }
            //    OnSetGunAim(isGunAim);
            //}

            //================= Lock =================//
            if (InputManager.IsClickLock())
            {
                if (isGunAim)
                {
                    m_Animator.SetBool("IsAim", false);
                    m_LockEnemy = false;
                    isGunAim = false;
                    OnSetGunAim(isGunAim);
                }
                OnSetLock();
            }

            LockOnEnemyDead();

            //================= ChangeWeap =================//
            if (Input.GetKeyDown(KeyCode.Alpha1))//剑盾
            {
                isWeaponState = true;
                m_EquiState = EquiState.SwordShield;
                ChangeWeaponRight(30000);
                ChangeWeaponLeft(30002);
                return;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))//大剑
            {
                Whirlwind = false;
                isWeaponState = true;
                m_EquiState = EquiState.GiantSword;
                //ChangeWeaponRight(30004);
                ChangeWeaponRight(30012);
                DestroyWeapon(30002);
                return;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))//短刀
            {
                isWeaponState = true;
                m_EquiState = EquiState.Dagger;
                ChangeWeaponRight(30006);
                DestroyWeapon(30002);
                return;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))//双刀
            {
                m_Explosion = false;
                isWeaponState = true;
                m_EquiState = EquiState.DoubleBlades;
                ChangeWeaponRight(30011);
                ChangeWeaponLeft(30010);
                return;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))//远程 之前叫火枪 后面要改为弓箭
            {
                isWeaponState = true;
                m_EquiState = EquiState.Pistol;
                // ChangeWeaponRight(30009);
                //DestroyWeapon(30000);
                CleanHandleWeapon();
                ChangeWeaponLeft(30009);
                ChangeWeaponRight(30016);
              //  BowInit();
                return;
            }
            //================= Skill =================//
            if (Input.GetKeyDown(KeyCode.E) && !IsDefense && !isHurt && !isFourthAtk && !isSecondSkill && IsHands)
            {
                m_Animator.SetInteger("ThumpNum", 8);
                isFourthAtk = true;
                m_IsStartCount = false;

            }
            if (Input.GetKeyDown(KeyCode.R) && !IsDefense && !isHurt && !isFourthAtk && !isSecondSkill && IsHands)
            {
                m_Animator.SetInteger("ThumpNum", 9);
                isFourthAtk = true;
                m_IsStartCount = false;
            }

            // GameObject.FindGameObjectsWithTag(NPCTag);
#if UNITY_EDITOR
            InputManager.IsOnGame();
#endif 
            isMotion = IsDefaultAnimPlay();
            RecordCombon();
        }

        private void GiantSwordAttack()
        {
            if (!Whirlwind)
            {
                if (InputManager.IsWhirlwind())
                {
                    Whirlwind = true;
                    m_Animator.SetTrigger("Whirlwind");
                    Whirlwind = false;
                }
            }
            if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight() || InputManager.IsClickHoldMouseRight() || InputManager.IsClickHoldMouserLeft() || InputManager.IsClickUpMouseLeft() || InputManager.IsClickUpMouseRight() && !isHurt)
            {
                if (InputManager.IsClickHoldMouseRight())
                {
                    R_HoldTime += Time.deltaTime;
                }
                else
                {
                    R_HoldTime = 0;
                }

                if (InputManager.IsClickHoldMouserLeft())
                {
                    L_HoldTime += Time.deltaTime;
                }
                else
                {
                    L_HoldTime = 0;
                }

                if (R_HoldTime > NeedHoldTime && InputManager.IsClickHoldMouseRight())
                {
                    m_Animator.SetInteger("R_HoldClick", 1);// -1为停止蓄力 0为左键蓄力，1为右键蓄力
                }
                else
                {
                    m_Animator.SetInteger("R_HoldClick", 0);
                }

                if (L_HoldTime > NeedHoldTime && InputManager.IsClickHoldMouserLeft())
                {
                    m_Animator.SetInteger("L_HoldClick", 1);
                }
                else
                {
                    m_Animator.SetInteger("L_HoldClick", 0);
                }
                
                m_Animator.SetInteger("FocusCount", m_FocusCount);


                
                if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight())
                {
                    if (m_AttackWaitCoroutine != null)
                    {
                        StopCoroutine(m_AttackWaitCoroutine);
                    }
                    m_AttackWaitCoroutine = StartCoroutine(AttackWait());
                }
                else
                {
                    if (m_AttackHoldWaitCoroutine != null)
                    {
                        StopCoroutine(m_AttackHoldWaitCoroutine);
                    }
                    m_AttackHoldWaitCoroutine = StartCoroutine(AttackHoldWait());
                }
            }

            if (m_WeaponAttack)
            {
               m_WeaponAttackPoint_R.SetAttackPoint();
                // m_WeaponAttackPoint_L.SetAttackPoint();
            }
        }


        private bool m_BowInit;
        private bool m_ShootRest;
        private bool m_IsAiming;
        private bool m_CanShoot;
        private bool m_IsOnBowAim;
        private float m_TimeToShoot =1.2f;
        private float m_ShootWait = 0.3f;

        private Transform m_ArrorTransform;
        private Vector3 m_ArrowImpulse;
        private Vector3 m_DefaultImpulse = new Vector3(2, 5, 80);

        private Coroutine m_PrepareCoro;
        private Coroutine m_ShootCoro;

        private ParticleSystem[] prepareParticles;
        private ParticleSystem[] aimParticles;
        private GameObject circleParticlePrefab;

        private Transform bowModel;
        private Transform m_BowShootPoint;


        private void BowInit()
        {
            //var entity = GameEntry.Entity.GetEntity(weaponDatas[1].Id);
            // if(entity == null)
            // {
            //     return;
            // }
            // ParticlesBow bow = entity.GetComponent<ParticlesBow>();
            // prepareParticles = bow.prepareParticles;
            // aimParticles = bow.aimParticles;
            // circleParticlePrefab = bow.circleParticlePrefab;
            // bowModel = bow.arrow;
            m_BowShootPoint = FindTools.FindFunc<Transform>(transform,"LeftHand");
            m_BowInit = true;
        }
        public IEnumerator PrepareSequence()
        {
            //foreach (ParticleSystem part in prepareParticles)
            //{
            //    part.Play();
            //}

            yield return new WaitForSeconds(m_TimeToShoot);
            m_CanShoot = true;
            //foreach (ParticleSystem part in aimParticles)
            //{
            //    part.Play();
            //}
        }

        public IEnumerator ShootSequence()
        {
           // yield return new WaitUntil(() => m_CanShoot == true);
            m_ShootRest = true;
            m_IsAiming = false;
            m_CanShoot = false;
            // bowModel.gameObject.SetActive(true);
            // yield return new WaitForSeconds(0.01f);
           
           // bowModel.gameObject.SetActive(false);
            yield return new WaitForSeconds(m_ShootWait);
            m_ShootRest = false;
        }

        private void BowAttack()
        {
            if (InputManager.IsBowAim())
            {
                if (!m_IsOnBowAim)
                {
                    m_moveBehaviour.BowAimOn();
                    m_Animator.SetBool("IsAim", true);
                    m_IsOnBowAim = true;
                    
                }
                else
                {
                    m_moveBehaviour.BowAimOff();
                    m_Animator.SetBool("IsAim", false);
                    m_IsOnBowAim = false;
                }
               
            }
            //if(InputManager.IsClickDownMouseLeft() && !isHurt)
            //{
            //    if (!m_BowInit)
            //    {
            //        BowInit();
            //    }
            //    if (!m_ShootRest && !m_IsAiming)
            //    {
            //        m_CanShoot = false;
            //        m_IsAiming = true;
            //        if(m_PrepareCoro != null)
            //        {
            //            StopCoroutine(m_PrepareCoro);
            //        }
            //        // m_PrepareCoro = StartCoroutine(PrepareSequence());
            //        m_CanShoot = true;
            //        // bowModel.gameObject.SetActive(true);

            //    }
            //}

            //if(InputManager.IsClickUpMouseLeft() && !isHurt)
            //{
            //    if(!m_ShootRest && m_IsAiming)
            //    {
            //        if (m_ShootCoro != null)
            //        {
            //            StopCoroutine(m_ShootCoro);
            //        }
            //        m_ShootCoro = StartCoroutine(ShootSequence());
            //    }
            //    if (m_AttackWaitCoroutine != null)
            //    {
            //        StopCoroutine(m_AttackWaitCoroutine);
            //    }
            //    m_AttackWaitCoroutine = StartCoroutine(AttackWait());
            //}
            if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight() || InputManager.IsClickHoldMouseRight() || InputManager.IsClickHoldMouserLeft() || InputManager.IsClickUpMouseLeft() || InputManager.IsClickUpMouseRight() && !isHurt)
            {

                    if (!m_BowInit)
                    {
                        BowInit();
                    }
                if (InputManager.IsClickHoldMouserLeft())
                {
                    L_HoldTime += Time.deltaTime;
                }
                else
                {
                    L_HoldTime = 0;
                }

                if (L_HoldTime > NeedHoldTime && InputManager.IsClickHoldMouserLeft())
                {
                    m_Animator.SetInteger("L_HoldClick", 1);
                }
                else
                {
                    m_Animator.SetInteger("L_HoldClick", 0);
                }

                if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight())
                {
                    if (m_AttackWaitCoroutine != null)
                    {
                        StopCoroutine(m_AttackWaitCoroutine);
                    }
                    m_AttackWaitCoroutine = StartCoroutine(AttackWait());
                }
                else
                {
                    if (m_AttackHoldWaitCoroutine != null)
                    {
                        StopCoroutine(m_AttackHoldWaitCoroutine);
                    }
                    m_AttackHoldWaitCoroutine = StartCoroutine(AttackHoldWait());
                }

            }


            //if (m_Attack)
            //{
            //    m_moveBehaviour.isAttack = true;
            //}
            //else
            //{
            //    m_moveBehaviour.isAttack = false;
            //}
            //if (m_WeaponAttack)
            //{
            //    m_WeaponAttackPoint_L.SetAttackPoint();
            //    // m_WeaponAttackPoint_L.SetAttackPoint();
            //}
        }
        private void DaggerAttack()
        {
            if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight() || InputManager.IsClickHoldMouseRight() || InputManager.IsClickHoldMouserLeft() || InputManager.IsClickUpMouseLeft() || InputManager.IsClickUpMouseRight() && !isHurt)
            {

                if (InputManager.IsClickHoldMouseRight())
                {
                    R_HoldTime += Time.deltaTime;
                }
                else
                {
                    R_HoldTime = 0;
                }

                if (InputManager.IsClickHoldMouserLeft())
                {
                    L_HoldTime += Time.deltaTime;
                }
                else
                {
                    L_HoldTime = 0;
                }

                if (R_HoldTime > NeedHoldTime && InputManager.IsClickHoldMouseRight())
                {
                    m_Animator.SetInteger("R_HoldClick", 1);// -1为停止蓄力 0为左键蓄力，1为右键蓄力
                }
                else
                {
                    m_Animator.SetInteger("R_HoldClick", 0);
                }

                if (L_HoldTime > NeedHoldTime && InputManager.IsClickHoldMouserLeft())
                {
                    m_Animator.SetInteger("L_HoldClick", 1);
                }
                else
                {
                    m_Animator.SetInteger("L_HoldClick", 0);
                }

                if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight())
                {
                    if (m_AttackWaitCoroutine != null)
                    {
                        StopCoroutine(m_AttackWaitCoroutine);
                    }
                    m_AttackWaitCoroutine = StartCoroutine(AttackWait());
                }
                else
                {
                    if (m_AttackHoldWaitCoroutine != null)
                    {
                        StopCoroutine(m_AttackHoldWaitCoroutine);
                    }
                    m_AttackHoldWaitCoroutine = StartCoroutine(AttackHoldWait());
                }

            }


            if (m_Attack)
            {
                m_moveBehaviour.isAttack = true;
            }
            else
            {
                m_moveBehaviour.isAttack = false;
            }
            if (m_WeaponAttack)
            {
                m_WeaponAttackPoint_R.SetAttackPoint();
                // m_WeaponAttackPoint_L.SetAttackPoint();
            }
        }
        public void RestoreEnergy()
        {
            float currentEnergy;
            //if (Energy < 100)
            currentEnergy = PlayerData.MP + (20 * Time.deltaTime);
            PlayerData.MP = currentEnergy > PlayerData.MaxMP ? PlayerData.MP : currentEnergy;

        }
        private void SwordShieldDefense()
        {
            if (InputManager.IsClickDefense())
            {
                //ChangeState<PlayerHurtState>(procedureOwner);
                //Log.Info("精力值...." + m_PlayerData.MP);
                if (m_PlayerData.MP > 0 && !isKnockedDown) 
                {
                    IsDefense = true;
                    //m_Animator.SetBool("Defense", true);
                }
                
            }
            else
            {
                IsDefense = false;
                m_Animator.SetBool("Defense", false);
            }
            if (m_PlayerData.MP <= 0)
            {
                m_Animator.SetBool("Defense", false);
            }
        }

        private void SwordShieldAttack()
        {

            if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight() || InputManager.IsClickHoldMouseRight() || InputManager.IsClickHoldMouserLeft() ||InputManager.IsClickUpMouseLeft()||InputManager.IsClickUpMouseRight() && !isHurt)
            {

                if (InputManager.IsClickHoldMouseRight())
                {
                    R_HoldTime += Time.deltaTime;

                }
                else
                {
                    R_HoldTime = 0;
                }

                if (InputManager.IsClickHoldMouserLeft())
                {
                    L_HoldTime += Time.deltaTime;
                }
                else
                {
                    L_HoldTime = 0;
                }


                if (R_HoldTime>NeedHoldTime && InputManager.IsClickHoldMouseRight())
                {
                    m_Animator.SetInteger("R_HoldClick", 1);// 0为停止蓄力 1为蓄力
                }
                else
                {
                    m_Animator.SetInteger("R_HoldClick", 0);
                }

                if (L_HoldTime > NeedHoldTime && InputManager.IsClickHoldMouserLeft())
                {
                    m_Animator.SetInteger("L_HoldClick", 1);
                }
                else
                {
                    m_Animator.SetInteger("L_HoldClick", 0);
                }


                if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight())
                {
                    if (m_AttackWaitCoroutine != null)
                    {
                        StopCoroutine(m_AttackWaitCoroutine);
                    }
                    m_AttackWaitCoroutine = StartCoroutine(AttackWait());
                }
                else
                {
                    if (m_AttackHoldWaitCoroutine != null)
                    {
                        StopCoroutine(m_AttackHoldWaitCoroutine);
                    }
                    m_AttackHoldWaitCoroutine = StartCoroutine(AttackHoldWait());
                }
               
            }


            if (m_Attack)
            {
                m_moveBehaviour.isAttack = true;
            }
            else
            {
                m_moveBehaviour.isAttack = false;
            }
            if (m_WeaponAttack)
            {
                m_WeaponAttackPoint_R.SetAttackPoint();
               // m_WeaponAttackPoint_L.SetAttackPoint();
            }
        }
        private IEnumerator ExplosionChange()
        {
            //延迟0.5秒切换武器
            yield return new WaitForSecondsRealtime(0.5f);
            isWeaponState = true;
            m_EquiState = EquiState.RevengerDoubleBlades;
            ChangeWeaponRight(30011);
            ChangeWeaponLeft(30010);
        }
 
        private void DoubleBladesAttack()
        {
            if (!m_Explosion)
            {
                if (InputManager.IsExplosion())
                {
                    m_Explosion = true;
                    m_Animator.SetTrigger("TwoSwordsBuff");
                    StartCoroutine(ExplosionChange());
                }
            }

            if (MoveY < 0)
            {
                m_Animator.SetBool("ClickBack", true);
            }
            else
            {
                m_Animator.SetBool("ClickBack", false);
            }
            if (InputManager.IsClickDownMouseLeft() || InputManager.IsClickDownMouseRight() && !isHurt)
            {
                if (m_AttackWaitCoroutine != null)
                {
                    StopCoroutine(m_AttackWaitCoroutine);
                }
                m_AttackWaitCoroutine = StartCoroutine(AttackWait());
            }

            if (m_Attack)
            {
                m_moveBehaviour.isAttack = true;
            }
            else
            {
                m_moveBehaviour.isAttack = false;
            }
            if (m_WeaponAttack)
            {
                m_WeaponAttackPoint_R.SetAttackPoint();
                m_WeaponAttackPoint_L.SetAttackPoint();
            }
        }

        private bool IsDefaultAnimPlay()
        {
            if (m_Animator.GetCurrentAnimatorClipInfoCount(3) >= 1)
            {
                return true;
            }
            return false;
        }

        #region LockLogic
        public void OnSetLock()
        {
            if (m_LockEnemy)
            {
                if (chnageLockCount < m_FrontEnemyObj.Count)
                {
                    int i = GetNextLockIndex();
                    currentLockEnemy = m_FrontEnemyObj[i].GetComponent<EnemyLogic>();
                    m_moveBehaviour.SetLockLookAt(currentLockEnemy.LockTransform);
                    if (m_ProcedureMain != null)
                    {
                        m_ProcedureMain.HideLockIcon();
                    }
                    if (m_ProcedureMain != null)
                    {
                        m_ProcedureMain.ShowLockIcon(currentLockEnemy.LockTransform);
                    }
                    m_LockEnemy = true;
                    m_lookAnimator.SetLookTarget(currentLockEnemy.transform);
                }
                else
                {
                    m_moveBehaviour.UnLockLookAt();
                    if (m_ProcedureMain != null)
                    {
                        m_ProcedureMain.HideLockIcon();
                    }
                    currentLockEnemy = null;
                    m_lookAnimator.SetLookTarget(null);
                    m_LockEnemy = false;
                }
            }
            else
            {
                GetFrontEnemy();
                frontEnemyNum = m_FrontEnemyObj.Count;
                if (frontEnemyNum > 0)
                {
                    currentLockEnemy = m_FrontEnemyObj[GetNearestEnemyIndex()].GetComponent<EnemyLogic>();
                    m_moveBehaviour.SetLockLookAt(currentLockEnemy.LockTransform);
                    if (m_ProcedureMain != null)
                    {
                        m_ProcedureMain.ShowLockIcon(currentLockEnemy.LockTransform);
                    }
                    TakeOutWeapon();//空手状态锁定敌人自动拿出武器
                    m_LockEnemy = true;
                    m_lookAnimator.SetLookTarget(currentLockEnemy.transform);
                }
            }
        }

        private void LockOnEnemyDead()
        {
            if (currentLockEnemy != null && currentLockEnemy.IsDead)
            {
                OnSetLock();
            }
        }
        private int GetNextLockIndex()
        {
            chnageLockCount++;
            currentLockEnemyIndex++;
            return currentLockEnemyIndex % m_FrontEnemyObj.Count;
        }

        private void GetFrontEnemy()
        {
            chnageLockCount = 1;
            m_FrontEnemyObj.Clear();
            GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemyObjects.Length; i++)
            {
                float distance = AIUtility.GetDistance(enemyObjects[i].GetComponent<Entity>(), this);
                if (distance < Constant.PlayerParameter.LOCK_DISTANCE)
                {
                    Vector3 relativePosition = enemyObjects[i].transform.position - transform.position;
                   // float result = Vector3.Dot(m_moveBehaviour.playerCamera.transform.forward, relativePosition);
                    float result = Vector3.Dot(m_moveBehaviour.playerCamera.transform.forward, relativePosition);
                    if (result > 0)
                    {
                        m_FrontEnemyObj.Add(enemyObjects[i]);
                    }
                }
            }
        }

        private int GetNearestEnemyIndex()
        {
            int m_index = 0;
            float indexDistance = 0;
            for (int i = 0; i < m_FrontEnemyObj.Count; i++)
            {
                float distance = AIUtility.GetDistance(m_FrontEnemyObj[i].GetComponent<Entity>(), this);
                if (i == 0)
                {
                    indexDistance = distance;
                }
                else
                {
                    if (indexDistance > distance)
                    {
                        m_index = i;
                        indexDistance = distance;
                    }
                }
            }
            return currentLockEnemyIndex = m_index;
        }

        #endregion

        #region About put and take weapon

        public void PutDownWeapon()
        {
            if (!isHands)
            {
                return;
            }
            IsHands = !IsHands;
            m_Animator.SetBool("IsHand", IsHands);
            m_Animator.SetTrigger("PutOrTakeTrigger");
            //isPutting = false;
        }

        public void TakeOutWeapon()
        {
            if (IsHands)
            {
                return;
            }
            IsHands = !IsHands;
            m_Animator.SetBool("IsHand", IsHands);
            m_Animator.SetTrigger("PutOrTakeTrigger");
            //m_Animator.SetTrigger("TakeOutWeaponTrigger");
        }

        public void TakeOutWeaponWhenAtk()
        {
            if (IsHands)
            {
                return;
            }
            IsHands = !IsHands;
            m_Animator.SetBool("IsHand", IsHands);
        }

        #endregion

        #region About ChangeWeaponLogic
        private void ChangeWeaponRight(int weaponId)
        {
            ResetFocusEngy();

            var oldEntity = GameEntry.Entity.GetEntity(weaponDatas[0].Id);
            if (oldEntity != null)
            {
                GameEntry.Entity.DetachEntity(weaponDatas[0].Id);
                GameEntry.Entity.HideEntity(oldEntity);
                //oldEntity.OnHide(false,null);
            }
            weaponDatas[0] = new WeaponData(GameEntry.Entity.GenerateSerialId(), weaponId, Id, CampType.Player);
            GameEntry.Entity.ShowWeapon(weaponDatas[0], EntityExtension.WeaponHand.Right);
        }
        private void ChangeWeaponLeft(int weaponId)
        {
            var oldEntity = GameEntry.Entity.GetEntity(weaponDatas[1].Id);
            if (oldEntity != null)
            {
                GameEntry.Entity.DetachEntity(weaponDatas[1].Id);
                GameEntry.Entity.HideEntity(oldEntity);
                //oldEntity.OnHide(false, null);
            }
            weaponDatas[1] = new WeaponData(GameEntry.Entity.GenerateSerialId(), weaponId, Id, CampType.Player);
            GameEntry.Entity.ShowWeapon(weaponDatas[1], EntityExtension.WeaponHand.Left);
        }

        private void CleanHandleWeapon()
        {
            for (int i = 0; i < weaponDatas.Count; i++)
            {
                var thisEntity = GameEntry.Entity.GetEntity(weaponDatas[i].Id);
                if (thisEntity == null)
                {
                    return;
                }
                GameEntry.Entity.DetachEntity(thisEntity);
                GameEntry.Entity.HideEntity(thisEntity);
            }
        }
        private void DestroyWeapon(int weaponId)
        {
            var thisEntity = GameEntry.Entity.GetEntity(weaponDatas[1].Id);
            if (thisEntity == null)
            {
                return;
            }
            GameEntry.Entity.DetachEntity(weaponDatas[1].Id);
            GameEntry.Entity.HideEntity(thisEntity);
            //thisEntity.OnHide(false, null);
        }
        #endregion

        #region Coroutine

        private IEnumerator AttackHoldWait()
        {
            m_Attack = true;
            yield return m_AttackHoldWait;
            m_FocusCount = 0;
            m_Animator.SetInteger("FocusCount", m_FocusCount);
            m_Attack = false;
        }

        private IEnumerator AttackWait()
        {
            m_Attack = true;

            if (InputManager.IsClickDownMouseLeft() && InputManager.IsClickDownMouseRight())
            {
                m_DoubleClick = true;
                m_IsAttackTap = false;
                m_IsAttackThump = false;
                m_IsAttackJump = false;
                
            }
            else if(InputManager.IsClickDownMouseRight())
            {
                m_IsAttackThump = true;
                m_IsAttackTap = false;
                m_DoubleClick = false;
                m_IsAttackJump = false;
           
            }
            else if(InputManager.IsClickDownMouseLeft()||InputManager.IsClickUpMouseLeft())
            {
                m_IsAttackTap = true;
                m_IsAttackThump = false;
                m_DoubleClick = false;
                m_IsAttackJump = false;
                Whirlwind = false;
            }
            else if (InputManager.IsClickDownMouseLeft()&& Input.GetKeyDown(KeyCode.W))
            {
                m_IsAttackTap =false;
                m_IsAttackThump = false;
                m_DoubleClick = false;
                m_IsAttackJump = true;
               
            }
          
            yield return m_AttackInputWait;
            m_DoubleClick = false;
            m_IsAttackTap = false;
            m_IsAttackThump = false;
            m_IsAttackJump = false;
            //Whirlwind = false;
            m_Attack = false;
        }

        //用于垫步完一定时间内可释放特殊攻击-垫步攻击 （目前好像没有垫步了）
        public void StartCoro(float m_time)
        {
            StartCoroutine(StepToAtkWait(m_time));
        }

        // 角色垫步后一秒内可执行特殊攻击
        private IEnumerator StepToAtkWait(float duringTime)
        {
            isCanStepAtk = true;

            yield return new WaitForSeconds(duringTime);
            isCanStepAtk = false;
        }

        // 角色蓄力冲刺
        public IEnumerator ShiftWait(float time)
        {
            isShift = true;
            yield return new WaitForSeconds(time);
            isShift = false;
        }


        #endregion

        #region About Atked

        //判断敌人攻击的方向 用于处理向背后倒还是向前倒
        private bool EnemyAtkedDir(Entity attacker)
        {
            Entity player = gameObject.GetComponent<TargetableObject>();
            float angle = AIUtility.GetAngleInSeek(player, attacker);
            if (angle > 110)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //重置蓄力时间（用于蓄力结束释放技能或者 蓄力中途取消）
        private void ResetFocusEngy()
        {
            m_ClickAttackBtnDuration = 0;
            m_Animator.SetFloat("ClickAttackBtnDuration", m_ClickAttackBtnDuration);
        }

        //记录普攻播放到第几招了 用于像蛮力肩撞这种可以和普工衔接的技能
        //比如普攻放完第二招释放蛮力肩撞 然后再接普攻 就应该释放普工第三招 但条件是在一定时间内
        private void RecordCombon()
        {
            if (!isCombo) return;
            comboTime += Time.deltaTime;
            if (comboTime >= 3.5f)
            {
                isCombo = false;
                attackCount = 0;
            }
        }
        #endregion

        #region InAnimation Event

        public void ShootArrow()
        {
            if (m_LockEnemy)
            {
                m_ArrorTransform = transform;
               // m_ArrowImpulse = m_DefaultImpulse;
                Vector3 dir = currentLockEnemy.transform.position - m_BowShootPoint.position;
                m_ArrowImpulse = new Vector3(dir.x,dir.y+7, dir.z);
            }
            else
            {
                if (m_IsOnBowAim)
                {
                    m_ArrorTransform = m_moveBehaviour.playerCamera.transform;
                }
                else
                {
                    m_ArrorTransform = transform;
                }
                m_ArrowImpulse = m_DefaultImpulse;
            }

            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 30017)
            {
                Position = m_BowShootPoint.position,
                Rotation = transform.rotation,
            }) ;

            //GameEntry.Entity.ShowArrow(new ArrowData(GameEntry.Entity.GenerateSerialId(), 30015,weaponDatas[1].OwnerId, weaponDatas[1].OwnerCamp,weaponDatas[1].Attack)
            //{
            //    ArrowImpulse = m_ArrowImpulse,
            //    Position = m_BowShootPoint.position,
            //    Rotation = transform.rotation,
            //    Parent = m_ArrorTransform,
            //    IsLock =m_LockEnemy,
            //});
        }
        public void PlayerSound(int soundID,out int? ID)
        {
          ID = GameEntry.Sound.PlaySound(soundID);
        }
        public void GetUpAnimationEvent()
        {
            m_AnimationEventGetUp = true;
        }

        public void DeBuffAnimStart(BuffType buffType)
        {
            Buff.BuffTypeEnum = buffType;

        }

        public void DeBuffAnimEnd()
        {
            Buff.BuffTypeEnum = BuffType.None;

        }

        /// <summary>
        /// 霸体
        /// </summary>
        public void SuperArmorStart()
        {
            IsCanBreak = false;
        }

        public void SuperArmorEnd()
        {
            IsCanBreak = true;
        }

        public void FocusEneryCount(int count)
        {
            m_FocusCount = count;
        }
        public void AttackStart()
        {
            m_WeaponAttack = true;
            isAim = false;

            //switch (m_EquiState)
            //{
            //    case EquiState.None:
            //        break;
            //    case EquiState.SwordShield:
            //        GameEntry.Sound.PlaySound(WaveWeaponSound.SliceHandSwordWaveID[Utility.Random.GetRandom(0, WaveWeaponSound.SliceHandSwordWaveID.Count)]);
            //        break;
            //    case EquiState.GiantSword:
            //        GameEntry.Sound.PlaySound(WaveWeaponSound.GrantSwordWaveID[Utility.Random.GetRandom(0, WaveWeaponSound.GrantSwordWaveID.Count)]);
            //        break;
            //    case EquiState.Dagger:
            //        GameEntry.Sound.PlaySound(WaveWeaponSound.DaggerWaveID[Utility.Random.GetRandom(0, WaveWeaponSound.DaggerWaveID.Count)]);
            //        break;
            //    case EquiState.DoubleBlades:
            //        GameEntry.Sound.PlaySound(WaveWeaponSound.SliceHandSwordWaveID[Utility.Random.GetRandom(0, WaveWeaponSound.DaggerWaveID.Count)]);
            //        break;
            //    case EquiState.Pistol:
            //        break;
            //    default:
            //        break;
            //}
            if (!isFocusEngy && m_EquiState == EquiState.GiantSword)
            {
                m_Animator.SetFloat("ClickAttackBtnDuration", m_ClickAttackBtnDuration);
            }
            if (m_RightWeaponCollider != null)
                m_RightWeaponCollider.enabled = true;
            ShowTrail();
        }

        public void AttackEnd()
        {
            m_WeaponAttack = false;
            isThump = false;
            if (m_RightWeaponCollider != null)
                m_RightWeaponCollider.enabled = false;
            HideTrail();
        }

        public void FocusRecovery()//蓄力完毕修改武器
        {
            ChangeWeaponRight(30012);
            DestroyWeapon(30002);
        }
        public void PlayEffect(string parentName, int entityID, Vector3 rotate, float keepTime, Type type ,out int ID)
        {  
            GameEntry.Entity.ShowEffect(new EffectData(ID = GameEntry.Entity.GenerateSerialId(), entityID)
            {
                Position = FindTools.FindFunc<Transform>(transform, parentName).position,
                Rotation = Quaternion.Euler(rotate),
                KeepTime = keepTime,
                Owner = this

            }, type) ;

        }

        public void PlayEffect(string parentName, int entityID, Vector3 rotate, float keepTime,bool isFollow ,out int ID)
        {
            GameEntry.Entity.ShowEffect(new EffectData(ID = GameEntry.Entity.GenerateSerialId(), entityID)
            {
                Position = FindTools.FindFunc<Transform>(transform, parentName).position,
                Rotation = Quaternion.Euler(rotate),
                KeepTime = keepTime,
                Owner = this,
                IsFollow = isFollow,
                ParentName = parentName
            }) ;
        }

        public void SetDefense(bool isDefense)
        {
            IsDefense = isDefense;
        }

        // 角色向前位移动画事件
        public void PlayerShiftStartAnim(float time)
        {
            shiftSpeed = 3f;
            StartCoroutine(ShiftWait(time));
        }

        #endregion

        #region AboutFsm

        private void CreateFsm()
        {
            AddFsmState();
            fsm = GameEntry.Fsm.CreateFsm(gameObject.name, this, stateList);
            StartFsm();
        }

        private void StartFsm()
        {
            fsm.Start<PlayerMotionState>();
        }

        private void DestroyFsm()
        {
            GameEntry.Fsm.DestroyFsm(fsm);
            for (int i = 0; i < stateList.Count; i++)
            {
                ReferencePool.Release((IReference)stateList[i]);
            }
            stateList.Clear();
            fsm = null;
        }

        private void AddFsmState()
        {
            stateList.Add(PlayerIdleState.Create());
            stateList.Add(PlayerMotionState.Create());
            stateList.Add(PlayerAttackState.Create());
            stateList.Add(PlayerDefenseState.Create());
            stateList.Add(PlayerDodgeState.Create());
            stateList.Add(PlayerHurtState.Create());
            stateList.Add(PlayerEquipState.Create());
            stateList.Add(PlayerKnockedDownState.Create());
            stateList.Add(PlayerGetUpState.Create());
            stateList.Add(PlayerStepState.Create());
            stateList.Add(PlayerEquipWeaponState.Create());
            stateList.Add(PlayerFocusEnergyState.Create());
            stateList.Add(PlayerSkillState.Create());
            stateList.Add(PlayerSecondSkillState.Create());
            stateList.Add(PlayerKnockedBackState.Create());
            stateList.Add(PlayerKnockedFlyState.Create());
        }
        #endregion

        #region About ShiftLogic
        /// <summary>
        /// 垫步的位移
        /// </summary>
        public void StepMove(Transform player, float LR, float FB, float moveBlend, float speed)
        {
            if (moveBlend != 0)
            {
                PlayerFrontShift(player, speed);
            }
            else
            {
                if (LR > 0)
                {
                    player.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);
                }
                else if (LR < 0)
                {
                    player.Translate(Vector3.left * Time.deltaTime * speed, Space.Self);
                }
                if (FB > 0)
                {
                    player.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
                }
                else if (FB < 0)
                {
                    player.Translate(Vector3.back * Time.deltaTime * speed, Space.Self);
                }

            }
        }

        //角色向前位移
        public void PlayerFrontShift(Transform player, float speed)
        {
            player.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
        }
        public void PlayerShiftInUpdate(bool isShift, float speed)
        {
            if (isShift) PlayerFrontShift(transform, speed);
        }
        #endregion

        #region About PistolLogic(火枪)

        public void FireBullet()
        {
            if (m_EquiState != EquiState.Pistol)
            {
                return;
            }
            //Vector3 aimV3 = Camera.main.ScreenToWorldPoint(m_ProcedureMain.m_GunAimForm.transform.position);

            GameObject obj = null;
            GameObject bullet = null;
            Transform fireDir = null;

            for (int i = 0; i < Weapons.Count; i++)
            {
                if (Weapons[i] is WeaponLogicRightHand)
                {
                    obj = Weapons[i].gameObject;
                    bullet = obj.transform.Find("Bullet").gameObject;
                    fireDir = obj.transform.Find("FireDir");
                }
            }
            if (bullet)
            {
                Vector3 inputv3 = Input.mousePosition;
                Vector3 pos = Camera.main.WorldToScreenPoint(fireDir.position);
                Vector3 m_MousePos = new Vector3(inputv3.x, inputv3.y, pos.z);
                fireDir.position = Camera.main.ScreenToWorldPoint(m_MousePos);

                Vector3 rayOrigin = fireDir.position;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, int.MaxValue))
                {
                    var clone = Instantiate(bullet, bullet.transform.position, bullet.transform.rotation);
                    clone.transform.position = fireDir.position;
                    clone.SetActive(true);
                    //clone.GetComponent<Rigidbody>().AddForce(fireDir.forward * 5f, ForceMode.Impulse);
                    clone.GetComponent<Rigidbody>().velocity = (hit.point - clone.transform.position) * 5f;
                }

            }
        }

        private void OnSetGunAim(bool isAim)
        {
            //if (isAim)
            //{
            //    m_ProcedureMain.ShowGunAimIcon();
            //    m_moveBehaviour.m_thirdPersonOrbit.SetAim();
            //}
            //else
            //{
            //    m_ProcedureMain.HideGunAimIcon();
            //    m_moveBehaviour.m_thirdPersonOrbit.UnSetAim();
            //}
        }


        #endregion

        #region About DamageLogic
        public override ImpactData GetImpactData()
        {
            return new ImpactData(m_PlayerData.Camp, m_PlayerData.HP, m_PlayerData.Attack, m_PlayerData.Defense);
        }
        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.tag == "Effects" )
        //    {
        //        //ApplyDamage(other.transform.parent.gameObject.Entity, 50,Vector3.zero);
        //    }
        //}
        public override void ApplyDamage(Entity attacker, int damageHP, Vector3 weapon)
        {
            //int HurtDefense = Animator.StringToHash("HurtDefense");

            //翻滚 、 肩撞时无敌
            if (isDodge || isShoulder)
            {
                return;
            }

            if (attacker != null)
            {
                m_Attacker = attacker.gameObject;
            }
            else
            {
                return;
            }
            isBehindAtked = EnemyAtkedDir(attacker);//判断是否是从背后受到的攻击
            if (IsDefense && !isBehindAtked && m_PlayerData.MP>0)
            {
                Debug.Log("格挡成功");
                underAttack = true;
                //m_Animator.SetTrigger(HurtDefense);
                GameEntry.Event.Fire(attacker, ApplyDefenseEventArgs.Create(this));
                return;
            }
            //if (Invulnerable)
            //{
            //    return;
            //}
            base.ApplyDamage(attacker, damageHP,weapon);
            GameEntry.Event.Fire(attacker, ApplyDamageEventArgs.Create(this));
           // Invulnerable = true;
        }

        #endregion

        #region WeaponTrail
        public void ShowTrail()
        {
            m_WeaponAttackPoint_R.gameObject.GetComponent<WeaponTrail>().ShowTrail();
            if (m_WeaponAttackPoint_L)
            {
                m_WeaponAttackPoint_L.gameObject.GetComponent<WeaponTrail>().ShowTrail();
            }
        }

        public void HideTrail()
        {
            m_WeaponAttackPoint_R.gameObject.GetComponent<WeaponTrail>().HideTrail();
            if (m_WeaponAttackPoint_L)
            {
                m_WeaponAttackPoint_L.gameObject.GetComponent<WeaponTrail>().HideTrail();
            }
        }
        #endregion

        #region PlayerEffect

        protected void ShowRunEffect()
        {
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70007),
                typeof(PlayerMoveEffectLogic));
        }

        #endregion

        #region Collider(碰撞相关)

        /// <summary>
        /// 此事件处理武器碰撞相关
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            var enemyWeapon = other.GetComponent<WeaponAttackPoint>();
            if (enemyWeapon != null)
            {
                if (enemyWeapon.GetComponent<BoxCollider>().enabled && m_RightWeaponCollider.enabled)
                {
                    //只处理敌人正前方的拼刀
                    //Vector3 targetDir = enemy.transform.position - transform.position;
                    //targetDir.y = 0;
                    //float angle = Vector3.SignedAngle(targetDir, transform.forward,Vector3.up);
                    //angle < 45f && angle > -45f
                    EnemyLogic enemy = enemyWeapon.GetComponentInParent<EnemyLogic>();
                    float angle = AIUtility.GetPlaneAngle(enemy, Entity);
                    if (AIUtility.CheckInAngle(-45f, 45f, angle))
                    {

                        //若碰撞检测启动意味着敌人正在攻击
                        //更新敌人和玩家招架布尔值
                        enemy.EnemyAttackEnd();
                        enemyWeapon.GetComponentInParent<EnemyLogic>().IsParry = true;
                        IsParry = true;//玩家自身招架暂时没有使用
                        //Debug.Log("拼刀拼刀拼刀");
                    }

                }
            }
        }

        /// <summary>
        /// 此事件处理身体碰撞相关
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            var enemy = collision.collider.GetComponent<EnemyLogic>();
            if (enemy != null)
            {
                if (isPushAttack)
                {
                    //冲刺技能造成伤害
                    Debug.Log(enemy.name);
                    isPushAttack = false;
                    //m_Animator.SetBool("InPushAttack", false);
                  //  enemy.ApplyDamage(RightHand, RightHand.weaponData.Attack);
                }
            }

        }

        #endregion

    }
}
