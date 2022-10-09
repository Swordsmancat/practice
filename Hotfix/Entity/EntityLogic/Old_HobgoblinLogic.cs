using System;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using System.Collections.Generic;
using System.Collections;


namespace Farm.Hotfix
{
    public class Old_HobgoblinLogic : EnemyLogic
    {
        private Transform zhanhou_Transform;
        private Transform chongci_Transform;
        private Transform feipu_Transform;
        public bool IsPush = true;



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

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            m_Animator.SetBool(m_IsCanAttack, IsCanAttack);
        }

        public bool IsEffectAttack = false;                     // 是否在攻击用于判断显示什么特效
        protected override void AddFsmState()
        {
            stateList.Add(Old_HobgoblinIdleState.Create());
            stateList.Add(Old_HobgoblinMotionState.Create());
            stateList.Add(Old_HobgoblinAttackState.Create());
            stateList.Add(EnemyDeadState.Create());
            stateList.Add(Old_HobgoblinHurtState.Create());
            stateList.Add(EnemyRotateState.Create());
            stateList.Add(Old_HobgoblinPatrolState.Create());
            stateList.Add(Old_HobgoblinWindState.Create());
            stateList.Add(Old_HobgoblinPushState.Create());
            stateList.Add(EnemyKnockedDownState.Create());
            stateList.Add(EnemyKnockedFlyState.Create());
            stateList.Add(EnemyKnockedBackState.Create());
            //stateList.Add(EnemyShoutState.Create());
            stateList.Add(Old_HobgoblinFightState.Create());
            stateList.Add(EnemyOutOfTheFight.Create());
        }

        public override Type ChangeStateEnemy(EnemyStateType stateType)
        {
            switch (stateType)
            {
                case EnemyStateType.Idle:
                    return typeof(Old_HobgoblinIdleState);
                case EnemyStateType.Motion:
                    return typeof(Old_HobgoblinMotionState);
                case EnemyStateType.Attack:
                    return typeof(Old_HobgoblinAttackState);
                case EnemyStateType.Push:
                    return typeof(Old_HobgoblinPushState);
                case EnemyStateType.Dead:
                    return typeof(EnemyDeadState);
                case EnemyStateType.Wind:
                    return typeof(Old_HobgoblinWindState);
                case EnemyStateType.Hurt:
                    return typeof(Old_HobgoblinHurtState);
                case EnemyStateType.Rotate:
                    return typeof(EnemyRotateState);
                case EnemyStateType.Patrol:
                    return typeof(Old_HobgoblinPatrolState);
                case EnemyStateType.KnockedDown:
                    return typeof(EnemyKnockedDownState);
                case EnemyStateType.KnockedFly:
                    return typeof(EnemyKnockedFlyState);
                case EnemyStateType.KnockedBack:
                    return typeof(EnemyKnockedBackState);
                //case EnemyStateType.Shout:
                //    return typeof(EnemyShoutState);
                case EnemyStateType.Fight:
                    return typeof(Old_HobgoblinFightState);
                case EnemyStateType.OutOfFight:
                    return typeof(EnemyOutOfTheFight);
                default:
                    return typeof(Old_HobgoblinIdleState);
            }
        }

        protected override void StartFsm()
        {
            fsm.Start<HobgoblinIdleState>();
        }

        public override void ApplyDamage(Entity attacker, int damageHP, Vector3 point)
        {

            //if (enemyData.HP <= 0 || enemyData.HP <= damageHP)
            //{
            //    base.ApplyDamage(attacker, damageHP);
            //    return;
            //}

            base.ApplyDamage(attacker, damageHP, point);

            //ApplyDamage会连续触发 若后续修改此代码需调整
            //是否受伤
            if (m_PrevAttackTimeHP != enemyData.HP)
            {
                IsHurt = true;
                GameEntry.Sound.PlaySound(enemyData.ByAttackSoundId);
                GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.BloodEffectId)
                {
                    Position = point
                },
                    typeof(EnemyHurtEffectLogic));
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

    }
}
