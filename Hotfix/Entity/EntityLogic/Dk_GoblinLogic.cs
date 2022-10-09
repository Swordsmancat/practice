using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
     public class Dk_GoblinLogic: EnemyLogic
     {
        //≈……˙¿‡∂¿”–◊÷∂Œ
        public float BlockTime = 0;
        public float ShoutTime = 0;
        public bool IsRoll = false;
        #region Base

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnDead(Entity attacker, Vector3 point)
        {
            base.OnDead(attacker,point);
            EnemyAttackEnd();
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.DeadEffectId)
            {
                Position = point,
            },
                typeof(EnemyHurtEffectLogic));
        }

        public override void ApplyDamage(Entity attacker, int damageHP, Vector3 weapon)
        {

            if (enemyData.HP <= 0)
            {
                return;
            }

            if (IsRoll)
            {
                return;
            }

            //if (Invulnerable)
            //{
            //    return;
            //}

            if (IsBlock)
            {
                IsHurt = true;
              //  Invulnerable = true;
                GameEntry.Event.Fire(this, ApplyDamageEventArgs.Create(this));
                return;
            }

            if (enemyData.HP <= 0 || enemyData.HP <= damageHP)
            {
                base.ApplyDamage(attacker, damageHP,weapon);
                return;
            }

            if (IsParry)
            {
               // Invulnerable = true;
                EnemyAttackEnd();
                return;
            }

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

        #endregion //End Base

        #region State
        public override Type ChangeStateEnemy(EnemyStateType stateType)
        {
            switch (stateType)
            {
                case EnemyStateType.Idle:
                    return typeof(GoblinIdleState);
                case EnemyStateType.Shout:
                    return typeof(DK_GoblinShoutState);
                case EnemyStateType.Motion:
                    return typeof(DK_GoblinMotionState);
                case EnemyStateType.Dead:
                    return typeof(EnemyDeadState);
                case EnemyStateType.Attack:
                    return typeof(DK_GoblinAttackState);
                case EnemyStateType.Hurt:
                    return typeof(DK_GoblinHurtState);
                case EnemyStateType.Fight:
                    return typeof(DK_GoblinFightState);
                case EnemyStateType.Rotate:
                    return typeof(EnemyRotateState);
                case EnemyStateType.OutOfFight:
                    return typeof(EnemyOutOfTheFight);
                case EnemyStateType.KnockedDown:
                    return typeof(EnemyKnockedDownState);
                default:
                    return typeof(GoblinIdleState);
            }
        }

        protected override void AddFsmState()
        {
            stateList.Add(GoblinIdleState.Create());
            stateList.Add(DK_GoblinMotionState.Create());
            stateList.Add(EnemyRotateState.Create());
            stateList.Add(EnemyDeadState.Create());
            stateList.Add(DK_GoblinHurtState.Create());
            stateList.Add(EnemyOutOfTheFight.Create());
            stateList.Add(DK_GoblinShoutState.Create());
            stateList.Add(DK_GoblinAttackState.Create());
            stateList.Add(DK_GoblinFightState.Create());
            stateList.Add(EnemyKnockedDownState.Create());
        }

        protected override void StartFsm()
        {
            fsm.Start<GoblinIdleState>();
        }
        #endregion

        #region AnimationEvent

        public override void AnimationEnd()
        {
            base.AnimationEnd();
            IsRoll = false;
        }

        public override void AnimationStart()
        {
            base.AnimationStart();
            IsRoll = true;
        }

        #endregion
    }
}
