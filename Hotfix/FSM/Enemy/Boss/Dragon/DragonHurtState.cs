using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix 
{
    /// <summary>
    /// 黑龙受伤状态.
    /// </summary>
    public class DragonHurtState : EnemyHurtState
    {
        private readonly static int s_hurtState = Animator.StringToHash("HurtState");
        private new DragonLogic owner;

        protected override void EnemyHurtStateStart(ProcedureOwner fsm)
        {
            owner = fsm.Owner as DragonLogic;
            owner.SetRichAiStop();
            owner.EnemyAttackEnd();
            owner.m_Animator.SetTrigger(Hurt);
            owner.m_Animator.SetInteger(s_hurtState, (int)owner.HurtState);
            Debug.Log("进入黑龙受伤状态"+owner.HurtState);
        }

        protected override void EnemyHurtStateEnd(ProcedureOwner procedureOwner)
        {
            Debug.Log("退出黑龙受伤状态");
            if (owner.IsLocking)
            {
                Debug.Log("改变到移动状态");
                owner.m_Animator.SetInteger(s_hurtState, -1);
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
            }
            else
            {
                Debug.Log("改变到吼叫状态");
                owner.m_Animator.SetInteger(s_hurtState, -1);
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Shout));
            }
        }

        public static new DragonHurtState Create()
        {
            DragonHurtState state = ReferencePool.Acquire<DragonHurtState>();
            return state;
        }

    }
}


