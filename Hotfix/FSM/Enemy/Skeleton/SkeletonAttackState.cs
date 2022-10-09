using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

namespace Farm.Hotfix
{
    public class SkeletonAttackState : EnemyAttackState
    {
        private float m_PrevTimeHP;

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            if(owner.IsAnimPlayed)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Idle));
            }

            if (owner.enemyData.HP <= 0)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Dead));
            }

            if(m_PrevTimeHP != owner.enemyData.HP)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Hurt));
            }

        }

        public new static SkeletonAttackState Create()
        {
            SkeletonAttackState state = ReferencePool.Acquire<SkeletonAttackState>();
            return state;
        }

        protected override void EnemyAttackStateStart(EnemyLogic owner)
        {
            base.EnemyAttackStateStart(owner);
            m_PrevTimeHP = owner.enemyData.HP;
            NormalAttack();
        }
    }
}

