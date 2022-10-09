using UnityEngine;
using System;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class GoblinIdleState : EnemyIdleState
    {
        private readonly static int m_NormalBlend = Animator.StringToHash("NormalBlend");
        private readonly float AlertDistance = 90f;

        public static GoblinIdleState Create()
        {
            GoblinIdleState state = ReferencePool.Acquire<GoblinIdleState>();
            return state;
        }

        protected override void EnemyIdleStateStart(EnemyLogic owner)
        {
            owner.m_Animator.SetFloat(m_NormalBlend, 0f);
            //if (owner.CurrentTargetDisdance <= AlertDistance && owner.CurrentTargetDisdance > 90)
            //{
            //    owner.m_Animator.SetFloat(m_NormalBlend, 1f);
            //}
            //else
            //{
            //    owner.m_Animator.SetFloat(m_NormalBlend, 0f);
            //}
        }
        protected override void LockPlayerDo(ProcedureOwner procedureOwner)
        {
            owner.LockEntity(owner.find_Player);
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Shout));
        }
    }
}

