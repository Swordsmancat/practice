using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    /// <summary>
    /// 敌人闲置状态
    /// </summary>
   public class EnemyIdleState : EnemySeekState
    {
        protected EnemyLogic owner;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.UnLockEntity();
            EnemyIdleStateStart(owner);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

        public static EnemyIdleState Create()
        {
            EnemyIdleState state = ReferencePool.Acquire<EnemyIdleState>();
            return state;
        }

        /// <summary>
        /// 停止移动
        /// </summary>
        protected virtual void EnemyIdleStateStart(EnemyLogic owner)
        {
            owner.m_Animator.SetFloat("MoveBlend", 0f);
        }

    }
}
