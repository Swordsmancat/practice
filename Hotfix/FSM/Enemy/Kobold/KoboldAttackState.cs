using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

namespace Farm.Hotfix
{
    public class KoboldAttackState : EnemyAttackState
    {
        private float m_PrevTimeHP;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_PrevTimeHP != owner.enemyData.HP)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Hurt));
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.EnemyAttackEnd();
        }

        public static new KoboldAttackState Create()
        {
            KoboldAttackState state = ReferencePool.Acquire<KoboldAttackState>();
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

