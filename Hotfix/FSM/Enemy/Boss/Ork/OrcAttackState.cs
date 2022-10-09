using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class OrcAttackState : EnemyAttackState
    {
        private readonly float FRIST_PHASE = 0.7f;
        private readonly float SECOND_PHASE = 0.5f;
        private readonly float PushDistance = 2f; //距离
        private readonly float PushPrec = 10f;


        
        public static new OrcAttackState Create()
        {
            OrcAttackState state = ReferencePool.Acquire<OrcAttackState>();
            return state;
        }

        protected override void EnemyAttackStateStart(EnemyLogic owner)
        {
            owner.SetRichAiStop();
            StopBlend(owner);
            int num = Utility.Random.GetRandom(0, 100);

            //不同阶段不同攻击方式
            if (owner.enemyData.HPRatio >= FRIST_PHASE)
            {
                NormalAttack(); //普通攻击
            }
            else if (owner.enemyData.HPRatio <= FRIST_PHASE && owner.enemyData.HPRatio >= SECOND_PHASE)
            {
                //第二阶段
                owner.ReduceAttackTime = SECOND_PHASE;
                if (owner.CurrentTargetDisdance >= PushDistance && num <= PushPrec)
                {
                    PushSkill();
                }
                else if (num <= 30)
                {
                    SkillAttack();
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
                else if (num <= 50)
                {
                    SkillAttack();
                }
                else
                {
                    NormalAttack();
                }
            }
        }
    }
}

