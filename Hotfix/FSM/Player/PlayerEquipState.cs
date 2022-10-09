using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerEquipState : PlayerBaseActionState
    {
        private PlayerLogic owner;
        private float invincibleTime = 0.6f;
        private float invincibleFrame;

        private static readonly int _PlayerEquipState = Animator.StringToHash("PlayerEquipState");//当前装备的是那种武器1.剑盾 2.大剑 3.短刀 4.双剑 5.远程
        private static readonly int PlayerEquipStateTrigger = Animator.StringToHash("PlayerEquipStateTrigger");

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            if (owner.EquiState == EquiState.RevengerDoubleBlades)
            {
                owner.m_Animator.SetInteger(_PlayerEquipState, 4);
                
            }
            else
            {
                owner.m_Animator.SetInteger(_PlayerEquipState, (int)owner.EquiState);
            }
            owner.m_Animator.SetTrigger(PlayerEquipStateTrigger);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            invincibleFrame += Time.deltaTime;
            owner.m_Animator.ResetTrigger(PlayerEquipStateTrigger);
            if (invincibleFrame >= invincibleTime)
            {
                owner.isWeaponState = false;
                ChangeState<PlayerIdleState>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            invincibleFrame = 0;
            owner.isWeaponState = false;
        }

        public static PlayerEquipState Create()
        {
            PlayerEquipState state = ReferencePool.Acquire<PlayerEquipState>();
            return state;
        }

    }
}
