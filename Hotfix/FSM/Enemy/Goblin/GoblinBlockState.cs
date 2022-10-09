using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Fsm;

namespace Farm.Hotfix
{
    public class GoblinBlockState : EnemyBlockState
    {
        private readonly static int Block = Animator.StringToHash("Block");
        private readonly static int BlockWalkState = Animator.StringToHash("BlockWalkState");
        private readonly static float ExitTime = 4f;

        private GoblinLogic owner;
        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner as GoblinLogic;
            owner.SetRichAiStop();
            owner.m_Animator.SetBool(Block, true);
            owner.m_Animator.SetInteger(BlockWalkState, Utility.Random.GetRandom(0, 2));
            owner.IsBlock = true;
        }

        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            /*AIUtility.RotateToTarget(owner.LockingEntity, owner, -15f, 15f)*/;

            owner.BlockTime += elapseSeconds;
            if(owner.BlockTime >= ExitTime)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
            }
             if (owner.enemyData.HPRatio <= 0.2)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Parry));
            }
        }

        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.IsAnimPlayed = false;
            owner.m_Animator.SetBool(Block, false);
            owner.m_Animator.SetInteger(BlockWalkState, -1);
        }

        public static GoblinBlockState Create()
        {
            GoblinBlockState state = ReferencePool.Acquire<GoblinBlockState>();
            return state;
        }
    }
}

