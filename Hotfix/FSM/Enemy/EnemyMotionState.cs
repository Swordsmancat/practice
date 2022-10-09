using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using Pathfinding;

namespace Farm.Hotfix
{
   public class EnemyMotionState : EnemySeekState
    {
        private readonly static int s_moveBlend = Animator.StringToHash("MoveBlend");
        
        protected EnemyLogic owner;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            //owner.m_Animator.speed /= owner.m_Animator.humanScale;
            //不同模型同位移速度
            //owner.m_Animator.SetFloat("ScaleFactor", 1 / owner.m_Animator.humanScale);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (owner.IsLocking)
            {
                if(owner.LockingEntity != null && !owner.LockingEntity.IsDead)
                {
                    float distance = AIUtility.GetDistance(owner, owner.LockingEntity);
                    //判断玩家是否在攻击范围内
                    if (!owner.CheckInAttackRange(distance))
                    {
                        //若在攻击范围外
                        //设置混合树,设置怪物移动,设置怪物目标
                        MovementBlend();
                        owner.SetRichAIMove();
                        owner.SetSearchTarget(owner.LockingEntity.CachedTransform);
                        //Debug.Log("进行移动中`");
                    }
                    else
                    {
                        StopMoveBlend();
                        //Debug.Log("基础移动--战斗");
                        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
                    }
                }
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

        public static EnemyMotionState Create()
        {
            EnemyMotionState state = ReferencePool.Acquire<EnemyMotionState>();
            return state;
        }

        /// <summary>
        /// 移动方式
        /// </summary>
        protected virtual void MovementBlend()
        {
            owner.m_Animator.SetFloat(s_moveBlend, 1f,0.5f,Time.deltaTime * 2);
        }

        /// <summary>
        /// 停止移动
        /// </summary>
        protected virtual void StopMoveBlend()
        {
            //Debug.Log("停止移动");
            //owner.m_Animator.SetFloat(s_moveBlend, owner.m_Animator.GetFloat(s_moveBlend) * 0.5f);
            //if (owner.m_Animator.GetFloat(s_moveBlend) < 0.1)
            owner.m_Animator.SetFloat(s_moveBlend, 0f);


        }
    }
}
