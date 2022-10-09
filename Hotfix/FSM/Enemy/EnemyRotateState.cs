using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    /// <summary>
    /// �ṩ�����麯������̳�����д��Ӧ�߼�
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
            //Debug.Log("������ת״̬");
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            owner.SetRichAiStop();
            AIUtility.RotateToTarget(m_Plyaer, owner, 
                MinAngle, MaxAngle, 
                RotateSpeed, turnTime);

            float angle = AIUtility.GetPlaneAngle(m_Plyaer, owner);
            //Log.Info("��ԽǶ�" + angle);

            //Angle < 10f && Angle > -10f
            //����
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
        /// ������ת״̬��ʼ
        /// </summary>
        protected virtual void EnemyRotateStateStart(EnemyLogic owner)
        {
            owner.SetRichAiStop();
            owner.LockEntity(owner.find_Player);
            m_Plyaer = owner.LockingEntity;
            /*owner.UnLockEntity();*///�������ҵ�������Ȼ�����ֱ��˲���������
            RotateSpeed = 100f;
            owner.m_Animator.SetBool(m_Rotate, true);
            owner.IsBlock = true;
        }

        /// <summary>
        /// ������ת״̬����
        /// </summary>
        protected virtual void EnemyRotateStateEnd(EnemyLogic owner)
        {
            owner.m_Animator.SetBool(m_Rotate, false);
            owner.LockEntity(m_Plyaer);
            owner.IsBlock = false;
        }
    }
}

