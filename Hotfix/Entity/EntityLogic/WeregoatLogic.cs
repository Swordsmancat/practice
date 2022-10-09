using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System;
using Pathfinding;
using GameFramework;

namespace Farm.Hotfix
{
    public class WeregoatLogic : EnemyLogic
    {
        private readonly static string s_BackWeapon = "BackWeapon";//后背武器点位名
        private readonly static string s_Weapon_L = "Weapon_L";//左手武器点位名
        private readonly static string s_Weapon_R = "Weapon_R";//右手武器点位名
        private readonly static string s_StartPoint = "StartPoint";//武器特效点位
        public BuffType m_BuffType;
        private GameObject BackWeaponObject;
        private GameObject Weapon_LObject;
        private GameObject Weapon_RObject;
        private GameObject Weapon_StartPoint;
        private readonly static int m_Posture = Animator.StringToHash("Posture");
        AnimatorStateInfo stateinfo;
        public GameObject BackWeapon
        {
            get { return BackWeaponObject; }
        }
        public GameObject StartPoint
        {
            get { return Weapon_StartPoint; }
        }
        protected override void OnDead(Entity attacker, Vector3 point)
        {
            
            Debug.Log("攻击数据");
            base.OnDead(attacker,point);
            EnemyAttackEnd();
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.DeadEffectId)
            {
                Position = point,
            },
                typeof(EnemyHurtEffectLogic));
        }
        //protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        //{
        //    base.OnUpdate(elapseSeconds, realElapseSeconds);
        //    stateinfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        //}
            public void BackWeaponOpen()
        {
            //BackWeaponObject = FindTools.FindFunc(transform, s_BackWeapon).gameObject;

            Log.Info("显示后背武器");
            BackWeaponObject.SetActive(true);
            Weapon_LObject.SetActive(false);
            Weapon_RObject.SetActive(false);
            //if (stateinfo.IsName("BackWeaponOpen") && stateinfo.length > 0.58f)
            //{
               
            //}

        }

        public void BackWeaponClose()
        {
            
            //Log.Info("进入关闭背后武器函数" + stateinfo.length);
            //BackWeaponObject = FindTools.FindFunc(transform, s_BackWeapon).gameObject;
            Log.Info("关闭后背武器");
            BackWeaponObject.SetActive(false);
            Weapon_LObject.SetActive(true);
            Weapon_RObject.SetActive(true);
            //if (stateinfo.IsName("BackWeaponClose") && stateinfo.normalizedTime > 0.22f)
            //{
                
            //}

        }
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            //Energy = 100f;
            hurtNum = 0;
        }
        protected override void AddFsmState()
        {
            Debug.Log("添加事件");
            
            //BackWeaponOpen();
            //Debug.Log("后背武器物体"+BackWeaponObject.name);
            stateList.Add(WeregoatIdleState.Create());
            stateList.Add(WeregoatMotionState.Create());
            stateList.Add(WeregoatAttackState.Create());
            //stateList.Add(EnemyDeadState.Create());
            stateList.Add(WeregoatDeadState.Create());
            stateList.Add(WeregoatHurtState.Create());
            //stateList.Add(EnemyHurtState.Create());
            //stateList.Add(EnemyFightState.Create());
            stateList.Add(WeregoatFightState.Create());
            stateList.Add(EnemyRotateState.Create());
            stateList.Add(EnemyOutOfTheFight.Create());
            stateList.Add(WeregoarParryState.Create());
            stateList.Add(WeregoarBlockState.Create());
            stateList.Add(EnemyKnockedDownState.Create());
            //stateList.Add(EnemyShoutState.Create());
            BackWeaponObject = FindTools.FindFunc(transform, s_BackWeapon).gameObject;
            Weapon_LObject = FindTools.FindFunc(transform, s_Weapon_L).gameObject;
            Weapon_RObject = FindTools.FindFunc(transform, s_Weapon_R).gameObject;
            Weapon_StartPoint = FindTools.FindFunc(transform, s_StartPoint).gameObject;
            Weapon_LObject.SetActive(false);
            Weapon_RObject.SetActive(false);
            

        }


        public override Type ChangeStateEnemy(EnemyStateType stateType)
        {
            
            switch (stateType)
            {
                case EnemyStateType.Idle:
                    //BackWeaponOpen();
                    //Debug.Log("站立");
                    return typeof(WeregoatIdleState);
                //return typeof(EnemyIdleState);
                case EnemyStateType.Motion:
                    //Debug.Log("移动");
                    //if (m_Animator.GetFloat(m_Posture)>0)
                    //{ BackWeaponClose(); }
                    return typeof(WeregoatMotionState);
                case EnemyStateType.Dead:
                    //return typeof(EnemyDeadState);
                    return typeof(WeregoatDeadState);
                case EnemyStateType.Hurt:
                    //return typeof(EnemyHurtState);
                    return typeof(WeregoatHurtState);
                case EnemyStateType.Attack:
                    return typeof(WeregoatAttackState);
                case EnemyStateType.Block:
                    return typeof(WeregoarBlockState);
                case EnemyStateType.Parry:
                    return typeof(WeregoarParryState);
                case EnemyStateType.Rotate:
                    return typeof(EnemyRotateState);
                //case EnemyStateType.Shout:
                //    return typeof(EnemyShoutState);
                case EnemyStateType.OutOfFight:
                    return typeof(EnemyOutOfTheFight);
                case EnemyStateType.Fight:
                    //return typeof(EnemyFightState);
                    return typeof(WeregoatFightState);
                case EnemyStateType.KnockedDown:
                    return typeof(EnemyKnockedDownState);
                default:
                    //return typeof(EnemyIdleState);
                    //BackWeaponOpen();
                    return typeof(WeregoatIdleState);
            }
        }

        public override void ApplyDamage(Entity attacker, int damageHP, Vector3 weapon)
        {

            //if (enemyData.HP <= 0 || enemyData.HP <= damageHP)
            //{
            //    base.ApplyDamage(attacker, damageHP);
            //    return;
            //}


           base.ApplyDamage(attacker, damageHP,weapon);

            //ApplyDamage会连续触发 若后续修改此代码需调整
            //是否受伤
            if (m_PrevAttackTimeHP != enemyData.HP)
            {
                IsHurt = true;
                GameEntry.Sound.PlaySound(enemyData.ByAttackSoundId);
                GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.BloodEffectId)
                {
                    Position = weapon,
                },
                    typeof(EnemyHurtEffectLogic));
                m_PrevAttackTimeHP = enemyData.HP;
            }
        }

        protected override void StartFsm()
        {
            //
            //fsm.Start<EnemyIdleState>();
            fsm.Start<WeregoatIdleState>();
            
        }
        public override void ApplyBuffEvent(BuffType buffType)
        {
            base.ApplyBuffEvent(buffType);
            m_BuffType = buffType;

        }
    }
}
