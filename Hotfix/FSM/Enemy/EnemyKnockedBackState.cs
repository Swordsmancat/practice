//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/8/8/周一 10:53:29
//------------------------------------------------------------

using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class EnemyKnockedBackState : EnemyBaseState
    {
        private EnemyLogic owner;
        private readonly string Layer = "Base Layer";//动画层
        private static readonly int KnockedBack = Animator.StringToHash("KnockedBack");

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.Buff.BuffTypeEnum = BuffType.None;
            owner.m_Animator.SetTrigger(KnockedBack);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            /// TODO:切换状态
            if (owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex(Layer)).normalizedTime >= 0.9f)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.GetUp));
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

        }

        public static EnemyKnockedBackState Create()
        {
            EnemyKnockedBackState state = ReferencePool.Acquire<EnemyKnockedBackState>();
            return state;
        }
    }
}
