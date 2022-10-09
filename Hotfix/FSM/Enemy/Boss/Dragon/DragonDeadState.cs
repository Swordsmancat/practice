using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{

    /// <summary>
    /// ºÚÁúËÀÍö×´Ì¬
    /// </summary>
    public class DragonDeadState : EnemyDeadState
    {
        private readonly static int s_flying = Animator.StringToHash("Flying");
        private readonly static int s_deadTrigger = Animator.StringToHash("DeadTrigger");
        protected override void OnEnter(IFsm<EnemyLogic> fsm)
        {
            base.OnEnter(fsm);
            Debug.Log("½øÈëºÚÁúËÀÍö×´Ì¬");
            owner.m_Animator.SetTrigger(s_deadTrigger);
        }

        public static new DragonDeadState Create()
        {
            DragonDeadState state = ReferencePool.Acquire<DragonDeadState>();
            return state;
        }

    }

}

