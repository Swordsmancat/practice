//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/8/8/周一 10:53:29
//------------------------------------------------------------

using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class EnemyKnockedDownState : EnemyBaseState
    {
        private EnemyLogic owner;
        private readonly string Layer = "Base Layer";//动画层
        private static readonly int KnockedDown = Animator.StringToHash("KnockedDown");
        private float outTime = 1.8f;
        private float currentTime;
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.Buff.BuffTypeEnum = BuffType.None;
            owner.m_Animator.SetTrigger(KnockedDown);
            //Log.Info("倒地状态");
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            currentTime += Time.deltaTime;
            Log.Info("倒地时间" + currentTime);
            if (currentTime > outTime)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
                
            }
            //if (owner.m_AnimationEventGetUp)
            //{
            //    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Idle));
            //}
            /// TODO:切换状态
                //if (owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex(Layer)).normalizedTime >= 0.9f)
                //{
                //   
                //}
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            currentTime = 0;
        }

        public static EnemyKnockedDownState Create()
        {
            EnemyKnockedDownState state = ReferencePool.Acquire<EnemyKnockedDownState>();
            return state;
        }
    }
}
