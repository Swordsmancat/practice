using System.Collections;
using System.Collections.Generic;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using UnityEngine;
using GameFramework;
namespace Farm.Hotfix
{
    public class GoatAttackState : EnemyAttackState
    {
        private static readonly int attack = Animator.StringToHash("Attack");
        AnimatorStateInfo info;
        public static new GoatAttackState Create()
        {
            GoatAttackState state = ReferencePool.Acquire<GoatAttackState>();
            return state;
        }
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            
            
            base.OnEnter(procedureOwner);
            owner.SetRichAiStop();
            owner.m_Animator.SetTrigger(attack);
            
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            owner.SetRichAiStop();
            info = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
            AttackAnimationEnd();
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }
            private void AttackAnimationEnd()
        {
            //Debug.Log("动画播放进度" + info.IsName("Attack") + info.normalizedTime);
            ////判断动画是否播放完成
            if (info.IsName("Attack") && info.normalizedTime >= 0.75f)
            {
                owner.AnimationEnd();
                owner.IsCanAttack = false;
            }

        }
    }
}
