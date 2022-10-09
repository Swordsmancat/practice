using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    /// <summary>
    /// 黑龙攻击状态
    /// </summary>
    public class DragonAttackState : EnemyAttackState
    {
        private readonly static int s_moveBlend = Animator.StringToHash("MoveBlend");   //地面移动混合树
        private readonly static int s_flyMoveBlend = Animator.StringToHash("FlyMoveBlend");  //空中移动混合树
        private readonly static int s_landAttackState = Animator.StringToHash("Land_Attack_State"); //地面攻击状态
        private readonly static int s_fly_Attack_State = Animator.StringToHash("Fly_Attack_State"); //空中攻击状态
        private readonly static int s_after_Skill_Attack_State = Animator.StringToHash("After_Skill_Attack_State"); //使用技能之后的攻击状态
        private DragonLogic _owner;

        protected override void EnemyAttackStateStart(EnemyLogic owner)
        {
            base.EnemyAttackStateStart(owner);
            _owner = owner as DragonLogic;
            NormalAttack();
        }

        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            _owner.AttackState = -1;
        }
        protected override void StopBlend(EnemyLogic owner)
        {
            owner.m_Animator.SetFloat(s_moveBlend, 0);
            owner.m_Animator.SetFloat(s_flyMoveBlend, 0);
        }

        protected override void NormalAttack()
        {
            owner.m_Animator.SetInteger(s_landAttackState, _owner.AttackState);
        }

        protected override void ResetAttackState()
        {
            owner.m_Animator.SetInteger(s_landAttackState, DefaultState);
            owner.m_Animator.SetInteger(s_fly_Attack_State, DefaultState);
            owner.m_Animator.SetInteger(s_after_Skill_Attack_State, DefaultState);
        }

        public static new DragonAttackState Create()
        {
            DragonAttackState state = ReferencePool.Acquire<DragonAttackState>();
            return state;
        }
    }

}

