using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

namespace Farm.Hotfix
{
    /// <summary>
    /// ���˺��״̬
    /// Ҳ�ɿ����ǵ�һ���������(�������)״̬
    /// EnemyShoutState ���ᴥ�����˶���
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

        //���´��뷽��̳�����д
        #region virutal function

        /// <summary>
        /// ������״̬
        /// ��д�÷���(��ʹ��base.EnterShoutState())��Ӱ�����(���紴�������¼�)
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected virtual void EnterShoutState(ProcedureOwner procedureOwner)
        {
            owner = procedureOwner.Owner;
            owner.m_Animator.SetTrigger(m_Shout);
            //Debug.Log("���Ŷ���");
            owner.m_Animator.SetBool(m_ShoutEnd, false);
            System.Random rand = new System.Random();
            int result = shout[rand.Next(shout.Length)];
            
            //������Ƶ
            //GameEntry.Sound.PlaySound(randomNum);
        }

        /// <summary>
        /// Update
        /// ��д�÷�����Ӱ�����(���繥����ʱ)
        /// </summary>
        /// <param name="procedureOwner"></param>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        protected virtual void UpdateShoutState(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
           // Debug.Log("��н�����" + owner.IsAnimPlayed);
            if (owner.IsAnimPlayed)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            }
        }

        /// <summary>
        /// �뿪���״̬
        /// ��д�÷�����Ӱ�����(�������������¼�)
        /// </summary>
        protected virtual void LeaveShoutState()
        {
            owner.m_Animator.SetBool(m_ShoutEnd, true);
            owner.IsAnimPlayed = false;
        }

        #endregion
    }
}

