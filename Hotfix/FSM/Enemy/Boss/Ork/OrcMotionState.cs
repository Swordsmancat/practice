using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class OrcMotionState : EnemyMotionState
    {
        private readonly static int m_MoveBlend = Animator.StringToHash("MoveBlend");
        private readonly static float m_GoRunRange = 8f;
        private bool IsRun = false;

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            if (owner.IsLocking)
            {
                float disdance = AIUtility.GetDistance(owner, owner.LockingEntity);
                if (disdance >= m_GoRunRange)
                {
                    IsRun = true;
                }
                else
                {
                    IsRun = false;
                }
            }

            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        public static new OrcMotionState Create()
        {
            OrcMotionState state = ReferencePool.Acquire<OrcMotionState>();
            return state;
        }

        protected override void MovementBlend()
        {
            if (IsRun)
            {
                owner.m_Animator.SetFloat(m_MoveBlend, 2f, 0.5f, Time.deltaTime * 2);
            }
            else
            {
                owner.m_Animator.SetFloat(m_MoveBlend, 1f, 0.5f, Time.deltaTime * 2);
            }
        }

        protected override void LockPlayerDo(ProcedureOwner procedureOwner)
        {
            owner.LockEntity(owner.find_Player);
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Shout));
        }

    }
}

