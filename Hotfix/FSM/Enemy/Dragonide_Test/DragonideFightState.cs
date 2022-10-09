using GameFramework;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{

    public class DragonideFightState : EnemyFightState
    {
        private readonly static int m_BlockRollLeft = Animator.StringToHash("BlockRollLeft");
        private readonly static int m_BlockRollRight = Animator.StringToHash("BlockRollRight");
        private readonly static int m_AvoidBack = Animator.StringToHash("AvoidBack");
        private readonly static int m_AvoidFront = Animator.StringToHash("AvoidFront");
        //private readonly static int m_isback = Animator.StringToHash("IsBack");
        //private readonly static int m_isleft = Animator.StringToHash("IsLeft");
        //private readonly static int m_isright = Animator.StringToHash("IsRight");
        private readonly static int m_WalkSpeed = Animator.StringToHash("WalkSpeed");
        private readonly static int m_InFight = Animator.StringToHash("InFight");
        private readonly static int m_FightBlend = Animator.StringToHash("FightBlend");
        private readonly static int m_isback = Animator.StringToHash("IsBack");
        private float WaitTime = 0.3f;  //等待时间
        private float TimeDoChangeState = 0;
        private float SECOND_PHASE = 0.5f;
        AnimatorStateInfo info;

        private readonly static int m_BlockStatePerc = 10;
        private readonly static int m_RollStatePrec = 30;
        private bool OnlyOnce;
        private int m_ChanceDo;
        private DragonideLogic owner;



        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner as DragonideLogic;
            //owner.BlockTime = 0;
            //owner.IsBlock = false;
            OnlyOnce = true;
            owner.HideTrail();//角色倒地 关闭拖尾和攻击检测 防止怪物碰到角色武器会一直受伤
            owner.EnemyAttackEnd();//同上
            m_ChanceDo = Utility.Random.GetRandom(0, 100);
            if (m_ChanceDo % 13 == 0)
                GameEntry.Sound.PlaySound(26026);
            else if (m_ChanceDo % 7 == 0)
                GameEntry.Sound.PlaySound(26027);
            else if (m_ChanceDo % 11 == 0)
                GameEntry.Sound.PlaySound(26028);
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            info = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
            FightAnimationEnd();
            float disdance = AIUtility.GetDistance(owner, owner.find_Player);
            owner.RestoreEnergy();
            if (owner.LockingEntity != null)
            {
                //float disdance = AIUtility.GetDistance(owner, owner.LockingEntity);
                IsBack(disdance);
            }
            if (!owner.CheckInAttackRange(disdance))
            {
                if (owner.enemyData.HPRatio <= SECOND_PHASE)
                {
                    WaitTime = 0.1f;
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
                
                if (owner.find_Player.m_Attack)
                {
                    InAttackRange(procedureOwner);
                }
                else if (TimeDoChangeState >= WaitTime)
                {
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Attack));
                    TimeDoChangeState = 0;
                }

            }

            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            
        }
        protected override void InAttackRange(ProcedureOwner fsm)
        {
            base.InAttackRange(fsm);

            if (m_ChanceDo >= m_RollStatePrec && owner.find_Player.m_moveBehaviour.isAttack)
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

        private void FightAnimationEnd()
        {
            //Debug.Log("动画播放进度" + info.normalizedTime);
            ////判断动画是否播放完成
            if (info.normalizedTime >= 0.6f)
            {
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
        /// <summary>
        /// 太近后退
        /// </summary>
        /// <param name="disdance"></param>
        private void IsBack(float disdance)
        {
            //owner.m_Animator.SetBool(m_InFight, true);
            //目标方位
            Vector3 target = owner.find_Player.transform.position - owner.transform.position;
            Vector3 obj = owner.transform.forward;
            //bool left = (Vector3.Cross(target, obj).y > 0);
            //Debug.Log("方位向量" + Vector3.Cross(target, obj).y);
            // Debug.Log("距离+++++" + disdance);

            if (disdance <= 2.0)
            {
                //owner.m_Animator.SetFloat(m_FightBlend, 0,0.5f, 2 * Time.deltaTime);
                owner.m_Animator.SetBool(m_isback, true);

            }
            else 
            { owner.m_Animator.SetBool(m_isback, false); }

            //else if (disdance > 2.5 && disdance < 5 && left)
            //{
            //    //owner.m_Animator.SetFloat(m_FightBlend, 0.5f);
            //    owner.m_Animator.SetFloat(m_FightBlend, 1f, 0.5f, 2 * Time.deltaTime);

            //}
            //else if (disdance > 2.5 && disdance < 5 && !left)
            //{
            //    //owner.m_Animator.SetFloat(m_FightBlend, 1.5f);
            //    owner.m_Animator.SetFloat(m_FightBlend, 2f, 0.5f, 2 * Time.deltaTime);

            //}
            //else if (disdance >= 5)
            //{
            //    //owner.m_Animator.SetFloat(m_FightBlend, 3f);
            //    owner.m_Animator.SetFloat(m_FightBlend, 3, 0.5f, 2 * Time.deltaTime);
            //}
        }
        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            owner.m_Animator.SetBool(m_isback, false);
            base.OnLeave(fsm, isShutdown);
        }


        public static DragonideFightState Create()
        {
            DragonideFightState state = ReferencePool.Acquire<DragonideFightState>();
            return state;
        }
    }
}
