using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    
    public class UndeadShoutState : EnemyBaseState
    {
        private readonly static int fightSecond = Animator.StringToHash("fightSecond");
        //private readonly static int m_ShoutEnd = Animator.StringToHash("ShoutEnd");
        private readonly static float ExitTime = 2f;
        private UndeadLogic owner;
     

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner as UndeadLogic;
            owner.SetRichAiStop();
            owner.m_Animator.SetBool(fightSecond, true);
            //owner.m_Animator.SetTrigger(fightSecond);

        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            owner.ShoutTime +=Time.deltaTime;
            if (owner.ShoutTime >= ExitTime)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
            }

           
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.IsAnimPlayed = false;
            owner.m_Animator.SetBool(fightSecond, false);
            LeaveShoutState();
        }

       

        public static UndeadShoutState Create()
        {
            UndeadShoutState state = ReferencePool.Acquire<UndeadShoutState>();
            return state;
        }


        protected virtual void EnterShoutState(ProcedureOwner procedureOwner)
        {
                owner.m_Animator.SetTrigger(fightSecond); 
            
            //owner.m_Animator.SetBool(m_ShoutEnd, false);
            //≤‚ ‘“Ù∆µ
            //GameEntry.Sound.PlaySound(20014);
        }

       
  

    
        protected virtual void LeaveShoutState()
        {
            //owner.m_Animator.SetBool(m_ShoutEnd, true);
            owner.IsAnimPlayed = false;
        }

       
    }
}

