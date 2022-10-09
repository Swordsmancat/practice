using UnityEngine;
using System;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;


namespace Farm.Hotfix
{
    public class UndeadIdleState : EnemyIdleState
    {
        // Start is called before the first frame update
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
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

        public static new UndeadIdleState Create()
        {
            UndeadIdleState state = ReferencePool.Acquire<UndeadIdleState>();
            return state;
        }

        protected override void LockPlayerDo(ProcedureOwner procedureOwner)
        {
            owner.LockEntity(owner.find_Player);
            //base.LockPlayerDo(procedureOwner);
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
        }
    }

}