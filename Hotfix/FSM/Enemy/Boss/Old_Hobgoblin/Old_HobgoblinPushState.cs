using UnityEngine;
using System;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class Old_HobgoblinPushState : EnemyBaseActionState
    {
        protected static readonly int Push_Hit = Animator.StringToHash("Push_Hit");
        protected static readonly int Out_Push = Animator.StringToHash("Out_Push");
        protected static readonly int AttackState = Animator.StringToHash("AttackState");
        float Push_Time = 0;
        protected EnemyLogic owner;




        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Push_Time = 0;
            int m_randomNum = Utility.Random.GetRandom(11, 13);
            owner = procedureOwner.Owner;
            owner.m_Animator.SetInteger(AttackState, m_randomNum);
            owner.m_Animator.SetBool(Push_Hit, false);
            owner.m_Animator.SetBool(Out_Push, false);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            owner.AttackLockEntity(owner.find_Player);
            owner.SetSearchTarget(owner.LockingEntity.CachedTransform);
            float distance_push = AIUtility.GetDistance(owner.LockingEntity, owner);
            Push_Time += elapseSeconds;
            if (distance_push <= 5f)
            {
                owner.m_Animator.SetBool(Push_Hit, true);
                if(owner.IsAnimPlayed) 
                {
                    //Debug.Log("冲刺距离" + distance);
                    //Debug.Log("冲刺--攻击");
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
                }
            }
            else if(Push_Time >= 15f)
            {
                Debug.Log("冲刺时间" + Push_Time);
                owner.m_Animator.SetBool(Out_Push , true);
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Wind));
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            //Debug.Log("冲刺 关" + p_distance);
            Push_Time = 0;
            owner.m_Animator.SetInteger(AttackState, -1);
            owner.m_Animator.SetBool(Push_Hit, false);
            owner.m_Animator.SetBool(Out_Push, false);
        }

        public static new Old_HobgoblinPushState Create()
        {
            Old_HobgoblinPushState state = ReferencePool.Acquire<Old_HobgoblinPushState>();
            return state;
        }
    }
}
