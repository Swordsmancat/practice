using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    /// <summary>
    /// 提供两个虚函数方便继承类重写相应逻辑
    /// </summary>
    public class EnemyRotateState : EnemyBaseActionState
    {

        private readonly int m_Rotate = Animator.StringToHash("Rotate");
        private readonly float MinAngle = -3f;
        private readonly float MaxAngle = 3f;
        private readonly float turnTime = 400f;
        private EnemyLogic owner;
        private TargetableObject m_Plyaer;
        protected float RotateSpeed;
        

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            EnemyRotateStateStart(owner);
            //Debug.Log("进入旋转状态");
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            owner.SetRichAiStop();
            AIUtility.RotateToTarget(m_Plyaer, owner, 
                MinAngle, MaxAngle, 
                RotateSpeed, turnTime);

            float angle = AIUtility.GetPlaneAngle(m_Plyaer, owner);
            //Log.Info("相对角度" + angle);

            //Angle < 10f && Angle > -10f
            //结束
            if (AIUtility.CheckInAngle(MinAngle, MaxAngle, angle))
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            EnemyRotateStateEnd(owner);
        }


        public static EnemyRotateState Create()
        {
            EnemyRotateState state = ReferencePool.Acquire<EnemyRotateState>();
            return state;
        }

        /// <summary>
        /// 敌人旋转状态开始
        /// </summary>
        protected virtual void EnemyRotateStateStart(EnemyLogic owner)
        {
            owner.SetRichAiStop();
            owner.LockEntity(owner.find_Player);
            m_Plyaer = owner.LockingEntity;
            /*owner.UnLockEntity();*///解除对玩家的锁定不然怪物会直接瞬间面向玩家
            RotateSpeed = 100f;
            owner.m_Animator.SetBool(m_Rotate, true);
            owner.IsBlock = true;
        }

        /// <summary>
        /// 敌人旋转状态结束
        /// </summary>
        protected virtual void EnemyRotateStateEnd(EnemyLogic owner)
        {
            owner.m_Animator.SetBool(m_Rotate, false);
            owner.LockEntity(m_Plyaer);
            owner.IsBlock = false;
        }
    }
}

