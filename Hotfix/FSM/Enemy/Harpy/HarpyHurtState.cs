using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;


namespace Farm.Hotfix
{
    public class HarpyHurtState : EnemyHurtState
    {
        private readonly static int GroundHurt = Animator.StringToHash("GroundHurt");
        private readonly static int HurtInSky = Animator.StringToHash("HurtInSky");
        private readonly static int Flying = Animator.StringToHash("Flying");

        public static new HarpyHurtState Create()
        {
            HarpyHurtState state = ReferencePool.Acquire<HarpyHurtState>();
            return state;
        }

        protected override void EnemyHurtStateStart(ProcedureOwner fsm)
        {
            if(owner.m_Animator.GetBool(Flying))
            {
                owner.m_Animator.SetTrigger(HurtInSky);
            }
            else
            {
                owner.m_Animator.SetTrigger(GroundHurt);
            }

        }
    }

}

