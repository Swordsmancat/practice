using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using System.Collections;
using System;


namespace Farm.Hotfix
{
    public class HarpyMotionState : EnemyMotionState
    {
        private readonly static int MoveBlend = Animator.StringToHash("MoveBlend");
        private readonly static int MaxDistance = 10;
        new private HarpyLogic owner;
        
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner as HarpyLogic;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            owner.WaitTimer += elapseSeconds;
            //目前鸟人起飞方式大于指定距离之后就起飞
            if (owner.WaitTimer >= owner.WaitTime)
            {
                if (owner.CurrentTargetDisdance >= MaxDistance)
                {
                    owner.WaitTimer = 0;
                    ChangeState<HarpyFlyState>(procedureOwner);
                }
            }

        }

        protected override void MovementBlend()
        {
            owner.m_Animator.SetFloat(MoveBlend, 2f);
        }

        protected override void StopMoveBlend()
        {
            base.StopMoveBlend();
        }

        public new static HarpyMotionState Create()
        {
            HarpyMotionState state = ReferencePool.Acquire<HarpyMotionState>();
            return state;
        }
    }
}

