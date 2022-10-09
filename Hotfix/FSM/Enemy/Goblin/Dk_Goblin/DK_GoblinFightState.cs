using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Farm.Hotfix
{

    public class DK_GoblinFightState : EnemyFightState
    {
        // Start is called before the first frame update
        private static readonly int m_AvoidBack = Animator.StringToHash("Avoidback");
        private static readonly int m_Avoidfront = Animator.StringToHash("AvoidFront");
        private static readonly int m_Avoidleft = Animator.StringToHash("Avoidleft");
        private static readonly int m_AvoidRight = Animator.StringToHash("AvoidRight");
        private static readonly int CanAvoid = Animator.StringToHash("CanAvoid");
        private Dk_GoblinLogic owner;

        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner as Dk_GoblinLogic;
            owner.m_Animator.SetBool(CanAvoid, true);
        }

        //protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        //{
        //    base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        //    if (owner.IsAnimPlayed)
        //    {
        //        owner.m_Animator.SetBool(CanAvoid, false);

        //    }
        //}
        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.IsAnimPlayed = false;
            owner.m_Animator.SetBool(CanAvoid, false);
            owner.IsHurt = false;

        }
        public static new DK_GoblinFightState Create()
        {
            DK_GoblinFightState state = ReferencePool.Acquire<DK_GoblinFightState>();
            return state;
        }

        protected override void InAttackRange(IFsm<EnemyLogic> fsm)
        {
            base.InAttackRange(fsm);
            int num = Utility.Random.GetRandom(0, 4);
            AvoidAttack(num);
            
            //if (m_ChanceDo >= m_RollStatePrec && owner.find_Player.m_moveBehaviour.isAttack)
            //{

            //    if (OnlyOnce)
            //    {
            //        int num = Utility.Random.GetRandom(0, 4);
            //        AvoidAttack(num);
            //        OnlyOnce = false;
            //    }

            //}

        }

        private void AvoidAttack(int num)
        {
            Debug.Log("¶ã±Ü¶ã±Ü¶ã±Ü£¡£¡£¡");
            switch (num)
            {
                case (int)AvoidType.back:
                    owner.m_Animator.SetTrigger(m_AvoidBack);
                    break;
                case (int)AvoidType.front:
                    owner.m_Animator.SetTrigger(m_Avoidfront);
                    break;
                case (int)AvoidType.left:
                    owner.m_Animator.SetTrigger(m_Avoidleft);
                    break;
                case (int)AvoidType.right:
                    owner.m_Animator.SetTrigger(m_AvoidRight);
                    break;
            }
            owner.m_Animator.SetBool(CanAvoid, false);
        }
    }
}