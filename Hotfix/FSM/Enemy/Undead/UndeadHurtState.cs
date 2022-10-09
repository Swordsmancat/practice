using Farm.Hotfix;
using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;

namespace Farm.Hotfix
{
    public class UndeadHurtState : EnemyHurtState
    {
        private static readonly int CanAvoid = Animator.StringToHash("CanAvoid");
        private static readonly int m_AvoidBack = Animator.StringToHash("Avoidback");
        private static readonly int m_Avoidleft = Animator.StringToHash("Avoidleft");
        private static readonly int m_AvoidRight = Animator.StringToHash("AvoidRight");
        private static readonly int BlockHurt = Animator.StringToHash("BlockHit");
        
        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner as UndeadLogic;
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
        protected enum AvoidType
        {
            back,
            left,
            right
        }
        public static new UndeadHurtState Create()
        {
            UndeadHurtState state = ReferencePool.Acquire<UndeadHurtState>();
            return state;
        }
        // Start is called before the first frame update
        protected override void EnemyHurtStateStart(ProcedureOwner fsm)
        {
            if (owner.IsBlock)
            {
                owner.m_Animator.SetTrigger(BlockHurt);
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
            base.EnemyHurtStateEnd(procedureOwner);// ‹…À∂Øª≠Ω· ¯
            owner.m_Animator.SetBool(CanAvoid, true);
            int num = Utility.Random.GetRandom(0, 3);
            AvoidAttack(num);
            if (owner.IsBlock)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Block));
            }
           
            else
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
            }
            
        }
        private void AvoidAttack(int num)
        {

            Debug.Log("∂„±‹∂„±‹∂„±‹£°£°£°");
            switch (num)
            {
                case (int)AvoidType.back:
                    owner.m_Animator.SetTrigger(m_AvoidBack);
                    break;
                case (int)AvoidType.left:
                    owner.m_Animator.SetTrigger(m_Avoidleft);
                    owner.IsBlock = true;
                    break;
                case (int)AvoidType.right:
                    owner.m_Animator.SetTrigger(m_AvoidRight);
                    owner.IsBlock = true;
                    break;
            }
        }
    }
}