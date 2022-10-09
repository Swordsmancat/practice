using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerEquipWeaponState : PlayerBaseActionState
    {
        private PlayerLogic owner;
        private float invincibleTime = 0.2f;
        private float invincibleFrame;

        private static readonly int TakeOutWeaponTrigger = Animator.StringToHash("TakeOutWeaponTrigger");

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;

            owner.m_Animator.SetTrigger(TakeOutWeaponTrigger);
            //owner.m_Animator.SetTrigger(PutOrTakeTrigger);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            invincibleFrame += Time.deltaTime;
            if (invincibleFrame >= invincibleTime)
            {
                owner.TakeOutWeaponWhenAtk();
                ChangeState<PlayerAttackState>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            invincibleFrame = 0;
        }

        public static PlayerEquipWeaponState Create()
        {
            PlayerEquipWeaponState state = ReferencePool.Acquire<PlayerEquipWeaponState>();
            return state;
        }

    }
}
