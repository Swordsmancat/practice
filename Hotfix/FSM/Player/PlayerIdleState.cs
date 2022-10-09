using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerIdleState : PlayerBaseActionState
    {
        private PlayerLogic owner;

        private static readonly int Defense = Animator.StringToHash("Defense");

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;

            //owner.m_Animator.SetFloat(MoveBlend, 0f);
            //Log.Info("基础状态");
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (owner.MoveX!=default || owner.MoveY != default)
            {
                ChangeState<PlayerMotionState>(procedureOwner);
                return;
            }
            owner.RestoreEnergy();
            if (owner.m_Attack)
            {
                if (!owner.IsHands)
                {
                    //owner.TakeOutWeaponWhenAtk();
                    ChangeState<PlayerEquipWeaponState>(procedureOwner);
                    return;
                }
                ChangeState<PlayerAttackState>(procedureOwner);
                return;
            }
            if (owner.isFourthAtk && owner.IsHands && !owner.isSecondSkill)
            {
                ChangeState<PlayerSkillState>(procedureOwner);
            }
            if (owner.isDodge)
            {
                ChangeState<PlayerDodgeState>(procedureOwner);
                return;
            }
            if (owner.IsDefense && owner.IsHands)
            {
                //owner.m_Animator.SetBool(Defense, true);
                ChangeState<PlayerDefenseState>(procedureOwner);
            }
            else
            {
                owner.m_Animator.SetBool(Defense, false);
            }
            if (owner.isWeaponState)
            {
                ChangeState<PlayerEquipState>(procedureOwner);
            }
            if (owner.isStep)
            {
                ChangeState<PlayerStepState>(procedureOwner);
                return;
            }
            if (owner.isFocusEngy)
            {
                ChangeState<PlayerFocusEnergyState>(procedureOwner);
            }

        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }


        public static PlayerIdleState Create()
        {
            PlayerIdleState state = ReferencePool.Acquire<PlayerIdleState>();
            return state;
        }

        public new void Clear()
        {
            owner = null;
        }
    }
}
