using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;

namespace Farm.Hotfix
{
    public class GoblinHurtState : EnemyHurtState
    {
        private static readonly int BlockHurt = Animator.StringToHash("BlockHurt");
        private static readonly int CanAvoid = Animator.StringToHash("CanAvoid");

        public static new GoblinHurtState Create()
        {
            GoblinHurtState state = ReferencePool.Acquire<GoblinHurtState>();
            return state;
        }
        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner as GoblinLogic;
            owner.SetRichAiStop();
            owner.IsBlock = true;
        }
        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.IsAnimPlayed = false;
            owner.IsBlock = false;
            owner.IsHurt = false;

        }
        protected override void EnemyHurtStateStart(ProcedureOwner fsm)
        {
            if (owner.IsBlock)
            {
                owner.m_Animator.SetTrigger(BlockHurt);
                GameEntry.Sound.PlaySound(10007);
                GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), owner.LeftHand.weaponData.WeaponHitEffectId),
                    typeof(EnemyBlockEffectLogic));
            }
            else
            {
                if (owner.find_Player.isThump)
                {
                    owner.m_Animator.SetTrigger(HurtThump);
                }
                else
                {
                    owner.m_Animator.SetTrigger(Hurt);
                }
            }
            owner.m_Animator.SetBool(CanAvoid, false);
        }

        protected override void EnemyHurtStateEnd(IFsm<EnemyLogic> procedureOwner)
        {
            if (owner.IsBlock)
            {
                owner.m_Animator.SetBool(CanAvoid, true);
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Block));
            }
            else
            {
                owner.m_Animator.SetBool(CanAvoid, true);
                owner.LockEntity(owner.find_Player);
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
            }
           
        }

    }
}

