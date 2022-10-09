using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

namespace Farm.Hotfix
{
    public class HarpyAttackState : EnemyAttackState
    {
        public static new HarpyAttackState Create()
        {
            HarpyAttackState state = ReferencePool.Acquire<HarpyAttackState>();
            return state;
        }

        protected override void NormalAttack()
        {
            int randomNum = Utility.Random.GetRandom(0, 4);
            owner.m_Animator.SetInteger(AttackState, randomNum);
        }

        protected override void EnemyAttackStateStart(EnemyLogic owner)
        {
            base.EnemyAttackStateStart(owner);
            NormalAttack();
        }



    }

}

