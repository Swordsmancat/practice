using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using System.Collections;

namespace Farm.Hotfix
{
    public class PlayerStepState : PlayerBaseActionState
    {
        private PlayerLogic owner;
        private float invincibleTime = 0.5f;
        private float invincibleFrame;

        private static readonly int StepTrigger = Animator.StringToHash("StepTrigger");
        private static readonly int Step = Animator.StringToHash("Step");
        private static readonly int LockMoveBlendX = Animator.StringToHash("LockMoveBlendX");
        private static readonly int LockMoveBlendY = Animator.StringToHash("LockMoveBlendY");
        private static readonly int MoveBlend = Animator.StringToHash("MoveBlend");
        private static readonly int IsLockEnemy = Animator.StringToHash("IsLockEnemy");
        private static readonly int SolidAtkNum = Animator.StringToHash("SolidAtkNum");

        private Vector2 currentLockDir;


        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            if (owner.m_LockEnemy)
            {
                owner.m_Animator.SetBool(IsLockEnemy, true);
            }
            else
            {
                owner.m_Animator.SetBool(IsLockEnemy, false);
            }
            owner.m_Animator.SetTrigger(StepTrigger);
            owner.m_Animator.SetBool(Step,true);
            owner.m_Animator.SetInteger(SolidAtkNum, Utility.Random.GetRandom(1, 3));//目前只有双剑有滑步 滑步后的特殊攻击随机取一种
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            invincibleFrame += Time.deltaTime;

            //动画自身位移太短 滑步增加点位移 
            owner.StepMove(owner.gameObject.transform, owner.m_Animator.GetFloat(LockMoveBlendX),
                owner.m_Animator.GetFloat(LockMoveBlendY),owner.m_Animator.GetFloat(MoveBlend), 3f);

            owner.m_Animator.ResetTrigger(StepTrigger);
            if (invincibleFrame >= invincibleTime)
            {
                owner.isStep = false;
                ChangeState<PlayerMotionState>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            invincibleFrame = 0;
            owner.m_Animator.SetBool(Step, false);
            owner.StartCoro(0.5f);
        }

        public static PlayerStepState Create()
        {
            PlayerStepState state = ReferencePool.Acquire<PlayerStepState>();
            return state;
        }

    }
}
