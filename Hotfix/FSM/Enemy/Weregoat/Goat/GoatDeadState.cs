using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
namespace Farm.Hotfix
{
    public class GoatDeadState : EnemyDeadState
    {
        private static readonly int Die = Animator.StringToHash("Die");

        protected override void OnEnter(ProcedureOwner fsm)
        {
            owner = fsm.Owner;
            owner.SetRichAiStop();
            owner.UnLockEntity();
            owner.EnemyAttackEnd();
            //Debug.Log("À¿Õˆ∂Øª≠" + deadNum);
            owner.m_Animator.SetTrigger(Die);
        }

        public static GoatDeadState Create()
        {
            GoatDeadState state = ReferencePool.Acquire<GoatDeadState>();
            return state;
        }
    }
}