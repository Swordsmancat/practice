using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using Farm.Hotfix;

namespace Farm.Hotfix
{
    public class DK_GoblinMotionState : EnemyMotionState
{
    private readonly static int m_NormalBlend = Animator.StringToHash("NormalBlend");
    private readonly static int runEnd = Animator.StringToHash("runEnd");
        private readonly static int run = Animator.StringToHash("Run");
        private readonly static int m_GoRunRange = 3;
    private bool IsRun = false;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.LockEntity(owner.find_Player);
        }
        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
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
        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            owner.IsAnimPlayed = false;
        }
        public static new DK_GoblinMotionState Create()
    {
            DK_GoblinMotionState state = ReferencePool.Acquire<DK_GoblinMotionState>();
        return state;
    }

    protected override void MovementBlend()
    {
        if (IsRun)
        {
            //播放移动的动画
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
        owner.m_Animator.SetBool(runEnd, true);
        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Attack));
    }
}
}

