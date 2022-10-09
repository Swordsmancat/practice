//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/8/8/周一 12:23:39
//------------------------------------------------------------
using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class EnemyGetUpState :EnemyBaseActionState
    {
        private EnemyLogic owner;
        private readonly string Layer = "Base Layer";
        private static readonly int GetUp = Animator.StringToHash("GetUp");

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex(Layer)).normalizedTime >= 0.9f)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Idle));
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

        public static EnemyGetUpState Create()
        {
            EnemyGetUpState state = ReferencePool.Acquire<EnemyGetUpState>();
            return state;
        }
    }
}
