using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
namespace Farm.Hotfix
{
    public class DragonideDeadState : EnemyDeadState
    {
        private static readonly int DeadState = Animator.StringToHash("DieState");
        private static readonly int Die = Animator.StringToHash("Die");

        protected override void OnEnter(ProcedureOwner fsm)
        {
            int deadNum = Utility.Random.GetRandom(0, 2);
            owner = fsm.Owner;
            owner.SetRichAiStop();
            owner.UnLockEntity();
            owner.EnemyAttackEnd();
            //Debug.Log("À¿Õˆ∂Øª≠" + deadNum);
            owner.m_Animator.SetTrigger(Die);
            owner.m_Animator.SetInteger(DeadState,deadNum);
            GameEntry.Sound.PlaySound(owner.enemyData.DeadSoundId);
        }
            public static DragonideDeadState Create()
        {
            DragonideDeadState state = ReferencePool.Acquire<DragonideDeadState>();
            return state;
        }
        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            owner.m_Animator.SetInteger(DeadState, -1);
            base.OnLeave(fsm, isShutdown);
        }
    }
}