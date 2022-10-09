using UnityEngine;
using System;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using System.Collections.Generic;
using System.Collections;


namespace Farm.Hotfix
{
    public class UndeadLogic : EnemyLogic
    {
        public float BlockTime = 0;
        public float ShoutTime = 0;
        protected override void OnDead(Entity attacker, Vector3 point)
        {
            base.OnDead(attacker,point);
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.DeadEffectId)
            {
                Position = point,
            },
                typeof(EnemyHurtEffectLogic));
        }

        // Start is called before the first frame update
        protected override void AddFsmState()
        {
            //Ìí¼Ó×´Ì¬
            stateList.Add(UndeadIdleState.Create());
            stateList.Add(UndeadMotionState.Create());
            stateList.Add(UndeadAttackState.Create());
            stateList.Add(UndeadHurtState.Create());
            stateList.Add(EnemyDeadState.Create());
            stateList.Add(EnemyRotateState.Create());
            stateList.Add(EnemyFightState.Create());
            stateList.Add(EnemyOutOfTheFight.Create());
            stateList.Add(UndeadBlockState.Create());
            //stateList.Add(UndeadShoutState.Create());
        }
        public override Type ChangeStateEnemy(EnemyStateType stateType)
        {
            //¸Ä±ä×´Ì¬
            base.ChangeStateEnemy(stateType);
            switch (stateType)
            {
                case EnemyStateType.Idle:
                    return typeof(UndeadIdleState);
                case EnemyStateType.Motion:
                    return typeof(UndeadMotionState);
                case EnemyStateType.Attack:
                    return typeof(UndeadAttackState);
                case EnemyStateType.Hurt:
                    return typeof(UndeadHurtState);
                case EnemyStateType.Block:
                    return typeof(UndeadBlockState);
                case EnemyStateType.Dead:
                    return typeof(EnemyDeadState);
                //case EnemyStateType.Shout:
                //    return typeof(UndeadShoutState);
                case EnemyStateType.Rotate:
                    return typeof(EnemyRotateState);
                case EnemyStateType.Fight:
                    return typeof(EnemyFightState);
                case EnemyStateType.OutOfFight:
                    return typeof(EnemyOutOfTheFight);
                default:
                    return typeof(UndeadIdleState);
            }

        }
        protected override void StartFsm()
        {
            fsm.Start<UndeadIdleState>();//ÉèÖÃ³õÊ¼×´Ì¬
        }

        public override void ApplyDamage(Entity attacker, int damageHP, Vector3 weapon)
        {
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
    }
}
