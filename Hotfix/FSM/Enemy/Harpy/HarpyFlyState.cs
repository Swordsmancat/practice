using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{

    public class HarpyFlyState : EnemyBaseActionState
    {
        private readonly static int Flying = Animator.StringToHash("Flying");
        private readonly static int Downward = Animator.StringToHash("Downward");
        private readonly static int NearPlayer = Animator.StringToHash("NearPlayer");
        private readonly static int AttackState = Animator.StringToHash("AttackState");
        private readonly static int Swoop = Animator.StringToHash("Swoop");
        private readonly static int DownTime = 1;
        private readonly static int WaitTime = 12;
        private readonly static float LimitDistance = 1f;
        private readonly static float LimitHeight = 0.5f;

        private HarpyLogic owner;
        private float DownTimer;
        private float InSkyAttackTimer;
        private bool InSkyAttack;

        protected override void OnInit(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner as HarpyLogic;
            owner.m_Animator.SetBool(Flying, true);
            owner.SetRichAiStop();
            owner.UnLockEntity();
            DownTimer = 0;
            InSkyAttackTimer = 0;
            InSkyAttack = false;
        }

        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            InSkyAttackTimer += elapseSeconds;
            //�������λ��
            if(!InSkyAttack)
                owner.SetSearchTarget(owner.find_Player.transform);

            //ͨ��ʱ��͹���״̬���ж�ʲôʱ����
            if (!owner.m_Animator.GetBool(Downward) && !InSkyAttack)
            {
                float distance = AIUtility.GetDistanceNoYAxis(owner.find_Player.transform.position,
                    owner.transform.position);

                if (distance <= LimitDistance)
                {
                    DownTimer += elapseSeconds;
                }

                //����ָ��ʱ�併��
                //���߿�ִ�й���ͬʱ���޶��ľ����ڽ���
                if (DownTimer >= DownTime)
                {
                    owner.m_Animator.SetBool(Flying, false);
                    owner.m_Animator.SetBool(Downward, true);
                }
                else if (distance <= owner.enemyData.AttackRange * 6)
                {
                    //Debug.Log("�ڹ�����Χ��");
                    if (owner.IsCanAttack)
                    {
                        if(InSkyAttackTimer >= WaitTime)
                        {
                            //Debug.Log("�ɹ���");
                            InSkyAttack = true;
                            owner.m_Animator.SetBool(Swoop, true);
                            owner.m_Animator.SetInteger(AttackState, 3);
                            owner.ResetAttack();
                        }
                    }
                }
            }

            if (owner.m_Animator.GetBool(Downward) || InSkyAttack)//�½�
            {
                //Debug.Log("�½�");

                if(owner.m_Animator.GetBool(Swoop))
                {
                    Debug.Log("����");
                    owner.transform.LookAt(owner.find_Player.transform.position);
                }
                else if(owner.m_Animator.GetBool(Swoop) && owner.CurrentHight >= 4f)
                {
                    Debug.Log("��ת");
                    AIUtility.RotateToTarget(owner.find_Player, owner, -10f, 10f);
                }

                //ƽ���½�
                if (InSkyAttack)
                {
                    AIUtility.SmoothMove(owner.transform.position,
                    owner.find_Player.transform.position, 
                    owner, owner.enemyData.MoveSpeedRun);
                }
                else
                {
                    AIUtility.SmoothMove(owner.transform.position,
                        owner.transform.forward + -owner.transform.up + owner.transform.position,
                        owner,
                        owner.enemyData.MoveSpeedRun);
                }


                if (owner.CurrentHight <= 5f && owner.m_Animator.GetBool(Downward) == true)
                {
                    owner.m_Animator.SetBool(Downward, false);
                    owner.m_Animator.SetBool(NearPlayer, true);
                }
                else if (InSkyAttack && owner.CurrentHight <= 5f)
                {
                    owner.m_Animator.SetBool(Flying, false);
                    owner.m_Animator.SetBool(Swoop, false);
                    owner.LockEntity(owner.find_Player);
                }
                
            }
            else//������
            {
                //�������
                AIUtility.RotateToTarget(owner.find_Player, owner, -10f, 10f);
                //Debug.Log("����");
                //����ƽ���ƶ�
                if (owner.IsCanUp && owner.m_Animator.GetBool(Flying))
                {
                    AIUtility.SmoothMove(owner.transform.position,
                        owner.transform.position + owner.transform.up,
                        owner,
                        owner.enemyData.MoveSpeedRun);
                }

                //��ǰ��ƽ���ƶ�
                AIUtility.SmoothMove(owner.transform.position,
                    owner.transform.forward + owner.transform.position,
                    owner,
                    owner.enemyData.MoveSpeedRun);
            }

            //�������֮���л�״̬
            if (owner.m_Animator.GetBool(NearPlayer) && owner.CurrentHight <= LimitHeight)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            }
            else if (owner.CurrentHight <= LimitHeight && InSkyAttack)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            }
        }

        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.m_Animator.SetBool(Flying, false);
            owner.m_Animator.SetBool(Downward, false);
            owner.m_Animator.SetBool(NearPlayer, false);
            owner.m_Animator.SetBool(Swoop, false);
            owner.m_Animator.SetInteger(AttackState, -1);
            owner.LockEntity(owner.find_Player);
            DownTimer = 0;
            InSkyAttackTimer = 0;
            InSkyAttack = false;
        }

        public static HarpyFlyState Create()
        {
            HarpyFlyState state = ReferencePool.Acquire<HarpyFlyState>();
            return state;
        }

    }
}