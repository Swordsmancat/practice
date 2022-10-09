using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerKnockedBackState : PlayerBaseState
    {
        private PlayerLogic owner;
        private float knockedDownTime = 1.2f;
        private float knockedDownFrame;

        private readonly string Layer = "Base Layer";
        private static readonly int KnockedBack = Animator.StringToHash("KnockedBack");
        private static readonly int IsKnockedDown = Animator.StringToHash("IsKnockedDown");
        private static readonly int IsBehindAtked = Animator.StringToHash("IsBehindAtked");

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.isKnockedDown = true;

            owner.Buff.BuffTypeEnum = BuffType.None;
            owner.m_Animator.SetTrigger(KnockedBack);
            owner.m_Animator.SetBool(IsKnockedDown, true);
            owner.m_moveBehaviour.IsKnockLock(true);//防止角色倒地后 按方向键会转动
            owner.HideTrail();//角色倒地 关闭拖尾和攻击检测 防止怪物碰到角色武器会一直受伤
            owner.AttackEnd();//同上
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex(Layer)).normalizedTime >= 0.9f)
            {
                ChangeState<PlayerGetUpState>(procedureOwner);
            }

            //knockedDownFrame += Time.deltaTime;
            // if (knockedDownFrame >= knockedDownTime)
            // {
                //ChangeState<PlayerGetUpState>(procedureOwner);
            // }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.isKnockedDown = false;
            owner.m_Animator.SetBool(IsKnockedDown, false);

            knockedDownFrame = 0;
        }

        public static PlayerKnockedBackState Create()
        {
            PlayerKnockedBackState state = ReferencePool.Acquire<PlayerKnockedBackState>();
            return state;
        }

    }
}
