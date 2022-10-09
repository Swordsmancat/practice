using System;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using System.Collections.Generic;
using System.Collections;


namespace Farm.Hotfix
{
    public class HobgoblinLogic : EnemyLogic
    {
        private Transform zhanhou_Transform;
        private Transform chongci_Transform;
        private Transform feipu_Transform;
        public bool IsPush = true;
        float PP_Time = 0;
        AnimatorStateInfo Info;
        public bool Enemy_walkBack;



        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            zhanhou_Transform = FindTools.FindFunc<Transform>(transform, "zhanhou");
            chongci_Transform = FindTools.FindFunc<Transform>(transform, "chongci");
            feipu_Transform = FindTools.FindFunc<Transform>(transform, "feipu");
            

        }
        protected override void OnDead(Entity attacker, Vector3 point)
        {
            base.OnDead(attacker, point);
          //  GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(),enemyData.DeadEffectId),
          //      typeof(EnemyHurtEffectLogic));
        }

        protected static readonly int m_IsCanAttack = Animator.StringToHash("IsCanAttack");
        //protected static readonly int m_Stoic_Enemy = Animator.StringToHash("Stoic_Enemy");
        //protected static readonly int m_Counterattack = Animator.StringToHash("Counterattack");


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            m_Animator.SetBool(m_IsCanAttack, IsCanAttack);
            PP_Time += Time.deltaTime;
            //m_Animator.SetInteger(m_Counterattack,CounterattackNum);
            //m_Animator.SetBool(m_Stoic_Enemy, Stoic_Enemy);
            //Debug.Log("霸体 " + Stoic_Enemy);
            //Debug.Log("无敌 " + Enemy_walkBack);
            //Info_Hob = m_Animator.GetCurrentAnimatorStateInfo(0);
            //Enemy_walkBack = Info_Hob.IsName("walkBack");
            //if (Info.IsName("walkBack"))
            //{
            //    //Log.Info("0000000000 " + Info_Hob.normalizedTime);
            //    Is_Invincible = true;
            //}
            //else if (!Info.IsName("walkBack"))
            //{
            //    //Log.Info("1111111111 " + Info_Hob.normalizedTime);
            //    Is_Invincible = false;
            //}
            //if (PP_Time > 0.5f)
            //{
            //    CounterattackNum = 0;
            //    PP_Time = 0;
            //}
            //Debug.Log("霸体 " + Stoic);
            //Debug.Log("霸体持续时间 " + Stoic_Time);
            //if (Stoic)
            //{
            //    PP_Time += Time.deltaTime;
            //    if (PP_Time > Stoic_Time)
            //    {
            //        Stoic = false;
            //        PP_Time = 0;
            //    }
            //}
        }

        public bool IsEffectAttack = false;                     // 是否在攻击用于判断显示什么特效
        protected override void AddFsmState()
        {
            stateList.Add(HobgoblinIdleState.Create());
            stateList.Add(HobgoblinMotionState.Create());
            stateList.Add(HobgoblinAttackState.Create());
            stateList.Add(EnemyDeadState.Create());
            stateList.Add(HobgoblinHurtState.Create());
            stateList.Add(EnemyRotateState.Create());
            stateList.Add(HobgoblinPatrolState.Create());
            stateList.Add(HobgoblinWindState.Create());
            stateList.Add(HobgoblinPushState.Create());
            stateList.Add(EnemyKnockedDownState.Create());
            stateList.Add(EnemyKnockedFlyState.Create());
            stateList.Add(EnemyKnockedBackState.Create());
            //stateList.Add(EnemyShoutState.Create());
            stateList.Add(HobgoblinFightState.Create());
            stateList.Add(EnemyOutOfTheFight.Create());
        }
        
        public override Type ChangeStateEnemy(EnemyStateType stateType)
        {
            switch(stateType)
            {
                case EnemyStateType.Idle:
                    return typeof(HobgoblinIdleState);
                case EnemyStateType.Motion:
                    return typeof(HobgoblinMotionState);
                case EnemyStateType.Attack:
                    return typeof(HobgoblinAttackState);
                case EnemyStateType.Push:
                    return typeof(HobgoblinPushState);
                case EnemyStateType.Dead:
                    return typeof(EnemyDeadState);
                case EnemyStateType.Wind:
                    return typeof(HobgoblinWindState);
                case EnemyStateType.Hurt:
                    return typeof(HobgoblinHurtState);
                case EnemyStateType.Rotate:
                    return typeof(EnemyRotateState);
                case EnemyStateType.Patrol:
                    return typeof(HobgoblinPatrolState);
                case EnemyStateType.KnockedDown:
                    return typeof(EnemyKnockedDownState);
                case EnemyStateType.KnockedFly:
                    return typeof(EnemyKnockedFlyState);
                case EnemyStateType.KnockedBack:
                    return typeof(EnemyKnockedBackState);
                //case EnemyStateType.Shout:
                //    return typeof(EnemyShoutState);
                case EnemyStateType.Fight:
                    return typeof(HobgoblinFightState);
                case EnemyStateType.OutOfFight:
                    return typeof(EnemyOutOfTheFight);
                default:
                    return typeof(HobgoblinIdleState);
            }
        }
        
        protected override void StartFsm()
        {
            fsm.Start<HobgoblinIdleState>();
        }

        public override void ApplyDamage(Entity attacker, int damageHP,Vector3 point)
        {

            if (Is_Invincible)
            {
                return;
            }

            base.ApplyDamage(attacker, damageHP, point);
            //ApplyDamage会连续触发 若后续修改此代码需调整
            //是否受伤
            if (m_PrevAttackTimeHP != enemyData.HP)
            {
                IsHurt = true;
                if (IsWeak)
                {
                    GameEntry.Sound.PlaySound(enemyData.ByAttackSoundId);
                    GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.BloodEffectId)
                    {
                        Position = point
                    },
                        typeof(EnemyHurtEffectLogic)) ;
                }
                else
                {
                    if (Stoic_Enemy)
                    {
                        //Log.Info("霸体受击");
                        GameEntry.Sound.PlaySound(enemyData.StoicHurtSoundId);
                        GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.StoicHurtEffectId)
                        {
                            Position = point
                        },
                            typeof(EnemyHurtEffectLogic));
                    }
                    else
                    {
                        GameEntry.Sound.PlaySound(enemyData.ByAttackSoundId);
                        GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 60068)
                        {
                            Position = point
                        },
                            typeof(EnemyHurtEffectLogic));
                    }
                }
                m_PrevAttackTimeHP = enemyData.HP;
            }
        }


        //测试技能特效
        //动画事件维护IsUseSkill

        public void SkillEffect()
        {
            IsUseSkill = true;
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70006),
                typeof(EnemySkillEffectLogic));
        }
        public void Zadi()
        {
            IsUseSkill = true;
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70120),
                typeof(Old_HobgoblinZadiLogic));
        }
        public void Roar()
        {
            IsUseSkill = true;
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70119) 
            { Position = zhanhou_Transform.position, KeepTime = 0.9f },
                typeof(Old_HobgoblinRoarLogic));
        }
        public void PushEffect()
        {
            IsUseSkill = true;
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70121) 
            { Position = chongci_Transform.position, KeepTime = 5f },
                typeof(Old_HobgoblinPushEffectLogic));
        }
        
        public void FeipuEffect()
        {
            IsUseSkill = true;
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70122)
            { Position = feipu_Transform.position, KeepTime = 5f },
                typeof(Old_HobgoblinFeipuLogic));
        }
        public void HuohuaEffect()
        {
            IsUseSkill = true;
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70124),
                typeof(Old_HobgoblinHuohuaLogic));
        }
        public virtual void PushEnd()
        {
            IsPush = false;
        }
        public virtual void Delayed()
        {
            m_Animator.speed = 0.5f;
        }
        public virtual void DelayedT()
        {
            m_Animator.speed = 0.25f;
        }
        public virtual void DelayedP()
        {
            m_Animator.speed = 0.1f;
        }


        public virtual void DelayedEnd()
        {
            m_Animator.speed = 1f;
        }

        public virtual void Counterattack()
        {
            //Debug.Log("霸体开");
            Stoic_Enemy = true;
        }

        public virtual void CounterattackEnd()
        {
            //Debug.Log("霸体关");
            Stoic_Enemy = false;
        }

        public void Invincible()
        {
            Is_Invincible = true;
        }
        public void InvincibleEnd()
        {
            Is_Invincible = false;
        }


    }
}

