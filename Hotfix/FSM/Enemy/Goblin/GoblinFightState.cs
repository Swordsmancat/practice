using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Farm.Hotfix
{
    public class GoblinFightState : EnemyFightState
    {
        private readonly static int m_FightBlend = Animator.StringToHash("FightBlend");
        private readonly static int m_BlockRollLeft = Animator.StringToHash("BlockRollLeft");
        private readonly static int m_BlockRollRight = Animator.StringToHash("BlockRollRight");
        private readonly static int m_AvoidBack = Animator.StringToHash("AvoidBack");
        private readonly static int m_Avoidfront = Animator.StringToHash("AvoidFront");
        private readonly static int m_BlockStatePerc = 30;
        private readonly static int m_RollStatePrec = 50;
        private bool OnlyOnce;
        private int m_ChanceDo;
        private GoblinLogic owner;

        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner as GoblinLogic;
            owner.BlockTime = 0;
            owner.IsBlock = false;
            OnlyOnce = true;
            m_ChanceDo = Utility.Random.GetRandom(0, 100);
        }

        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            //float disdance = AIUtility.GetDistance(owner, owner.LockingEntity);
            //if (!owner.CheckInAttackRange(disdance))
            //{
            //    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            //}
            //else
            //{
            //    if(owner.IsCanAttack)
            //    {
            //        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Attack));
            //    }

            //    if(m_ChanceDo >= m_RollStatePrec && owner.find_Player.m_moveBehaviour.isAttack)
            //    {
                    
            //        if(OnlyOnce)
            //        {
            //            int num = Utility.Random.GetRandom(0, 3);
            //            Debug.Log("¶ã±Ü¶ã±Ü¶ã±Ü£¡£¡£¡");
            //            switch (num)
            //            {
            //                case (int)AvoidType.back:
            //                    owner.m_Animator.SetTrigger(m_AvoidBack);
            //                    break;
            //                case (int)AvoidType.left:
            //                    owner.m_Animator.SetTrigger(m_BlockRollLeft);
            //                    break;
            //                case (int)AvoidType.right:
            //                    owner.m_Animator.SetTrigger(m_BlockRollRight);
            //                    break;
            //            }
            //            OnlyOnce = false;
            //        }

            //    }

            //    if (m_ChanceDo <= m_BlockStatePerc)
            //    {
            //        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Block));
            //    }
            //}
        }

        public static GoblinFightState Create()
        {
            GoblinFightState state = ReferencePool.Acquire<GoblinFightState>();
            return state;
        }

        protected override void InAttackRange(IFsm<EnemyLogic> fsm)
        {
            base.InAttackRange(fsm);

            if (m_ChanceDo >= m_RollStatePrec && owner.find_Player.m_moveBehaviour.isAttack)
            {

                if (OnlyOnce)
                {
                    int num = Utility.Random.GetRandom(0, 4);
                    AvoidAttack(num);
                    OnlyOnce = false;
                }

            }

            if (m_ChanceDo <= m_BlockStatePerc)
            {
                ChangeState(fsm, owner.ChangeStateEnemy(EnemyStateType.Block));
            }
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
                    owner.m_Animator.SetTrigger(m_BlockRollLeft);
                    break;
                case (int)AvoidType.right:
                    owner.m_Animator.SetTrigger(m_BlockRollRight);
                    break;
            }
        }
    }
}


