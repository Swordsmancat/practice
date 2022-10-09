using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using System;

namespace Farm.Hotfix
{
    /// <summary>
    /// 敌人行动基类状态
    /// </summary>
   public class EnemyBaseActionState :EnemyBaseState
    {
        private EnemyLogic owner;
        private ProcedureOwner ProcedureOwner;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            owner = procedureOwner.Owner;
            ProcedureOwner = procedureOwner;
            if (!owner.InitEvent)
            {
                GameEntry.Event.Subscribe(ApplyDamageEventArgs.EventId, OnApplyDamageEvent);
                GameEntry.Event.Subscribe(ApplyBuffEventArgs.EventId, OnApplyBuffEvent);
                GameEntry.Event.Subscribe(ApplyDefenseEventArgs.EventId, OnApplyDefenseEvent);
                owner.InitEvent = true;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
        }

        private void OnApplyBuffEvent(object sender, GameEventArgs e)
        {
            ApplyBuffEventArgs ne = (ApplyBuffEventArgs)e;
            TargetBuff targetBuff = (TargetBuff)ne.UserData;
            if (targetBuff == null)
            {
                Log.Warning("buff目标为空");
                return;
            }
            if (targetBuff.Target != owner)
            {
                return;
            }
            owner.ApplyBuffEvent(targetBuff.Buff.BuffTypeEnum);
            //switch (targetBuff.Buff.BuffTypeEnum)
            //{
            //    case BuffType.None:
            //        break;
            //    case BuffType.Tap:
            //        //TODO:击飞
            //        ChangeState<EnemyKnockedFlyState>(ProcedureOwner);
            //        break;
            //    case BuffType.Thump:
            //        ChangeState<EnemyKnockedDownState>(ProcedureOwner);
            //        break;
            //    case BuffType.Overwhelmed:
            //        break;
            //    default:
            //        break;
            //}
        }
        /// <summary>
        /// 防御事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplyDefenseEvent(object sender, GameEventArgs e)
        {
            ApplyDefenseEventArgs ne = (ApplyDefenseEventArgs)e;
            if (ne.UserData != owner)
            {
                return;
            }

            //if (owner.toParry)
            //{
            //    
            //    owner.toParry = false;
            //}


            owner.underAttack = true;
            if (owner.IsDefense && owner.fsm.CurrentState.GetType() != owner.ChangeStateEnemy(EnemyStateType.Parry))
            {
                ChangeState(ProcedureOwner, owner.ChangeStateEnemy(EnemyStateType.Parry));                   
            }
                    

        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

        }


        protected override void OnDestroy(ProcedureOwner fsm)
        {
            base.OnDestroy(fsm);
            if (owner.InitEvent)
            {
                GameEntry.Event.Unsubscribe(ApplyDamageEventArgs.EventId, OnApplyDamageEvent);
                GameEntry.Event.Unsubscribe(ApplyBuffEventArgs.EventId, OnApplyBuffEvent);
                GameEntry.Event.Unsubscribe(ApplyDefenseEventArgs.EventId, OnApplyDefenseEvent);
                owner.InitEvent = false;
            }
        }


        private void OnApplyDamageEvent(object sender, GameEventArgs e)
        {
            ApplyDamageEventArgs ne = (ApplyDamageEventArgs)e;
            if (ne.UserData != owner)
            {
                return;
            }
            //Log.Info("Hurt");
            Entity attcker = (Entity)sender;
            owner.attackDir = AIUtility.GetAttackerDir(owner, attcker);
            if (owner.fsm.CurrentState.GetType() == owner.ChangeStateEnemy(EnemyStateType.Parry))
            {
                if (owner.isBehindAtked)
                {
                    return;
                }
                else
                { 
                    ChangeState(ProcedureOwner, owner.ChangeStateEnemy(EnemyStateType.Hurt)); 
                }
            }
            else
            { 
                ChangeState(ProcedureOwner, owner.ChangeStateEnemy(EnemyStateType.Hurt));
            }

        }



    }
}
