using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using Farm.Hotfix;

namespace Farm.Hotfix
{
    public class UndeadMotionState : EnemyMotionState
    {
        // Start is called before the first frame update
        private readonly static int m_MoveBlend = Animator.StringToHash("MoveBlend");
        private readonly static float m_GoRunRange = 8f;
        private bool IsRun = false;
        new private UndeadLogic owner;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner as UndeadLogic;
        }
        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.IsAnimPlayed = false;
            owner.IsBlock = false;
            
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            if (owner.IsLocking)
            {
                float disdance = AIUtility.GetDistance(owner, owner.LockingEntity);//获取怪兽和敌人之间的距离
                if (disdance >= m_GoRunRange)//当这个距离大于m_GoRunRange
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

        public static new UndeadMotionState Create()
        {
            UndeadMotionState state = ReferencePool.Acquire<UndeadMotionState>();
            return state;
        }

        protected override void MovementBlend()
        {
            if (IsRun)
            {
                //播放移动的动画
                owner.m_Animator.SetFloat(m_MoveBlend, 2f, 0.5f, Time.deltaTime * 1);
            }
            else
            {
                owner.m_Animator.SetFloat(m_MoveBlend, 1f, 0.5f, Time.deltaTime * 2);
                owner.IsBlock = true;
                
            }
            //if (owner.CurrentTargetDisdance<=2f)
            //{
            //    owner.m_Animator.SetFloat(m_MoveBlend, 3f, 0.5f, Time.deltaTime * 2);
            //}
           
        }

        protected override void LockPlayerDo(ProcedureOwner procedureOwner)
        {
            owner.LockEntity(owner.find_Player);
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
        }

    }
}
