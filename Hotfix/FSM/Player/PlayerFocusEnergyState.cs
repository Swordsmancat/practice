using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerFocusEnergyState : PlayerBaseActionState
    {
        private PlayerLogic owner;
        private float invincibleTime = 5f;
        private float invincibleFrame;

        private static readonly int IsFocusEngy = Animator.StringToHash("IsFocusEngy");             //是否蓄力
        private static readonly int FocusEnergyTrigger = Animator.StringToHash("FocusEnergyTrigger");
        private static readonly int LockMoveBlendX = Animator.StringToHash("LockMoveBlendX");
        private static readonly int ThumpNum = Animator.StringToHash("ThumpNum");
        private static readonly int ClickAttackBtnDuration = Animator.StringToHash("ClickAttackBtnDuration");

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            //owner.m_Animator.SetInteger(ThumpNum, Utility.Random.GetRandom(1, 3));
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            owner.m_Animator.ResetTrigger(FocusEnergyTrigger);
            owner.m_Animator.SetBool(IsFocusEngy, owner.isFocusEngy);
            owner.m_Animator.SetFloat(LockMoveBlendX, Mathf.Max(Mathf.Abs(owner.MoveX), Mathf.Abs(owner.MoveY)), 0.1f, Time.deltaTime);
            
            //蓄力结束判断 有没有达到技能施放时间
            if (owner.isFocusEngy == false)
            {
                if (owner.m_Animator.GetFloat(ClickAttackBtnDuration) > 3f)
                {
                    owner.m_Animator.SetInteger(ThumpNum, 11);
                    ChangeState<PlayerSkillState>(procedureOwner);
                }
                else
                {
                    ChangeState<PlayerIdleState>(procedureOwner);
                }

            }
            else
            {
                owner.m_Animator.SetTrigger(FocusEnergyTrigger);
            }
            invincibleFrame += Time.deltaTime;
            //if (invincibleFrame >= invincibleTime)
            //{
            //    ChangeState<PlayerIdleState>(procedureOwner);
            //}
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            invincibleFrame = 0;
            owner.m_Animator.SetBool(IsFocusEngy, false);
            owner.m_IsStartCount = false;
            owner.isFocusEngy = false;
        }

        public static PlayerFocusEnergyState Create()
        {
            PlayerFocusEnergyState state = ReferencePool.Acquire<PlayerFocusEnergyState>();
            return state;
        }

    }
}
