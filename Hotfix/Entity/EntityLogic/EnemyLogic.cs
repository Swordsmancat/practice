using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using System.Collections.Generic;
using System.Collections;
using Pathfinding;
using System;
using GameFramework;
using Sirenix.OdinInspector;
using GameFramework.Event;

namespace Farm.Hotfix
{
    public class EnemyLogic : TargetableObject
    {
        #region AboutSerializeField

        [SerializeField]
        private EnemyData m_EnemyData = null;                           //敌人数据


        #endregion

        #region AboutPrivateField

        private Coroutine m_DeadWaitCoroutine;                      //死亡延时协程
        private readonly string PlayerTag = "Player";               //玩家标签
        private readonly string LockPositionName = "LockPosition";  //玩家锁定怪物的位置
        private Transform m_LockTransform;                          //当前怪物锁定目标的transform

        #endregion

        #region AboutProtectedField
        public IFsm<EnemyLogic> fsm;                     //状态机
        public List<FsmState<EnemyLogic>> stateList;     //状态机状态列表

        protected float m_PrevAttackTimeHP = 0;               //上次被攻击时的血量

        #endregion

        #region AboutPublicField

        public EnemyData enemyData => m_EnemyData;                      //敌人数据
        public Transform LockTransform
        {
            get
            {
                return m_LockTransform;
            }
        }  
        public TargetableObject LockingEntity { get; set; }             //获取玩家实体
        public WeaponAttackPoint WeaponAttackPoint_L = null;            //左手
        public WeaponAttackPoint WeaponAttackPoint_R = null;            //右手
        public bool m_WeaponAttack = false;                             //攻击点是否激活
        public bool IsLocking = false;                                  //是否锁定
        public bool IsCanAttack = false;                                //是否可攻击
        public bool IsUseSkill = false;                                 //是否使用技能
        public bool IsHurt = false;                                     //是否受伤
        public bool IsAnimPlayed = false;                               //动画是否播放完毕
        public bool IsParry = false;                                    //是否招架(拼刀)
        public bool IsBlock = false;                                    //是否格挡
        public bool IsThump = false;                                    //是否重击
        public bool IsRoll = false;                                     //是否闪避
        public PlayerLogic find_Player;                                 //玩家逻辑
        public float ReduceAttackTime = 0;                              //减少攻击时间
        public float AttackTimer = 0;                                   //攻击Cd
        public float CurrentTargetDisdance = 0;                         //当前目标与敌人之间的距离
        public WeaponLogicRightHand RightHand = null;                   //右手武器逻辑
        public WeaponLogicLeftHand LeftHand = null;                     //左手武器逻辑
        public BoxCollider BoxColliderRight = null;                     //右手武器碰撞
        public BoxCollider BoxColliderLeft = null;                      //左手武器碰撞
        public Vector3 OriginPoint;                                     //出生点
        public Quaternion OriginRotate;                                 //出生旋转角度
        public float Energy;                                            //精力值
        public float MaxEnergy;                                         //最大精力
        public int hurtNum;                                             //受击计数
        public bool underAttack;                                        //是否被攻击
        public bool isBehindAtked;                                      //前后方位
        //public bool toParry=true;                                     //格挡受击间隔
        public bool IsWeak;                                             //空精状态
        public float WeakTime = 5;                                      //空精恢复时间
        public float Stoic_Time = 0;                                    //霸体持续时间
        public bool Stoic = false;                                      //霸体
        public bool Stoic_Enemy;                                        //反击霸体
        public int CounterattackNum = 0;                                //反击
        public bool isKnockedDown;                                      //被击倒
        public bool Is_Invincible;                                      //无敌状态
        public Vector2 attackDir;
        public bool m_AnimationEventGetUp;
        //public bool m_IsDefense;
        private EquiState m_EquiState;
        private float toParrytime;
        #endregion

        //听觉半径
        //此为测试代码后续可将其填表加入
        private int HearRange = 5;

        #region A*

        private Vector3? m_AISearchTarget;      //目标位置

