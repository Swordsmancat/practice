using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class HobgoblinAttackState : EnemyAttackState
    {
        private readonly float FRIST_PHASE = 0.4f;
        // private readonly float SECOND_PHASE = 0.4f;
        private readonly float PushDistance = 2f;
        private readonly float CloseRange = 0.2f;
        private readonly float PushPrec = 10f;


        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            //Debug.Log("进入攻击状态");
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            owner.AttackLockEntity(owner.find_Player);
            owner.SetSearchTarget(owner.LockingEntity.CachedTransform);
            float distance = AIUtility.GetDistance(owner.LockingEntity, owner);
        }
        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.m_Animator.speed = 1f;
            owner.m_Animator.SetInteger(AttackState, DefaultState);
            //Debug.Log("退出攻击状态");
        }



        public static new HobgoblinAttackState Create()
        {
            HobgoblinAttackState state = ReferencePool.Acquire<HobgoblinAttackState>();
            return state;
        }

        protected override void EnemyAttackStateStart(EnemyLogic owner)
        {
            owner.SetRichAiStop();
            StopBlend(owner);
            int num = Utility.Random.GetRandom(0, 100);
            if (owner.enemyData.HPRatio >= FRIST_PHASE)
            {
                if (num <= 10)
                {
                    int randomNum = Utility.Random.GetRandom(0, 3);
                    owner.m_Animator.SetInteger(AttackState, randomNum);
                }
                else
                {
                    int randomNum = Utility.Random.GetRandom(3, 6);
                    owner.m_Animator.SetInteger(AttackState, randomNum);
                }
            }
            else if (owner.enemyData.HPRatio <= FRIST_PHASE)
            {
                //第二阶段
                owner.ReduceAttackTime = FRIST_PHASE;
                if (num <= 40)
                {
                    int randomNum = Utility.Random.GetRandom(0, 3);
                    owner.m_Animator.SetInteger(AttackState, randomNum);
                }
                else
                {
                    int randomNum = Utility.Random.GetRandom(3, 6);
                    owner.m_Animator.SetInteger(AttackState, randomNum);
                }
            }
        }
    }
}