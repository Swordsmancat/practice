using Farm.Hotfix;
using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class GoblinAttackState : EnemyAttackState
    {
        private readonly float FRIST_PHASE = 0.7f;//第一阶段
        private readonly float SECOND_PHASE = 0.5f;//第二阶段
        private readonly float PushDistance = 2f;//推进的距离
        private readonly float PushPrec = 10f;
        private float m_PrevTimeHP;
        protected static readonly int SkillState = Animator.StringToHash("SkillState");
        private static readonly int Defense = Animator.StringToHash("Defense");
        private readonly static int m_NormalBlend = Animator.StringToHash("NormalBlend");
        int shout = 0;

        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (owner.IsParry) //闪避
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Parry));
            }

            if (m_PrevTimeHP != owner.enemyData.HP)
            {
                if(owner.find_Player.isThump) //玩家是重击
                {
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Hurt));
                }
            }
        }

        public static new GoblinAttackState Create()
        {
            GoblinAttackState state = ReferencePool.Acquire<GoblinAttackState>();
            return state;
        }

        protected override void EnemyAttackStateStart(EnemyLogic owner)
        {
            owner.SetRichAiStop();
            StopBlend(owner);
            int num = Utility.Random.GetRandom(0, 100);

            //不同阶段不同攻击方式
            //if (owner.enemyData.HPRatio >= FRIST_PHASE)
            //{
               
                if (owner.CurrentTargetDisdance <= 1.5 && owner.CurrentTargetDisdance > 0.8)
                {

                    ShieldAttack();
                }
                else if (owner.CurrentTargetDisdance <= 0.8 && num<=10)
                {
                    owner.m_Animator.SetTrigger(Defense);
                }
                else if (num <= 60)
                {
                    SkillAttack();
                }
                else if (owner.CurrentTargetDisdance > 5 && num <= 20)
                {
                    //owner.m_Animator.SetInteger(SkillState, 0);
                    PushSkill();
                }
                else if (num > 60 && num <= 70)
                 {
                int randomNum = Utility.Random.GetRandom(4, 7);
                owner.m_Animator.SetInteger(SkillState, randomNum);
                 }
                else
                {
                    NormalAttack();
                }
               
            
      
            

        }
        protected override void NormalAttack()
        {
            int randomNum = Utility.Random.GetRandom(0, 4);
            owner.m_Animator.SetInteger(AttackState, randomNum);
        }
        protected override void SkillAttack()
        {
            int randomNum = Utility.Random.GetRandom(1, 4);
            owner.m_Animator.SetInteger(SkillState, randomNum);
        }
        protected override void PushSkill()
        {
            int skill = GetClipIndex(owner.m_Animator, "Push");
            int playstate = skill - 0;
            owner.m_Animator.SetInteger(SkillState, playstate);
            Debug.Log("冲刺");
        }
         protected override void StopBlend(EnemyLogic owner)
        {
            owner.m_Animator.SetFloat(m_NormalBlend, 0f);
        }
    }
}

