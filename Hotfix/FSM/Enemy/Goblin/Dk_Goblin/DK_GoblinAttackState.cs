using Farm.Hotfix;
using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using System;

namespace Farm.Hotfix
{

    public class DK_GoblinAttackState : EnemyAttackState
    {
     
        private readonly float PushDistance = 1f;//推进的距离
        int[] sound = { 20039, 20040,20041 };
        private static readonly int BackAir = Animator.StringToHash("backAir");
        private readonly static int m_NormalBlend = Animator.StringToHash("NormalBlend");
        public static new DK_GoblinAttackState Create()
        {
            DK_GoblinAttackState state = ReferencePool.Acquire<DK_GoblinAttackState>();
            return state;
        }
        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            owner.IsAnimPlayed = false;
            ResetAttackState();
        }
        protected override void ResetAttackState()
        {
            owner.m_Animator.SetFloat(BackAir, -1.0f);
            owner.m_Animator.SetInteger(AttackState, -1);
            owner.m_Animator.SetInteger(SkillState, -1);
        }
        protected override void EnemyAttackStateStart(EnemyLogic owner)
        {
            
            owner.SetRichAiStop();
            StopBlend(owner);
            int num = Utility.Random.GetRandom(0, 100);

            //不同阶段不同攻击方式

            if (num <= 70 && num >= 30)
            {
                SkillAttack();
            }
            else if (num <= 100 && num > 70)
            {
                NormalAttack();

            }else if (num < 30 && num >= 20)
            {
                int randomNum1 = Utility.Random.GetRandom(4, 6);
                owner.m_Animator.SetInteger(AttackState, randomNum1);
            }
            else
            {
                Vector3 newPosition = this.owner.transform.position;
                newPosition.z += owner.m_Animator.GetFloat("backAir") * Time.deltaTime;
                this.owner.transform.position = newPosition;
                owner.m_Animator.SetFloat(BackAir, 1.0f);
                int randomNum1 = Utility.Random.GetRandom(6, 8);
                owner.m_Animator.SetInteger(AttackState, randomNum1);
            }
        }
        protected override void NormalAttack()
        {
            int randomNum = Utility.Random.GetRandom(0, 4);
            if (randomNum == 5)
            {
                GameEntry.Sound.PlaySound(20033);
            }
            else
            {
                System.Random rand = new System.Random();
                int result = sound[rand.Next(sound.Length)];
                //测试音频
                GameEntry.Sound.PlaySound(result);
            }
            owner.m_Animator.SetInteger(AttackState, randomNum);
        }
        protected override void SkillAttack()
        {
            int randomNum = Utility.Random.GetRandom(0, 9);
            if (randomNum == 2||randomNum==6)
            {
                GameEntry.Sound.PlaySound(20033);
            }else if (randomNum == 1)
            {
                GameEntry.Sound.PlaySound(20034);
            }
            else
            {
                System.Random rand = new System.Random();
                int result = sound[rand.Next(sound.Length)];
                //测试音频
                GameEntry.Sound.PlaySound(result);
            }
            owner.m_Animator.SetInteger(SkillState, randomNum);
        }
        protected override void StopBlend(EnemyLogic owner)
        {
            owner.m_Animator.SetFloat(m_NormalBlend, 0f);
        }
    }
}