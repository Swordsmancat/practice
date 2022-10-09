using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerSecondSkillState : PlayerBaseActionState
    {
        private PlayerLogic owner;

        private float invincibleTime = 1.2f;
        private float invincibleFrame = 0f;

        private static readonly int IsSecondSkill = Animator.StringToHash("IsSecondSkill");
        private static readonly int SkillStateTime = Animator.StringToHash("SkillStateTime");



        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.m_Animator.SetFloat(SkillStateTime, 0);
            invincibleFrame = 0;
            owner.isSecondSkill = true;
            owner.m_Animator.SetBool(IsSecondSkill, true) ;
            owner.m_moveBehaviour.IsSkill(true);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            owner.m_Animator.SetFloat(SkillStateTime, Mathf.Repeat(owner.m_Animator.GetCurrentAnimatorStateInfo(3).normalizedTime, 1));
            if (owner.m_Animator.GetFloat(SkillStateTime) >= 0.7f)
            {
                ChangeState<PlayerMotionState>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.isSecondSkill = false;
            owner.m_Animator.SetBool(IsSecondSkill, false);
            owner.m_moveBehaviour.IsSkill(false);
        }

        public static PlayerSecondSkillState Create()
        {
            PlayerSecondSkillState state = ReferencePool.Acquire<PlayerSecondSkillState>();
            return state;
        }

    }
}
