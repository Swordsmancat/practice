using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    public class HarpyRotateState : EnemyRotateState
    {
        protected override void EnemyRotateStateStart(EnemyLogic owner)
        {
            base.EnemyRotateStateStart(owner);
            RotateSpeed = 5;
        }

        public new static HarpyRotateState Create()
        {
            HarpyRotateState state = ReferencePool.Acquire<HarpyRotateState>();
            return state;
        }
    }
}

