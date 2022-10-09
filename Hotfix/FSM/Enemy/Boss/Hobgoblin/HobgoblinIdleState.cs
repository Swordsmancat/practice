using UnityEngine;
using System;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class HobgoblinIdleState : EnemyIdleState
    {
        float idle_time = 0;
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
            idle_time += elapseSeconds;

            //if (idle_time >= 10)
            //{
            //    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Patrol));
            //}
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            idle_time = 0;
            base.OnLeave(fsm, isShutdown);
        }

        public static new HobgoblinIdleState Create()
        {
            HobgoblinIdleState state = ReferencePool.Acquire<HobgoblinIdleState>();
            return state;
        }
    }
}
