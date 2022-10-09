using UnityEngine;
using System.Collections;
using Pathfinding;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework.Fsm;
using GameFramework;
using UnityGameFramework.Runtime;


namespace Farm.Hotfix
{

    public class Old_HobgoblinPatrolState : EnemyMotionState
    {
        public float radius = 20;
        //private EnemyLogic owner;
        private IAstarAI m_Ai;
        private readonly static int m_MoveBlend = Animator.StringToHash("MoveBlend");
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            owner = procedureOwner.Owner;
            m_Ai = owner.GetAICompoent();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
              base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (!m_Ai.pathPending && (m_Ai.reachedEndOfPath || !m_Ai.hasPath))
            {
                m_Ai.destination = PickRandomPoint();
                owner.m_Animator.SetFloat(m_MoveBlend, 1f);
                owner.SetRichAIMove();
                //m_Ai.SearchPath();
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }


        public static new Old_HobgoblinPatrolState Create()
        {
            Old_HobgoblinPatrolState state = ReferencePool.Acquire<Old_HobgoblinPatrolState>();
            return state;
        }



        Vector3 PickRandomPoint()
        {
            var point = Random.insideUnitSphere * radius;
            point.y = 0;
            point += m_Ai.position;
            return point;
        }
    }
}

