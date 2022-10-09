using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
namespace Farm.Hotfix
{ 
    public class WeregoatDeadState:EnemyDeadState
    {
        private static readonly int DeadState = Animator.StringToHash("DieState");
        private static readonly int Die = Animator.StringToHash("Die");

        protected override void OnEnter(ProcedureOwner fsm)
        {
            //base.OnEnter(fsm);
            int deadNum = Utility.Random.GetRandom(0, 2);
            owner = fsm.Owner;
            owner.SetRichAiStop();
            owner.UnLockEntity();
            owner.EnemyAttackEnd();
            //Debug.Log("À¿Õˆ∂Øª≠" + deadNum);
            owner.m_Animator.SetTrigger(Die);
            owner.m_Animator.SetInteger(DeadState, deadNum);
            
            //GameEntry.Sound.PlaySound(owner.enemyData.DeadSoundId);
        }

        protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            owner.EnemyAttackEnd();
            //if (owner.IsAnimPlayed)
            //{
            //    //owner.m_Animator.SetBool(DeadState, false);
            //    owner.m_Animator.SetInteger(DieState, -1);
            //}
        }
        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            //if (owner.IsAnimPlayed)
            //    owner.m_Animator.SetInteger(DieState, -1);
        }
        public static WeregoatDeadState Create()
        {
            WeregoatDeadState state = ReferencePool.Acquire<WeregoatDeadState>();
            return state;
        }
    }
}
