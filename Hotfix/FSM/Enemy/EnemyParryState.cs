using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace Farm.Hotfix
{
    public class EnemyParryState : EnemyBaseActionState
    {
        private readonly static int m_Parry = Animator.StringToHash("Parry");
        //private readonly static float ExitTime = 0.3f;
        private EnemyLogic owner;
        //private float exitTimer = 0;

        protected override void OnInit(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            EnemyParryStateStart(owner);
        }
        public static EnemyParryState Create()
        {
            EnemyParryState state = ReferencePool.Acquire<EnemyParryState>();
            return state;
        }

        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            //exitTimer += elapseSeconds;
            if(owner.IsAnimPlayed)
            {
                owner.EnemyAttackEnd();
                EnemyParryStateEnd(procedureOwner);
            }
        }

        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.IsParry = false;
            owner.IsAnimPlayed = false;
            //exitTimer = 0;
        }

        /// <summary>
        /// ÕÐ¼Ü×´Ì¬¿ªÊ¼
        /// </summary>
        protected virtual void EnemyParryStateStart(EnemyLogic owner)
        {
            owner.m_Animator.SetTrigger(m_Parry);
            owner.EnemyAttackEnd();
        }

        /// <summary>
        /// ÕÐ¼Ü×´Ì¬½áÊø
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected virtual void EnemyParryStateEnd(IFsm<EnemyLogic> procedureOwner)
        {
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
        }
        /// <summary>
        /// ÕÐ¼Ü·´»÷ÍË³ö
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected virtual void EnemyParryStateCounter(IFsm<EnemyLogic> procedureOwner)
        {
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Attack));
        }

        /// <summary>
        /// ÕÐ¼ÜÆÆ·ÀÍË³ö
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected virtual void EnemyParryOutStateEnd(IFsm<EnemyLogic> procedureOwner)
        {
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
        }

    }
}

