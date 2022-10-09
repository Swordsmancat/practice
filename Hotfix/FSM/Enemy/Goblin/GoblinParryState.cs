using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Fsm;

namespace Farm.Hotfix
{
    public class GoblinParryState : EnemyParryState
    {
        public static GoblinParryState Create()
        {
            GoblinParryState state = ReferencePool.Acquire<GoblinParryState>();
            return state;
        }

        protected override void EnemyParryStateStart(EnemyLogic owner)
        {
            base.EnemyParryStateStart(owner);
            GameEntry.Sound.PlaySound(10007);//≤‚ ‘“Ù∆µ
        }

        protected override void EnemyParryStateEnd(IFsm<EnemyLogic> procedureOwner)
        {
            base.EnemyParryStateEnd(procedureOwner);
        }
    }

}
