using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class DragonideShoutState : EnemyShoutState
    {
        private readonly static int m_Shout = Animator.StringToHash("Shout");
        private readonly static int m_ShoutEnd = Animator.StringToHash("ShoutEnd");
        private static readonly int FrenzyEnd = Animator.StringToHash("FrenzyEnd");
        private static readonly int Frenzy = Animator.StringToHash("Frenzy");
        private bool isFrenzyEnd;
        AnimatorStateInfo info;
        private EnemyLogic owner;
        protected override void EnterShoutState(ProcedureOwner procedureOwner)
        {
            owner = procedureOwner.Owner;
            
            
            owner.m_Animator.SetTrigger(m_Shout);
            //Debug.Log("²¥·Å¶¯»­");
            owner.m_Animator.SetBool(m_ShoutEnd, false);
            //²âÊÔÒôÆµ
            GameEntry.Sound.PlaySound(26020);
            
            
        }

        protected override void UpdateShoutState(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {

            info = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime >= 0.9f)
            {
                //owner.AnimationEnd();
                //Debug.Log("ºð½ÐÖÐ" + owner.IsAnimPlayed);
                //owner.AnimationEnd();
                if (info.IsName("buff_01"))
                {
                    //
                    //Debug.Log("ºð½Ð½øÐÐÖÐ" + owner.IsAnimPlayed);
                    owner.AnimationEnd();
                    owner.IsAnimPlayed = true;
                    //owner.m_Animator.SetBool(m_ShoutEnd, true);
                }

                
            }
            if (owner.IsAnimPlayed)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            }

        }
        public static DragonideShoutState Create()
        {
            DragonideShoutState state = ReferencePool.Acquire<DragonideShoutState>();
            return state;
        }


        protected override void LeaveShoutState()
        {
            owner.m_Animator.SetBool(m_ShoutEnd, true);
            owner.IsAnimPlayed = false;
        }
    }
}
