using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

namespace Farm.Hotfix
{
    public class DK_GoblinShoutState : EnemyShoutState
    {
        // Start is called before the first frame update
        private readonly static int m_Shout = Animator.StringToHash("Shout");
        private readonly static int m_ShoutEnd = Animator.StringToHash("ShoutEnd");
        private EnemyLogic owner;
        int[] shout = { 20031, 20035, 20036 };//“Ù∆µ ˝◊È
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            EnterShoutState(procedureOwner);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            UpdateShoutState(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            LeaveShoutState();
        }

        public static DK_GoblinShoutState Create()
        {
            DK_GoblinShoutState state = ReferencePool.Acquire<DK_GoblinShoutState>();
            return state;
        }

        #region virutal function

        protected override void EnterShoutState(ProcedureOwner procedureOwner)
        {
            owner = procedureOwner.Owner;
            owner.m_Animator.SetTrigger(m_Shout);
            owner.m_Animator.SetBool(m_ShoutEnd, false);
            System.Random rand = new System.Random();
            int result = shout[rand.Next(shout.Length)];
            //≤‚ ‘“Ù∆µ
            GameEntry.Sound.PlaySound(result);
        }

     
        protected override void UpdateShoutState(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            if (owner.IsAnimPlayed)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            }
        }

        protected override void LeaveShoutState()
        {
            owner.m_Animator.SetBool(m_ShoutEnd, true);
            owner.IsAnimPlayed = false;
        }
        #endregion
    }
}
