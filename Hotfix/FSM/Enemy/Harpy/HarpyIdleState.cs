using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    public class HarpyIdleState : EnemyIdleState
    {
        new private HarpyLogic owner;

        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner as HarpyLogic;
        }

        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            owner.WaitTimer += elapseSeconds;
        }

        public static new HarpyIdleState Create()
        {
            HarpyIdleState state = ReferencePool.Acquire<HarpyIdleState>();
            return state;
        }

        protected override void LockPlayerDo(IFsm<EnemyLogic> procedureOwner)
        {
            GameEntry.Sound.PlaySound(owner.enemyData.ShoutSoundId);
            base.LockPlayerDo(procedureOwner);
        }
    }
}


