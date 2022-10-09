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
            //更新玩家位置
            if(!InSkyAttack)
                owner.SetSearchTarget(owner.find_Player.transform);

            //通过时间和攻击状态来判断什么时候降落
            if (!owner.m_Animator.GetBool(Downward) && !InSkyAttack)
            {
                float distance = AIUtility.GetDistanceNoYAxis(owner.find_Player.transform.position,
                    owner.transform.position);

                if (distance <= LimitDistance)
                {
                    DownTimer += elapseSeconds;
                }

                //到达指定时间降落
                //或者可执行攻击同时在限定的距离内降落
                if (DownTimer >= DownTime)
                {
                    owner.m_Animator.SetBool(Flying, false);
                    owner.m_Animator.SetBool(Downward, true);
                }
                else if (distance <= owner.enemyData.AttackRange * 6)
                {
                    //Debug.Log("在攻击范围内");
                    if (owner.IsCanAttack)
                    {
                        if(InSkyAttackTimer >= WaitTime)
                        {
                            //Debug.Log("可攻击");
                            InSkyAttack = true;
                            owner.m_Animator.SetBool(Swoop, true);
                            owner.m_Animator.SetInteger(AttackState, 3);
                            owner.ResetAttack();
                        }
                    }
                }
            }

            if (owner.m_Animator.GetBool(Downward) || InSkyAttack)//下降
            {
                //Debug.Log("下降");

                if(owner.m_Animator.GetBool(Swoop))
                {
                    Debug.Log("锁定");
                    owner.transform.LookAt(owner.find_Player.transform.position);
                }
                else if(owner.m_Animator.GetBool(Swoop) && owner.CurrentHight >= 4f)
                {
                    Debug.Log("旋转");
                    AIUtility.RotateToTarget(owner.find_Player, owner, -10f, 10f);
                }

                //平滑下降
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
            else//飞行中
            {
                //看向玩家
                AIUtility.RotateToTarget(owner.find_Player, owner, -10f, 10f);
                //Debug.Log("飞行");
                //向上平滑移动
                if (owner.IsCanUp && owner.m_Animator.GetBool(Flying))
                {
                    AIUtility.SmoothMove(owner.transform.position,
                        owner.transform.position + owner.transform.up,
                        owner,
                        owner.enemyData.MoveSpeedRun);
                }

                //向前方平滑移动
                AIUtility.SmoothMove(owner.transform.position,
                    owner.transform.forward + owner.transform.position,
                    owner,
                    owner.enemyData.MoveSpeedRun);
            }

            //降落地面之后切换状态
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