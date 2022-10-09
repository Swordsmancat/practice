using UnityEngine;
using System;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using System.Collections.Generic;
using System.Collections;

namespace Farm.Hotfix
{
    public class OrcLogic : EnemyLogic
    {
        protected override void OnDead(Entity attacker, Vector3 point)
        {
            base.OnDead(attacker,point);
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(),enemyData.DeadEffectId)
            {
                Position = point,
            },
                typeof(EnemyHurtEffectLogic));
        }

        protected override void AddFsmState()
        {
            stateList.Add(OrcIdleState.Create());
            stateList.Add(OrcMotionState.Create());
            stateList.Add(OrcAttackState.Create());
            stateList.Add(EnemyDeadState.Create());
            stateList.Add(OrcHurtState.Create());
            stateList.Add(EnemyRotateState.Create());
            stateList.Add(EnemyShoutState.Create());
            stateList.Add(OrcFightState.Create());
            stateList.Add(EnemyOutOfTheFight.Create());
            stateList.Add(EnemyKnockedDownState.Create());
        }

        public override Type ChangeStateEnemy(EnemyStateType stateType)
        {
            switch(stateType)
            {
                case EnemyStateType.Idle:
                    return typeof(OrcIdleState);
                case EnemyStateType.Motion:
                    return typeof(OrcMotionState);
                case EnemyStateType.Attack:
                    return typeof(OrcAttackState);
                case EnemyStateType.Dead:
                    return typeof(EnemyDeadState);
                case EnemyStateType.Hurt:
                    return typeof(OrcHurtState);
                case EnemyStateType.Rotate:
                    return typeof(EnemyRotateState);
                case EnemyStateType.Shout:
                    return typeof(EnemyShoutState);
                case EnemyStateType.Fight:
                    return typeof(OrcFightState);
                case EnemyStateType.OutOfFight:
                    return typeof(EnemyOutOfTheFight);
                case EnemyStateType.KnockedDown:
                    return typeof(EnemyKnockedDownState);
                default:
                    return typeof(OrcIdleState);
            }
        }

        protected override void StartFsm()
        {
            fsm.Start<OrcIdleState>();
        }

        public override void ApplyDamage(Entity attacker, int damageHP, Vector3 weapon)
        {

            if (enemyData.HP <= 0 || enemyData.HP <= damageHP)
            {
                base.ApplyDamage(attacker, damageHP,weapon);
                return;
            }

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


        //测试技能特效
        //动画事件维护IsUseSkill
        public void SkillEffect()
        {
            IsUseSkill = true;
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70006),
                typeof(EnemySkillEffectLogic));
        }

    }
}

