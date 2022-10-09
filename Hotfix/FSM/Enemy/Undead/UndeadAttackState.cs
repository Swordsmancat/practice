using Farm.Hotfix;
using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix { 
public class UndeadAttackState : EnemyAttackState
{
        private readonly float FRIST_PHASE = 0.7f;//第一阶段
        private readonly float SECOND_PHASE = 0.5f;//第二阶段
        private readonly float PushDistance = 2f;//推进的距离
        private readonly float PushPrec = 10f;
        protected static readonly int ShieldState = Animator.StringToHash("ShieldState");
        private static readonly int FightSecond = Animator.StringToHash("fightSecond");
        int shout = 0;
        public static new UndeadAttackState Create()
    {
        UndeadAttackState state = ReferencePool.Acquire<UndeadAttackState>();
        return state;
    }
       
       
        protected override void EnemyAttackStateStart(EnemyLogic owner)
        {
            owner.SetRichAiStop();
            StopBlend(owner);
            int num = Utility.Random.GetRandom(0, 100);

           // owner.m_Animator.SetInteger(AttackState, 6);
            //不同阶段不同攻击方式
            if (owner.enemyData.HPRatio >= FRIST_PHASE)
            {

                if (owner.CurrentTargetDisdance < 1)
                {
                    ShieldAttack();
                }
                else if (num <= 35 && owner.CurrentTargetDisdance <= 1)
                {
                    MoveAttack();
                }
                else
                {
                    NormalAttack();
                }
            }
            else if (owner.enemyData.HPRatio <= FRIST_PHASE && owner.enemyData.HPRatio >= SECOND_PHASE)
            {
                //第二阶段
                owner.ReduceAttackTime = SECOND_PHASE;
                if (shout < 1)
                {
                    owner.m_Animator.SetTrigger(FightSecond);

                }
                shout++;
                //owner.m_Animator.SetTrigger(FightSecond);
                if (owner.CurrentTargetDisdance >= PushDistance && num <= PushPrec)
                {
                    PushSkill();
                }
                else if (num <= 95 && num > 30)
                {
                    SkillAttack();
                }
                else if (owner.CurrentTargetDisdance < 0.8)
                {
                    ShieldAttack();
                }
                else if (num > 20 && num <= 30)
                {
                    SkillAttackSecond();
                }
                else if (num <= 20)
                {
                    MoveAttack();
                }
                else
                {
                    NormalAttack();
                }
            }
            else if (owner.enemyData.HPRatio <= SECOND_PHASE)
            {

                //第三阶段
                owner.ReduceAttackTime = FRIST_PHASE;


                if (owner.CurrentTargetDisdance >= PushDistance && num <= PushPrec)
                {
                    PushSkill();

                }
                else if (num <= 95 && num >= 50)
                {
                    SkillAttack();

                }
                else if (num > 20 && num < 50)
                {
                    MoveAttack();
                }
                else if (num <= 20)
                {
                    SkillAttackSecond();
                }
                else
                {
                    NormalAttack();

                }
            }

        }

        protected override void NormalAttack()
        {
            int randomNum = Utility.Random.GetRandom(0, 3);
            owner.m_Animator.SetInteger(AttackState, randomNum);
        }

        //protected override void EnemyAttackStateStart(EnemyLogic owner)
        //{
        //    base.EnemyAttackStateStart(owner);
        //    NormalAttack();
        //    //ShieldAttack();
        //}
        protected override void ShieldAttack()
        {
            int randomNum = Utility.Random.GetRandom(0, 3);
            owner.m_Animator.SetInteger(ShieldState, randomNum);
        }

    }
}
