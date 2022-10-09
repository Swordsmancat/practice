using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using System;

namespace Farm.Hotfix
{
    public class PlayerBaseActionState : PlayerBaseState
    {
        private PlayerLogic owner;
        private ProcedureOwner ProcedureOwner;
        private static readonly int Explosion = Animator.StringToHash("Explosion");
        private static readonly int TwoSwordsBuff = Animator.StringToHash("TwoSwordsBuff");
        

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


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (owner.m_Explosion)
            {
                owner.m_Animator.SetBool(Explosion, true);
            }
            else
            {
                owner.m_Animator.SetBool(Explosion, false);
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

        }


        protected override void OnDestroy(ProcedureOwner fsm)
        {
            base.OnDestroy(fsm);
            if (!owner.InitEvent)
            {
                GameEntry.Event.Unsubscribe(ApplyDamageEventArgs.EventId, OnApplyDamageEvent);
                GameEntry.Event.Unsubscribe(ApplyBuffEventArgs.EventId, OnApplyBuffEvent);
                GameEntry.Event.Unsubscribe(ApplyDefenseEventArgs.EventId, OnApplyDefenseEvent);
                owner.InitEvent = false;
            }
            //  GameEntry.Event.Unsubscribe(ApplyDamageEventArgs.EventId, OnApplyDamageEvent);
        }

        private  void OnApplyBuffEvent(object sender, GameEventArgs e)
        {
            ApplyBuffEventArgs ne = (ApplyBuffEventArgs)e;
            TargetBuff targetBuff = (TargetBuff)ne.UserData;
            if(targetBuff == null)
            {
                Log.Warning("buff目标为空");
                return;
            }
            if (targetBuff.Target != owner)
            {
                return;
            }
            owner.m_BuffType = targetBuff.Buff.BuffTypeEnum;
          
        }

        private void OnApplyDefenseEvent(object sender ,GameEventArgs e)
        {
            ApplyDefenseEventArgs ne = (ApplyDefenseEventArgs)e;
            if(ne.UserData != owner)
            {
                return;
            }

        }


        private void OnApplyDamageEvent(object sender, GameEventArgs e)
        {
            ApplyDamageEventArgs ne = (ApplyDamageEventArgs)e;
            //Debug.Log("格x");

            if ((object)ne.UserData != owner)
            {
                return;
            } 
            if (owner.IsDefense)
            {
                //Debug.Log("格挡");
                //owner.m_Animator.SetTrigger(HurtDefense);
                return;
            }
            Entity attcker = (Entity)sender;
            owner.attackDir = AIUtility.GetAttackerDir(owner, attcker);
            if (owner.IsCanBreak)
            {
                ChangeState<PlayerHurtState>(ProcedureOwner);
            }
           
        }


    }
}
