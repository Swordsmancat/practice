using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    /// <summary>
    /// ºÚÁúÏÐÖÃ×´Ì¬
    /// </summary>
    public class DragonIdleState : EnemyIdleState
    {
        public static new DragonIdleState Create()
        {
            DragonIdleState state = ReferencePool.Acquire<DragonIdleState>();
            return state;
        }

        protected override void LockPlayerDo(IFsm<EnemyLogic> procedureOwner)
        {
            owner.LockEntity(owner.find_Player);
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Shout));
        }
    }
}

