using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerGetUpState : PlayerBaseActionState
    {
        private PlayerLogic owner;
        private float GetUpTime = 0.5f;
        private float GetUpFrame;

        private static readonly int GetUp = Animator.StringToHash("GetUp");

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.m_Animator.SetInteger(GetUp, Utility.Random.GetRandom(1, 4));//3种起身动画随机一种
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            GetUpFrame += Time.deltaTime;
            if (GetUpFrame >= GetUpTime)
            {
                //ChangeState<PlayerIdleState>(procedureOwner);
                ChangeState<PlayerMotionState>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            GetUpFrame = 0;
            owner.m_moveBehaviour.IsKnockLock(false);
        }

        public static PlayerGetUpState Create()
        {
            PlayerGetUpState state = ReferencePool.Acquire<PlayerGetUpState>();
            return state;
        }

    }
}
