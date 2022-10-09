using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Fsm;

namespace Farm.Hotfix
{
    public class EnemyBlockState : EnemyBaseActionState
    {
        private EnemyLogic owner;

        protected override void OnInit(IFsm<EnemyLogic> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(IFsm<EnemyLogic> fsm)
        {
            base.OnEnter(fsm);
            owner = fsm.Owner;
        }

        protected override void OnUpdate(IFsm<EnemyLogic> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

    }
}

