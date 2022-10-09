using GameFramework;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{

    public class WeregoatFightState : EnemyFightState
    {
        private readonly static int m_BlockRollLeft = Animator.StringToHash("BlockRollLeft");
        private readonly static int m_BlockRollRight = Animator.StringToHash("BlockRollRight");
        private readonly static int m_AvoidBack = Animator.StringToHash("AvoidBack");
        private readonly static int m_AvoidFront = Animator.StringToHash("AvoidFront");
        private readonly static int m_Appel = Animator.StringToHash("Appel");
        private readonly static int m_AppelState = Animator.StringToHash("AppelState");
        AnimatorStateInfo info;

        private float WaitTime = 0.5f;  //等待时间
        private float TimeDoChangeState = 0;
        private float SECOND_PHASE = 0.5f;
        private readonly static int m_BlockStatePerc = 10;
        private readonly static int m_RollStatePrec = 30;
        private bool OnlyOnce;
        private int m_ChanceDo;

        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            //owner = procedureOwner.Owner as DragonideLogic;
            //owner.BlockTime = 0;
            //owner.IsBlock = false;
            OnlyOnce = true;
            owner.HideTrail();//角色倒地 关闭拖尾和攻击检测 防止怪物碰到角色武器会一直受伤
            owner.EnemyAttackEnd();//同上
            m_ChanceDo = Utility.Random.GetRandom(0, 100);
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            info = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
            FightAnimationEnd();
            float disdance = AIUtility.GetDistance(owner, owner.find_Player);
            
            if (!owner.CheckInAttackRange(disdance))
            {
                if (owner.enemyData.HPRatio <= SECOND_PHASE)
                {
                    WaitTime = 0.2f;
                }
                TimeDoChangeState += elapseSeconds;
                if (TimeDoChangeState >= WaitTime)
                {
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
                    TimeDoChangeState = 0;
                }
            }
            else
            {
                InAttackRange(procedureOwner);
            }

            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        }
        protected override void InAttackRange(ProcedureOwner fsm)
        {
            base.InAttackRange(fsm);

            if (m_ChanceDo <= m_BlockStatePerc && owner.find_Player.m_moveBehaviour.isAttack)
            {
                //闪避
                if (OnlyOnce)
                {
                    owner.m_Animator.SetTrigger(m_Appel);
                    int num = Utility.Random.GetRandom(0, 3);
                    owner.m_Animator.SetInteger(m_AppelState, num);
                    owner.IsRoll = true;
                    OnlyOnce = false;
                } 

            }
            else if (m_ChanceDo < m_RollStatePrec && owner.find_Player.m_moveBehaviour.isAttack)
            {
                if (OnlyOnce)
                {
                    int num = Utility.Random.GetRandom(0, 4);
                    //Log.Info("----------------"+num);
                    AvoidAttack(num);
                    OnlyOnce = false;
                }
            }

            //if (m_ChanceDo <= m_BlockStatePerc)
            //{
            //    ChangeState(fsm, owner.ChangeStateEnemy(EnemyStateType.Block));
            //}
        }
        public static WeregoatFightState Create()
        {
            WeregoatFightState state = ReferencePool.Acquire<WeregoatFightState>();
            return state;
        }

        private void FightAnimationEnd()
        {
            //Debug.Log("动画播放进度" + info.normalizedTime);
            ////判断动画是否播放完成
            if (info.normalizedTime >= 0.8f)
            {
                owner.IsRoll = false;
                
                owner.AnimationEnd();
            }

        }
        /// <summary>
        /// 翻滚
        /// </summary>
        /// <param name="num"></param>
        private void AvoidAttack(int num)
        {
            //Debug.Log("躲避躲避躲避！！！");
            switch (num)
            {
                case (int)AvoidType.back:
                    owner.m_Animator.SetTrigger(m_AvoidBack);
                    break;
                case (int)AvoidType.left:
                    owner.m_Animator.SetTrigger(m_BlockRollLeft);
                    break;
                case (int)AvoidType.right:
                    owner.m_Animator.SetTrigger(m_BlockRollRight);
                    break;
                case (int)AvoidType.front:
                    owner.m_Animator.SetTrigger(m_AvoidFront);
                    break;
            }
        }


    }
       
    
}
