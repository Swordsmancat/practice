using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;

namespace Farm.Hotfix
{
    public class DK_GoblinHurtState : EnemyHurtState
    {
        private static readonly int CanAvoid = Animator.StringToHash("CanAvoid");
        private static readonly int m_AvoidBack = Animator.StringToHash("Avoidback");
        private static readonly int m_Avoidfront = Animator.StringToHash("AvoidFront");
        private static readonly int m_Avoidleft = Animator.StringToHash("Avoidleft");
        private static readonly int m_AvoidRight = Animator.StringToHash("AvoidRight");
        int[] sound = { 20037, 20038 };
        // Start is called before the first frame update
        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner as Dk_GoblinLogic;
            owner.SetRichAiStop();

        }
        protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            owner.EnemyAttackEnd();
            if (owner.IsAnimPlayed)
            {
                owner.m_Animator.SetBool(CanAvoid, false);

            }
        }
        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.IsAnimPlayed = false;
            owner.IsHurt = false;

        }
        protected enum AvoidType
        {
            back,
            left,
            right,
            front
        }
        public static new DK_GoblinHurtState Create()
        {
            DK_GoblinHurtState state = ReferencePool.Acquire<DK_GoblinHurtState>();
            return state;
        }
        protected override void EnemyHurtStateStart(ProcedureOwner fsm)
        {
            if (owner.find_Player.isThump)
            {
                owner.m_Animator.SetTrigger(HurtThump);
            }
            else
            {
                owner.m_Animator.SetTrigger(Hurt);
                System.Random rand = new System.Random();
                int result = sound[rand.Next(sound.Length)];

                //≤‚ ‘“Ù∆µ
                GameEntry.Sound.PlaySound(result);
            }

            owner.m_Animator.SetBool(CanAvoid, false);
        }
        protected override void EnemyHurtStateEnd(IFsm<EnemyLogic> procedureOwner)
        {
            owner.m_Animator.SetBool(CanAvoid, true);
            base.EnemyHurtStateEnd(procedureOwner);
            owner.LockEntity(owner.find_Player);

            int num = Utility.Random.GetRandom(0, 4);
            AvoidAttack(num);

        }
        
        private void AvoidAttack(int num)
        {
            Debug.Log("∂„±‹∂„±‹∂„±‹£°£°£°");// ‹…À∫Û∂„±‹
            switch (num)
            {
                case (int)AvoidType.back:
                    owner.m_Animator.SetTrigger(m_AvoidBack);
                    break;
                case (int)AvoidType.front:
                    owner.m_Animator.SetTrigger(m_Avoidfront);
                    break;
                case (int)AvoidType.left:
                    owner.m_Animator.SetTrigger(m_Avoidleft);
                    break;
                case (int)AvoidType.right:
                    owner.m_Animator.SetTrigger(m_AvoidRight);
                    break;
            }
          
        }
    }
}