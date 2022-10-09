using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerDodgeState : PlayerBaseActionState
    {
        private PlayerLogic owner;
        private float invincibleTime = 0.8f;
        private float invincibleFrame;

        private static readonly int Dodge = Animator.StringToHash("Dodge");
        private static readonly int IsLockEnemy = Animator.StringToHash("IsLockEnemy");
        private static readonly int LockMoveBlendX = Animator.StringToHash("LockMoveBlendX");
        private static readonly int LockMoveBlendY = Animator.StringToHash("LockMoveBlendY");
        private static readonly int MoveBlend = Animator.StringToHash("MoveBlend");
        private static readonly int m_HashStateTime = Animator.StringToHash("StateTime");


        private static readonly string BaseLayer = "Base Layer";


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
                owner.m_Animator.SetFloat(LockMoveBlendY, owner.MoveY);
                owner.m_Animator.SetFloat(LockMoveBlendX, owner.MoveX);
            }
            else
            {
                owner.m_Animator.SetBool(IsLockEnemy, false);
            }

            owner.m_Animator.SetTrigger(Dodge);
            Log.Info("进入翻滚");
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            //invincibleFrame += Time.deltaTime;

            // 锁定后左右旋转增加向前的位移
            if (owner.m_Animator.GetFloat(LockMoveBlendX) != 0)
            {
                owner.PlayerFrontShift(owner.transform, 3);
            }
            owner.m_Animator.SetFloat(m_HashStateTime, Mathf.Repeat(owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex("Base Layer")).normalizedTime, 1f));

            //if (invincibleFrame >= invincibleTime)
            //{
            //    owner.isDodge = false;
            //    owner.m_Animator.ResetTrigger(Dodge);
            //    ChangeState<PlayerMotionState>(procedureOwner);
            //}

            //if (owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex(BaseLayer)).IsTag("Dodge"))
            //{
            if (owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex(BaseLayer)).normalizedTime >= 0.1f)
                {
                    ChangeState<PlayerMotionState>(procedureOwner);
                }
           
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
                    owner.isDodge = false;
                    owner.m_Animator.ResetTrigger(Dodge);
            //invincibleFrame = 0;
            owner.isDodge = false;
            owner.m_Animator.ResetTrigger(Dodge);
            owner.m_Animator.SetBool(IsLockEnemy, false);
            Log.Info("退出翻滚");
        }

        public static PlayerDodgeState Create()
        {
            PlayerDodgeState state = ReferencePool.Acquire<PlayerDodgeState>();
            return state;
        }

    }
}
