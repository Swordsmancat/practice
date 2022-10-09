using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using Farm.Hotfix;

namespace Farm.Hotfix
{
    public class GoblinMotionState : EnemyMotionState
    {
        private readonly static int m_NormalBlend = Animator.StringToHash("NormalBlend");
        private readonly static int m_FightBlend = Animator.StringToHash("FightBlend");
        private readonly static int m_InFight = Animator.StringToHash("InFight");
        private readonly static int m_FightStateRange = 5;
        private readonly static int m_GoRunRange = 15;
        private bool IsRun = false;
        private bool IsInFight = false;  //¹¥»÷×´Ì¬

        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            if(owner.IsLocking)
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

        public static new GoblinMotionState Create()
        {
            GoblinMotionState state = ReferencePool.Acquire<GoblinMotionState>();
            return state;
        }

        protected override void MovementBlend()
        {
            if (IsRun)
            {
                //²¥·ÅÒÆ¶¯µÄ¶¯»­
                owner.m_Animator.SetFloat(m_NormalBlend, 2f, 0.5f, Time.deltaTime * 1);
            }
            else
            {
                owner.m_Animator.SetFloat(m_NormalBlend, 1f, 0.5f, Time.deltaTime * 2);
            }
        }

        protected override void StopMoveBlend()
        {
            owner.m_Animator.SetFloat(m_NormalBlend, 0f);
          
        }
        protected override void LockPlayerDo(ProcedureOwner procedureOwner)
        {
            owner.LockEntity(owner.find_Player);
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Attack));
        }
    }
}

