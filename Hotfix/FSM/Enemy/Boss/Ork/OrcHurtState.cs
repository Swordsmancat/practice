using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;

namespace Farm.Hotfix
{
    public class OrcHurtState : EnemyHurtState
    {
        private static readonly int CanAvoid = Animator.StringToHash("CanAvoid");

        public static new OrcHurtState Create()
        {
            OrcHurtState state = ReferencePool.Acquire<OrcHurtState>();
            return state;
        }

        protected override void EnemyHurtStateStart(ProcedureOwner fsm)
        {
          //  Debug.Log("进入遭受攻击方法");
          //  Debug.Log(owner.find_Player.isAttack);
           // Debug.Log(owner.find_Player.isThump);
            if (owner.find_Player.isThump)
            {
                owner.m_Animator.SetTrigger(HurtThump);
            }
            else
            {
                owner.m_Animator.SetTrigger(Hurt);
            }
            owner.m_Animator.SetBool(CanAvoid, false);
        }

        protected override void EnemyHurtStateEnd(IFsm<EnemyLogic> procedureOwner)
        {
            base.EnemyHurtStateEnd(procedureOwner);
            owner.m_Animator.SetBool(CanAvoid, true);
        }
    }
}

