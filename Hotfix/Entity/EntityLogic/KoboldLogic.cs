using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class KoboldLogic : EnemyLogic
    {
        #region Base

        protected override void OnDead(Entity attacker, Vector3 point)
        {
            base.OnDead(attacker,point);
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.DeadEffectId)
            {
                Position = point,
            },
                typeof(EnemyHurtEffectLogic));
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        public override void ApplyDamage(Entity attacker, int damageHP, Vector3 weapon)
        {
            //if (enemyData.HP <= 0 || enemyData.HP <= damageHP)
            //{
            //    base.ApplyDamage(attacker, damageHP);
            //    return;
            //}

            base.ApplyDamage(attacker, damageHP,weapon);

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

        #endregion

        #region AboutFsm

        protected override void AddFsmState()
        {
            stateList.Add(EnemyIdleState.Create());
            stateList.Add(EnemyMotionState.Create());
            stateList.Add(KoboldAttackState.Create());
            stateList.Add(EnemyHurtState.Create());
            stateList.Add(EnemyDeadState.Create());
            stateList.Add(EnemyFightState.Create());
            stateList.Add(EnemyRotateState.Create());
            stateList.Add(EnemyOutOfTheFight.Create());
        }

        protected override void StartFsm()
        {
            fsm.Start<EnemyIdleState>();          
        }

        public override Type ChangeStateEnemy(EnemyStateType stateType)
        {
            switch (stateType)
            {
                case EnemyStateType.Idle:
                    return typeof(EnemyIdleState);
                case EnemyStateType.Motion:
                    return typeof(EnemyMotionState);
                case EnemyStateType.Attack:
                    return typeof(KoboldAttackState);
                case EnemyStateType.Dead:
                    return typeof(EnemyDeadState);
                case EnemyStateType.Hurt:
                    return typeof(EnemyHurtState);
                case EnemyStateType.Fight:
                    return typeof(EnemyFightState);
                case EnemyStateType.Rotate:
                    return typeof(EnemyRotateState);
                case EnemyStateType.OutOfFight:
                    return typeof(EnemyOutOfTheFight);
                default:
                    return typeof(EnemyIdleState);
            }
        }

        #endregion
    }
}

