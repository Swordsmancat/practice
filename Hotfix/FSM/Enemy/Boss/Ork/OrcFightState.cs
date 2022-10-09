using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class OrcFightState : EnemyFightState
    {
        private readonly static int m_AvoidBack = Animator.StringToHash("AvoidBack");
        private readonly static int m_AvoidPrec = 10;
        private readonly float AvoidSpeed = 15f;
        private bool OnlyOnce = true;
        private int m_ChanceDo;
        private EnemyLogic owner;
        
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            m_ChanceDo = Utility.Random.GetRandom(0, 100);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            OnlyOnce = true;
        }

        public static OrcFightState Create()
        {
            OrcFightState state = ReferencePool.Acquire<OrcFightState>();
            return state;
        }


        protected override void InAttackRange(ProcedureOwner fsm)
        {
            base.InAttackRange(fsm);

            if (m_ChanceDo <= m_AvoidPrec && owner.find_Player.m_moveBehaviour.isAttack)
            {
                if(OnlyOnce)
                {
                    //int num = Utility.Random.GetRandom(0, 3);
                    float distance = AIUtility.GetDistance(owner.LockingEntity, owner);
                    if(distance <= owner.enemyData.AttackRange)
                    {
                        AvoidAttack(0);
                        OnlyOnce = false;
                    }
                }
            }
        }

        private void AvoidAttack(int num)
        {
            Debug.Log("¶ã±Ü¶ã±Ü¶ã±Ü£¡£¡£¡");
            switch (num)
            {
                case (int)AvoidType.back:
                    AvoidToBack();
                    break;
                //case (int)AvoidType.left:
                //    owner.m_Animator.SetTrigger(m_BlockRollLeft);
                //    break;
                //case (int)AvoidType.right:
                //    owner.m_Animator.SetTrigger(m_BlockRollRight);
                //    break;
            }
        }

        private void AvoidToBack()
        {
            owner.m_Animator.SetTrigger(m_AvoidBack);
            owner.transform.Translate(AvoidSpeed * Time.deltaTime *  Vector3.back, Space.Self);
        }

    }
}
 
