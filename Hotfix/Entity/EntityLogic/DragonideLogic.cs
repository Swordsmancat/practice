using System.Collections;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using UnityEngine;
using System;
using GameFramework;

namespace Farm.Hotfix
{
    public class DragonideLogic : EnemyLogic
    {
        private readonly static string s_startPoint = "StartPoint";//����������Ч��λ��
        private Vector3 EffectsPoint_R; //��Ч��λ����
        private GameObject Weapon_StartPoint;
        public BuffType m_BuffType;


        protected override void OnDead(Entity attacker, Vector3 point)
        {
            Debug.Log("��������");
            base.OnDead(attacker,point);
            EnemyAttackEnd();
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.DeadEffectId)
            {
                Position = point,
            },
                typeof(EnemyHurtEffectLogic));
        }

        protected override void AddFsmState()
        {
            Debug.Log("����¼�");
            //stateList.Add(EnemyIdleState.Create());
            stateList.Add(DragonideIdleState.Create());
            //stateList.Add(EnemyMotionState.Create());
            stateList.Add(DragonideMotionState.Create());
            stateList.Add(DragonideAttackState.Create());
            stateList.Add(DragonideDeadState.Create());
            //stateList.Add(EnemyDeadState.Create());
            //stateList.Add(OrcHurtState.Create());
            stateList.Add(DragonideHurtState.Create());
            stateList.Add(EnemyRotateState.Create());
            //stateList.Add(EnemyShoutState.Create());
            stateList.Add(DragonideShoutState.Create());
            stateList.Add(DragonideFightState.Create());
            //stateList.Add(EnemyFightState.Create());
            stateList.Add(DragonideOutOfTheFight.Create());
            stateList.Add(DragonideParryState.Create());
            stateList.Add(EnemyKnockedDownState.Create());
            //stateList.Add(EnemyOutOfTheFight.Create());

        }

        public override Type ChangeStateEnemy(EnemyStateType stateType)
        {
            switch (stateType)
            {
                case EnemyStateType.Idle:
                //Debug.Log("վ��");
                return typeof(DragonideIdleState);
                //return typeof(EnemyIdleState);
                case EnemyStateType.Motion:
                    //Debug.Log("�ƶ�");
                    return typeof(DragonideMotionState);
                    //return typeof(EnemyMotionState);
                case EnemyStateType.Attack:
                    //Debug.Log("����");
                    return typeof(DragonideAttackState);
                case EnemyStateType.Dead:
                    return typeof(DragonideDeadState);
                    //return typeof(EnemyDeadState);
                case EnemyStateType.Hurt:
                    //return typeof(OrcHurtState);
                    return typeof(DragonideHurtState);
                case EnemyStateType.Rotate:
                    return typeof(EnemyRotateState);
                case EnemyStateType.Fight:
                    return typeof(DragonideFightState);
                    //return typeof(EnemyFightState);
                case EnemyStateType.OutOfFight:
                    return typeof(DragonideOutOfTheFight);
                //return typeof(EnemyOutOfTheFight);
                case EnemyStateType.Shout:
                    //return typeof(EnemyShoutState);
                    return typeof(DragonideShoutState);
                case EnemyStateType.Parry:
                    return typeof(DragonideParryState);
                case EnemyStateType.KnockedDown:
                    return typeof(EnemyKnockedDownState);
                default:
                    //return typeof(EnemyIdleState);
                    return typeof(DragonideIdleState);
            }
        }

        protected override void StartFsm()
        {
            //fsm.Start<EnemyIdleState>();
            fsm.Start<DragonideIdleState>();
        }
        /// <summary>
        /// ������������
        /// </summary>

        public override void ApplyDamage(Entity attacker, int damageHP, Vector3 weapon)
        {

            //if (enemyData.HP <= 0 || enemyData.HP <= damageHP)
            //{
            //    base.ApplyDamage(attacker, damageHP);
            //    return;
            //}

            base.ApplyDamage(attacker, damageHP,weapon);

            //ApplyDamage���������� �������޸Ĵ˴��������
            //�Ƿ�����
            if (m_PrevAttackTimeHP != enemyData.HP)
            {
                IsHurt = true;
                if (!Stoic)
                {
                    GameEntry.Sound.PlaySound(enemyData.ByAttackSoundId);
                    GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.BloodEffectId)
                    {
                        Position = weapon
                    },
                        typeof(EnemyHurtEffectLogic));
                }
                else
                {
                    //Log.Info("�����ܻ�");
                    GameEntry.Sound.PlaySound(enemyData.StoicHurtSoundId);
                    GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.StoicHurtEffectId)
                    {
                        Position=weapon
                    },
                        typeof(EnemyHurtEffectLogic));
                }
                m_PrevAttackTimeHP = enemyData.HP;
            }
        }

        
        public override void ApplyBuffEvent(BuffType buffType)
        {
            base.ApplyBuffEvent(buffType);
            m_BuffType = buffType;
      
        }
    }
}