        public IAstarAI m_AI;                  //寻路

        protected RichAI m_RichAI;              //寻路方式

        protected AIPath m_AIPath;
        #endregion

        #region Base
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            stateList = new List<FsmState<EnemyLogic>>();
            //  m_EnemyBehavior = GetComponent<EnemyBehavior>();
        }

        public void RestoreEnergy()
        {
            float currentEnergy;
            //if (Energy < 100)
            currentEnergy = Energy + (20 * Time.deltaTime);
            Energy = currentEnergy > MaxEnergy ? MaxEnergy : currentEnergy;

        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_EnemyData = userData as EnemyData;
            
            if (m_EnemyData == null)
            {
                Log.Error("Enemy data is invalid");
                return;
            }
            //获取武器数据
            List<WeaponData> weaponDatas = m_EnemyData.GetAllWeaponDatas();


            //for (int i = 0; i < weaponDatas.Count; i++)
            //{
            //    // GameEntry.Entity.ShowWeapon(weaponDatas[i]);
            //    GameEntry.Entity.ShowRightWeapon(weaponDatas[i]);
            //}

            //显示武器
            GameEntry.Entity.ShowWeapon(weaponDatas[0], EntityExtension.WeaponHand.Left);
            GameEntry.Entity.ShowWeapon(weaponDatas[1], EntityExtension.WeaponHand.Right);
            if (weaponDatas[0].EquiState != EquiState.None)
            {
                m_EquiState = weaponDatas[0].EquiState;
            }
            else if(weaponDatas[1].EquiState != EquiState.None)
            {
                m_EquiState = weaponDatas[1].EquiState;
            }
            else
            {
                m_EquiState = EquiState.None;
            }
            
            m_LockTransform = FindTools.FindFunc<Transform>(transform, LockPositionName);
            find_Player = GameObject.FindGameObjectWithTag(PlayerTag).GetComponent<PlayerLogic>();
            InitAStarAI();
            SetAIData();
            CreateFsm();
            
            OriginPoint = transform.position;
            OriginRotate = transform.rotation;
            IsCanAttack = false;
        }

