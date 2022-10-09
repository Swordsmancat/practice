using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    public class DragonRotateState : EnemyRotateState
    {
        protected override void EnemyRotateStateStart(EnemyLogic owner)
        {
            base.EnemyRotateStateStart(owner);
            RotateSpeed = 1;
        }

        public static new DragonRotateState Create()
        {
            DragonRotateState state = ReferencePool.Acquire<DragonRotateState>();
            return state;
        }
    }
}

