using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    public class HarpyLogic : EnemyLogic
    {
        //哈布鹰私有字段
        public bool IsCanUp = false;        //通过射线检测是否能上升
        public float CurrentHight = 0;      //当前高度
        public float WaitTimer = 0;         //等待时间触发
        public readonly int WaitTime = 5;   //等待时间

        //地面层(过滤)
        readonly private int LayerMask = 1 << 6;
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
            
            //发出一条垂直向下的射线
            //最大检查距离为10,设为最大飞行高度
            Ray downRay = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(downRay, out hit, 10, LayerMask))
            {
                Debug.DrawLine(downRay.origin, hit.point);
                IsCanUp = true;
            }
            else
            {
                IsCanUp = false;
            }

            CurrentHight = downRay.origin.y - hit.point.y;
        }

        protected override void OnDead(Entity attacker, Vector3 point)
        {
            base.OnDead(attacker,point);
        }

        public override void ApplyDamage(Entity attacker, int damageHP, Vector3 weapon)
        {
            if (enemyData.HP <= 0 || enemyData.HP <= damageHP)
            {
                base.ApplyDamage(attacker, damageHP, weapon);
                return;
            }

            base.ApplyDamage(attacker, damageHP, weapon);

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

        #region AboutSfm

        public override Type ChangeStateEnemy(EnemyStateType stateType)
        {
            switch (stateType)
            {
                case EnemyStateType.Idle:
                    return typeof(HarpyIdleState);
                case EnemyStateType.Motion:
                    return typeof(HarpyMotionState);
                case EnemyStateType.OutOfFight:
                    return typeof(EnemyOutOfTheFight);
                case EnemyStateType.Fight:
                    return typeof(EnemyFightState);
                case EnemyStateType.Hurt:
                    return typeof(HarpyHurtState);
                case EnemyStateType.Attack:
                    return typeof(HarpyAttackState);
                case EnemyStateType.Dead:
                    return typeof(EnemyDeadState);
                case EnemyStateType.Rotate:
                    return typeof(HarpyRotateState);
                default:
                    return typeof(HarpyIdleState);
            }

        }

        protected override void AddFsmState()
        {
            stateList.Add(HarpyIdleState.Create());
            stateList.Add(HarpyMotionState.Create());
            stateList.Add(EnemyOutOfTheFight.Create());
            stateList.Add(EnemyFightState.Create());
            stateList.Add(HarpyAttackState.Create());
            stateList.Add(EnemyDeadState.Create());
            stateList.Add(HarpyRotateState.Create());
            stateList.Add(HarpyHurtState.Create());
            stateList.Add(HarpyFlyState.Create());
        }

        protected override void StartFsm()
        {
            fsm.Start<HarpyIdleState>();
        }


        #endregion
    }

}