        private void SetAIData()
        {
            m_RichAI.maxSpeed = m_EnemyData.MoveSpeedRun;
            //m_AIPath.maxSpeed = m_EnemyData.MoveSpeed;
            m_PrevAttackTimeHP = m_EnemyData.HP;
            Energy = m_EnemyData.MP;
            MaxEnergy = m_EnemyData.MP;
            //Debug.Log("Energy" + Energy);
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
            //添加武器逻辑脚本和攻击点脚本
            if (childEntity is WeaponLogicLeftHand)
            {
                Weapons.Add((WeaponLogicLeftHand)childEntity);
                WeaponAttackPoint_L = childEntity.GetComponent<WeaponAttackPoint>();
                LeftHand = (WeaponLogicLeftHand)childEntity;
                BoxColliderLeft = childEntity.GetComponent<BoxCollider>();
                return;
            }
            if (childEntity is WeaponLogicRightHand)
            {
                Weapons.Add((WeaponLogicRightHand)childEntity);
                WeaponAttackPoint_R = childEntity.GetComponent<WeaponAttackPoint>();
                RightHand = (WeaponLogicRightHand)childEntity;
                BoxColliderRight = childEntity.GetComponent<BoxCollider>();
                return;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (m_WeaponAttack)
            {
                if (WeaponAttackPoint_L != null)
                    WeaponAttackPoint_L.SetAttackPoint();
                if (WeaponAttackPoint_R != null)
                    WeaponAttackPoint_R.SetAttackPoint();
            }
            //toParrytime += Time.deltaTime;
            //if (toParrytime >= 0.5f)
            //{
            //    toParry = true;
            //    toParrytime = 0;
            //}
            FixWeaponPostion();
            OnAiSearchUpdate();
            CurrentDisdanceUpdate();
        }

        protected override void OnDead(Entity attacker,Vector3 point)
        {
            m_DeadWaitCoroutine = StartCoroutine(DeadWait(attacker,point));

            if (m_DeadWaitCoroutine != null)
            {
                StartCoroutine(DeadWait(attacker,point));
                StartCoroutine(DeadWait(attacker,point));
            }

            StopCoroutine(m_DeadWaitCoroutine);
            DestroyAStarAi();
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            DestroyFsm();
        }

        public override ImpactData GetImpactData()
        {
            return new ImpactData(m_EnemyData.Camp, m_EnemyData.HP, m_EnemyData.Attack, m_EnemyData.Defense);
        }

        public override void ApplyDamage(Entity attacker, int damageHP, Vector3 weapon) //受到伤害
        {
            if (m_EnemyData.HP <= 0)
            {
                //怪物死亡不再执行
                //重载该函数需加载到调用父类之前
                return;
            }
            //if (IsParry)
            //    return;
            isBehindAtked = (AIUtility.GetDot(this, attacker))>0;
            if (IsDefense && isBehindAtked && Energy > 0 ) //
            {
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
            if (IsRoll) //闪避
            {
                return;
            }


            //打击感时间函数
            //增加判断条件防止覆盖死亡慢动作
            //此处根据武器攻击力判断
            if (m_EnemyData.HP >= damageHP)
            {
                //巨剑和重击调用
                //if(重剑or重击)
                if (find_Player.EquiState == EquiState.GiantSword)
                {
                    TimeStop();
                }
            }

        }

        #endregion

        #region AStar

        public void SetRichAIMove(float speed =0)
        {
            if (m_AI != null)
            {
                m_AI.isStopped = false;
                if(speed == 0)
                {
                 m_AI.maxSpeed = enemyData.MoveSpeedRun;
                }
                else
                {
                    m_AI.maxSpeed = speed;
                }
            }
        }
      
        public void SetRichAiStop()
        {
            if (m_AI != null)
            {
                m_AI.isStopped = true;
                m_AI.maxSpeed = 0f;
            }
        }

        /// <summary>
        /// 此函数更新目标位置
        /// </summary>
        /// <param name="transform">玩家的位置</param>
        public void SetSearchTarget(Transform transform)
        {
            m_AISearchTarget = transform.position;
        }

        /// <summary>
        /// 此函数更新目标位置
        /// </summary>
        /// <param name="pos">玩家位置</param>
        public void SetSearchTarget(Vector3 pos)
        {
            m_AISearchTarget = pos;
        }

        private void InitAStarAI()
        {
            m_AI = GetComponent<IAstarAI>();
            if (m_AI == null)
            {
                m_AI = gameObject.AddComponent<RichAI>();
            }
            m_RichAI = GetComponent<RichAI>();
            m_AIPath = GetComponent<AIPath>();
            m_AI.onSearchPath += OnAiSearchUpdate;
        }

        private void DestroyAStarAi()
        {
            if (m_AI != null)
            {
                m_AI.onSearchPath -= OnAiSearchUpdate;
            }
        }

        public void OnAiSearchUpdate()
        {
            if (m_AISearchTarget != null && m_AI != null)
            {
                m_AI.destination = (Vector3)m_AISearchTarget;
            }
        }

        public IAstarAI GetAICompoent()
        {
            return m_AI;
        }

        #endregion

        #region Fsm
        private void CreateFsm()
        {
            AddFsmState();
            fsm = GameEntry.Fsm.CreateFsm<EnemyLogic>(gameObject.name, this, stateList);
            StartFsm();
        }

        public virtual Type ChangeStateEnemy(EnemyStateType stateType)
        {
            switch (stateType)
            {
                case EnemyStateType.Idle:
                    return typeof(EnemyIdleState);
                case EnemyStateType.Motion:
                    return typeof(EnemyMotionState);
                case EnemyStateType.Hurt:
                    return typeof(EnemyHurtState);
                case EnemyStateType.Attack:
                    return typeof(EnemyAttackState);
                case EnemyStateType.Dead:
                    return typeof(EnemyDeadState);
                case EnemyStateType.Block:
                    return typeof(EnemyBlockState);
                case EnemyStateType.Parry:
                    return typeof(EnemyParryState);
                case EnemyStateType.Rotate:
                    return typeof(EnemyRotateState);
                case EnemyStateType.Shout:
                    return typeof(EnemyShoutState);
                case EnemyStateType.OutOfFight:
                    return typeof(EnemyOutOfTheFight);
                case EnemyStateType.Fight:
                    return typeof(EnemyFightState);
                default:
                    return typeof(EnemyIdleState);
            }
        }

        protected virtual void StartFsm()
        {
            fsm.Start<EnemyIdleState>();
        }

        protected virtual void AddFsmState()
        {
            stateList.Add(EnemyIdleState.Create());
            stateList.Add(EnemyMotionState.Create());
            stateList.Add(EnemyAttackState.Create());
            stateList.Add(EnemyDeadState.Create());
            stateList.Add(EnemyHurtState.Create());
            stateList.Add(EnemyFightState.Create());
            stateList.Add(EnemyRotateState.Create());
            stateList.Add(EnemyOutOfTheFight.Create());
            stateList.Add(EnemyShoutState.Create());
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

        #endregion

        #region AboutRange

        public bool CheckInAttackRange(float distance)
        {
            return distance <= enemyData.AttackRange;
        }

        public bool CheckInSeekRange(float distance)
        {
            return distance <= enemyData.SeekRange;
        }

        public bool CheckInSeekAngle(float angle)
        {
            return angle < enemyData.AttackAngle / 2 && angle > -enemyData.AttackAngle / 2;
        }

        #endregion

        #region AboutLockPlayer
        public void UnLockEntity()
        {
            LockingEntity = null;
            IsLocking = false;
            m_AISearchTarget = null;
            SetRichAiStop();
        }

        public void LockEntity(TargetableObject targetable)
        {
            LockingEntity = targetable;
            IsLocking = true;
            SetRichAIMove();
        }
        public void WindLockEntity(TargetableObject targetable)
        {
            LockingEntity = targetable;
            IsLocking = true;
            m_AI.isStopped = false;
            m_AI.maxSpeed = 0.1f;
        }
        public void AttackLockEntity(TargetableObject targetable)
        {
            LockingEntity = targetable;
            IsLocking = true;
            m_AI.isStopped = false;
            m_AI.maxSpeed = 0.01f;
        }
        public void Jumpity(TargetableObject targetable)
        {
            LockingEntity = targetable;
            IsLocking = true;
            m_AI.isStopped = false;
            m_AI.maxSpeed = 50f;
        }



        #endregion

        #region AnimationEvent

        public void PlayerSound(int soundID,out int? ID)
        {
           ID =  GameEntry.Sound.PlaySound(soundID);
        }
        public void PlayEffect(string parentName,int entityID,Vector3 rotate, float keepTime, Type type ,out int ID )
        {
            GameEntry.Entity.ShowEffect(new EffectData(ID= GameEntry.Entity.GenerateSerialId(), entityID)
            {
                Position = FindTools.FindFunc<Transform>(transform, parentName).position,
                Rotation = Quaternion.Euler(rotate),
                //Scale = Scale,
                KeepTime = keepTime,
                Owner = this
            }, type); ;
           
        }

        public void PlayEffect(string parentName, int entityID, Vector3 rotate,  float keepTime, bool isfollow,out int ID)
        {
            GameEntry.Entity.ShowEffect(new EffectData(ID = GameEntry.Entity.GenerateSerialId(), entityID)
            {
                Position = FindTools.FindFunc<Transform>(transform, parentName).position,
                Rotation = Quaternion.Euler(rotate),
                //Scale = Scale,
                KeepTime = keepTime,
                Owner = this,
                IsFollow = isfollow,
                ParentName = parentName
            }) ;

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
        /// 攻击动画开始
        /// </summary>
        public  void EnemyAttackStart()
        {
            m_WeaponAttack = true;

            switch (m_EquiState)
            {
                case EquiState.None:
                    break;
                case EquiState.SwordShield:
                    GameEntry.Sound.PlaySound(WaveWeaponSound.SliceHandSwordWaveID[Utility.Random.GetRandom(0, WaveWeaponSound.SliceHandSwordWaveID.Count)]);
                    break;
                case EquiState.GiantSword:
                    GameEntry.Sound.PlaySound(WaveWeaponSound.GrantSwordWaveID[Utility.Random.GetRandom(0, WaveWeaponSound.GrantSwordWaveID.Count)]);
                    break;
                case EquiState.Dagger:
                    GameEntry.Sound.PlaySound(WaveWeaponSound.DaggerWaveID[Utility.Random.GetRandom(0, WaveWeaponSound.DaggerWaveID.Count)]);
                    break;
                case EquiState.DoubleBlades:
                    GameEntry.Sound.PlaySound(WaveWeaponSound.SliceHandSwordWaveID[Utility.Random.GetRandom(0, WaveWeaponSound.DaggerWaveID.Count)]);
                    break;
                case EquiState.Pistol:
                    break;
                default:
                    break;
            }
            if (BoxColliderRight != null)
                BoxColliderRight.enabled = true;
            if (BoxColliderLeft != null)
                BoxColliderLeft.enabled = true;
            ShowTrail();
        }

        /// <summary>
        /// 攻击动画结束
        /// </summary>
        public void EnemyAttackEnd()
        {
            m_WeaponAttack = false;
            if (BoxColliderRight != null)
                BoxColliderRight.enabled = false;
            if (BoxColliderLeft != null)
                BoxColliderLeft.enabled = false;
            HideTrail();
            IsThump = false;  //重击
        }

        /// <summary>
        /// 动画结束
        /// </summary>
        public virtual void AnimationEnd()
        {
            IsAnimPlayed = true;
        }

        /// <summary>
        /// 动画开始
        /// </summary>
        public virtual void AnimationStart()
        {
            IsAnimPlayed = false;
        }

        /// <summary>
        /// 显示武器轨迹
        /// </summary>
        private void ShowTrail()
        {
            if(WeaponAttackPoint_R != null)
            {
                WeaponTrail weaponTrail = WeaponAttackPoint_R.gameObject.GetComponent<WeaponTrail>();
                if (weaponTrail != null)
                    weaponTrail.ShowTrail();
            }
            if(WeaponAttackPoint_L != null)
            {
                WeaponTrail weaponTrail = WeaponAttackPoint_L.gameObject.GetComponent<WeaponTrail>();
                if (weaponTrail != null)
                    weaponTrail.ShowTrail();
            }
        }

        /// <summary>
        /// 隐藏武器轨迹
        /// </summary>
        public void HideTrail()
        {
            if(WeaponAttackPoint_R != null)
            {
                WeaponTrail weaponTrail = WeaponAttackPoint_R.gameObject.GetComponent<WeaponTrail>();
                if (weaponTrail != null)
                    weaponTrail.HideTrail();
            }
            if (WeaponAttackPoint_L != null)
            {
                WeaponTrail weaponTrail = WeaponAttackPoint_L.gameObject.GetComponent<WeaponTrail>();
                if (weaponTrail != null)
                    weaponTrail.HideTrail();
            }

        }

        /// <summary>
        /// 敌人重击
        /// </summary>
        public void ThumpAttack()
        {
            IsThump = true;
        }

        public void AttackTurnAround()
        {
            if (IsLocking)
            {
                float angle = AIUtility.GetAngleInSeek(this, LockingEntity);
                if (AIUtility.GetDot(this, LockingEntity) < 0)
                {
                    angle /= 2;
                }
                if (AIUtility.GetCross(this, LockingEntity) < 0)
                {
                    angle = -angle;
                }
                transform.rotation = Quaternion.Euler(0,
                      transform.localRotation.eulerAngles.y + angle, 0);

            }
        }

        #endregion

        #region AboutDoAttack
        /// <summary>
        /// 执行攻击
        /// </summary>
        public void PerformAttack()
        {
            IsCanAttack = true;
        }

        /// <summary>
        /// 重置攻击
        /// </summary>
        public void ResetAttack()
        {
            IsCanAttack = false;
            AttackTimer = 0;
        }

        #endregion

        #region AboutCoroutine

        private IEnumerator DeadWait(Entity attacker,Vector3 point)
        {
            //死亡慢动作
            Time.timeScale = 0.05f;

            //等待2秒后恢复正常时间
            yield return new WaitForSecondsRealtime(0);
            Time.timeScale = 1f;

            //等待2秒后摧毁敌人
            yield return new WaitForSecondsRealtime(4);
            base.OnDead(attacker,point);
        }

        #endregion

        #region HitFeel

        private bool m_Stopping;

        /*普通顿帧*/
        //顿针时间
        public float comnStopTime = 0.04f;
        //慢动作时间
        public float comnSlowTime = 0.08f;

        /*特殊顿帧*/
        //持续时间
        public float continuedTime = 1f;
        //时间流逝倍速
        public float timeScale = 0.01f;

        //大剑顿帧 敌人掉血时触发 特殊顿帧目前为大剑蓄力屠龙斩 打中怪物触发
        protected void TimeStop()
        {
            if (!m_Stopping)
            {
                m_Stopping = true;
                if (find_Player.isSpecialAtk)
                {
                    StartCoroutine(SpecialStop(continuedTime, timeScale));
                }
                else
                {
                    find_Player.m_Animator.speed = 0.01f;
                    StartCoroutine(CommonStop(comnStopTime, comnSlowTime));
                }
            }
        }

        IEnumerator CommonStop(float m_comnStopTime, float m_comnSlowTime)
        {
            yield return new WaitForSecondsRealtime(m_comnStopTime);
            find_Player.m_Animator.speed = 0.1f;

            yield return new WaitForSecondsRealtime(m_comnSlowTime);
            find_Player.m_Animator.speed = 1;
            m_Stopping = false;
        }
        IEnumerator SpecialStop(float m_ContinuedTime, float m_TimeScale)
        {
            Time.timeScale = m_TimeScale;
            yield return new WaitForSecondsRealtime(m_ContinuedTime);
            Time.timeScale = 1f;
            m_Stopping = false;
            find_Player.AttackEnd();
            find_Player.HideTrail();
        }
        #endregion

        #region EnemyHear
        public bool HearSound()
        {
            //目前玩家不能潜行
            //if(sneak){return false}

            float distance = AIUtility.GetCheckPlaneDistance(transform, find_Player.transform);
            if(distance != 0)
            {
                if (distance <= HearRange)
                {
                    return true;
                }
            }

            return false;
        }


        #endregion

        #region PrivateUtility
        /// <summary>
        /// 更新当前怪物距离玩家的距离
        /// </summary>
        private void CurrentDisdanceUpdate()
        {
            if (IsLocking)
            {
                //锁定玩家
               // Log.Info("锁定玩家");
                if(m_AISearchTarget != null)
                    CurrentTargetDisdance = AIUtility.GetDistance(transform.position,(Vector3)m_AISearchTarget);
            }
            else
            {
                //锁定生成点
               // Log.Info("锁定出生点");
                if (m_AISearchTarget != null)
                    CurrentTargetDisdance = AIUtility.GetDistance(transform.position, (Vector3)m_AISearchTarget);
            }
        }

        /// <summary>
        /// 固定怪物武器位置
        /// </summary>
        private void FixWeaponPostion()
        {
            if (RightHand != null)
            {
                RightHand.gameObject.transform.localPosition = new Vector3(0, 0, 0);
                RightHand.gameObject.transform.localRotation = Quaternion.identity;
            }

            if (LeftHand != null)
            {
                LeftHand.gameObject.transform.localPosition = new Vector3(0, 0, 0);
                LeftHand.gameObject.transform.localRotation = Quaternion.identity;
            }
        }

        #endregion

        public virtual void ApplyBuffEvent(BuffType buffType)
        {

        }
    }
}
