using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;


namespace Farm.Hotfix
{
    public class EnemyDeadState : FsmState<EnemyLogic>, IReference
    {
        protected EnemyLogic owner;

        private static readonly int DeadState = Animator.StringToHash("Dead");
        protected static readonly int DieState = Animator.StringToHash("DieState");

        protected override void OnEnter(ProcedureOwner fsm)
        {
            base.OnEnter(fsm);
            owner = fsm.Owner;
            owner.SetRichAiStop();
            owner.UnLockEntity();
            owner.EnemyAttackEnd();
            //owner.m_Animator.SetBool(DeadState,true);
            Die();
            GameEntry.Sound.PlaySound(owner.enemyData.DeadSoundId);
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
        }

        public static EnemyDeadState Create()
        {
            EnemyDeadState state = ReferencePool.Acquire<EnemyDeadState>();
            return state;
        }

        public void Clear()
        {
            owner = null;
        }
        protected virtual void Die()//死亡动画随机播放一种
        {
            Log.Info("播放死亡动画");
            int randomNum = Utility.Random.GetRandom(0,2);
            owner.m_Animator.SetInteger(DieState, randomNum);
            owner.m_Animator.SetBool(DeadState, true);
        }
    }
}

