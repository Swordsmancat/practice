using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

namespace Farm.Hotfix
{
    /// <summary>
    /// 敌人吼叫状态
    /// 也可看作是第一次碰见玩家(锁定玩家)状态
    /// EnemyShoutState 不会触发受伤动画
    /// </summary>
    public class EnemyShoutState : EnemyBaseState
    {
        private readonly static int m_Shout = Animator.StringToHash("Shout");
        private readonly static int m_ShoutEnd = Animator.StringToHash("ShoutEnd");
        private EnemyLogic owner;
        int[] shout = { 20031, 20035, 20036 };
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            EnterShoutState(procedureOwner);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            UpdateShoutState(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            LeaveShoutState();
        }

        public static EnemyShoutState Create()
        {
            EnemyShoutState state = ReferencePool.Acquire<EnemyShoutState>();
            return state;
        }

        //以下代码方便继承类重写
        #region virutal function

        /// <summary>
        /// 进入吼叫状态
        /// 重写该方法(不使用base.EnterShoutState())不影响基类(例如创建受伤事件)
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected virtual void EnterShoutState(ProcedureOwner procedureOwner)
        {
            owner = procedureOwner.Owner;
            owner.m_Animator.SetTrigger(m_Shout);
            //Debug.Log("播放动画");
            owner.m_Animator.SetBool(m_ShoutEnd, false);
            System.Random rand = new System.Random();
            int result = shout[rand.Next(shout.Length)];
            
            //测试音频
            //GameEntry.Sound.PlaySound(randomNum);
        }

        /// <summary>
        /// Update
        /// 重写该方法不影响基类(例如攻击记时)
        /// </summary>
        /// <param name="procedureOwner"></param>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        protected virtual void UpdateShoutState(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
           // Debug.Log("吼叫进行中" + owner.IsAnimPlayed);
            if (owner.IsAnimPlayed)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            }
        }

        /// <summary>
        /// 离开吼叫状态
        /// 重写该方法不影响基类(例如销毁受伤事件)
        /// </summary>
        protected virtual void LeaveShoutState()
        {
            owner.m_Animator.SetBool(m_ShoutEnd, true);
            owner.IsAnimPlayed = false;
        }

        #endregion
    }
}

