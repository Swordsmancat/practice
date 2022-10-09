using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class Old_HobgoblinMotionState : EnemyMotionState
    {
        private readonly static int m_MoveBlend = Animator.StringToHash("MoveBlend");
        private readonly static float m_GoRunRange = 8f;
        private int m_Num ;
        private bool IsRun = false;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            m_Num = Utility.Random.GetRandom(0, 100);
            base.OnEnter(procedureOwner);
            //Debug.Log("���Rֵ" + m_Num);
            Debug.Log("�����ƶ�״̬");

        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            float disdance = AIUtility.GetDistance(owner, owner.LockingEntity);
            //Debug.Log("����" + disdance);
            if (m_Num >= 60)
            {
                Debug.Log("�ƶ�--����");
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Wind));
            }
            else if (disdance <= 15f && disdance >= 8f)
            {
                //Debug.Log("��� ��"+disdance );
                //Debug.Log("�ƶ�--���");
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Push));
            }
            else if (owner.IsLocking)
            {
                if (disdance >= m_GoRunRange)
                {
                    IsRun = true;
                }
                else
                {
                    IsRun = false;
                }
            }
            if (owner.IsLocking)
            {
                if (owner.LockingEntity != null && !owner.LockingEntity.IsDead)
                {
                    float distance = AIUtility.GetDistance(owner, owner.LockingEntity);

                    //�ж�����Ƿ��ڹ�����Χ��
                    if (!owner.CheckInAttackRange(distance))
                    {
                        //���ڹ�����Χ��
                        //���û����,���ù����ƶ�,���ù���Ŀ��
                        MovementBlend();
                        owner.SetRichAIMove();
                        owner.SetSearchTarget(owner.LockingEntity.CachedTransform);
                    }
                    else if(procedureOwner.CurrentState.GetType() != owner.ChangeStateEnemy(EnemyStateType.Push) && procedureOwner.CurrentState.GetType() != owner.ChangeStateEnemy(EnemyStateType.Wind))
                    {
                        StopMoveBlend();
                        //Debug.Log("�ƶ�--ս�� ����������");
                        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
                    }
                }
            }

            if (owner.CheckInSeekRange(disdance) && !owner.CheckInAttackRange(disdance))
            {
                if (procedureOwner.CurrentState.GetType() != owner.ChangeStateEnemy(EnemyStateType.Motion))
                {
                    if (procedureOwner.CurrentState.GetType() != owner.ChangeStateEnemy(EnemyStateType.Push))
                    {
                        if (procedureOwner.CurrentState.GetType() != owner.ChangeStateEnemy(EnemyStateType.Wind))
                        {
                            //Debug.Log("���ƶ�����̡�����--�ƶ� ����Ұ��Χ�� ���������� ");
                            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
                            return;
                        }
                    }
                }
            }
        }
        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            Debug.Log("�˳��ƶ�״̬");

        }


        public static new Old_HobgoblinMotionState Create()
        {
            Old_HobgoblinMotionState state = ReferencePool.Acquire<Old_HobgoblinMotionState>();
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
    }
}

