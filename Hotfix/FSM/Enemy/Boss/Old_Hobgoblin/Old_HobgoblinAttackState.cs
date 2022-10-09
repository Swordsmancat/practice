using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class Old_HobgoblinAttackState : EnemyAttackState
    {
        private readonly float FRIST_PHASE = 0.4f;
       // private readonly float SECOND_PHASE = 0.4f;
        private readonly float PushDistance = 2f;
        private readonly float CloseRange = 0.2f;
        private readonly float PushPrec = 10f;

        int p = 0;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Debug.Log("进入攻击状态");
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            owner.AttackLockEntity(owner.find_Player);
            owner.SetSearchTarget(owner.LockingEntity.CachedTransform);
            float distance = AIUtility.GetDistance(owner.LockingEntity, owner);
        }
        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.m_Animator.SetInteger(AttackState, DefaultState);
            Debug.Log("退出攻击状态");
        }



        public static new Old_HobgoblinAttackState Create()
        {
            Old_HobgoblinAttackState state = ReferencePool.Acquire<Old_HobgoblinAttackState>();
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
                if (p < 1)
                {
                    owner.m_Animator.SetTrigger(Shout);
                    p++;
                }
                //owner.m_Animator.SetInteger(AttackState,9);

                if (owner.CurrentTargetDisdance >= PushDistance && num <= PushPrec)
                {
                    owner.m_Animator.SetInteger(AttackState, 9);
                }
                else if (owner.CurrentTargetDisdance <= CloseRange)
                {
                    int randomNum = Utility.Random.GetRandom(7, 9);
                    owner.m_Animator.SetInteger(AttackState, randomNum);
                }
                //else if (owner.CurrentTargetDisdance <= SurroundDistance && num >= 50)
                //{
                //    int randomNum = Utility.Random.GetRandom(20, 22);
                //    owner.m_Animator.SetInteger(AttackState, randomNum);
                //}
                else if (num <= 40)
                {
                    int randomNum = Utility.Random.GetRandom(4, 7);
                    owner.m_Animator.SetInteger(AttackState, randomNum);
                }
                else
                {
                    int randomNum = Utility.Random.GetRandom(0, 4);
                    owner.m_Animator.SetInteger(AttackState, randomNum);
                }
            }
            else if (owner.enemyData.HPRatio <= FRIST_PHASE)
            {
                //第二阶段
                owner.ReduceAttackTime = FRIST_PHASE;
                if (p < 2)
                {
                    owner.m_Animator.SetTrigger(Shout);
                    p++;
                }
                if (owner.CurrentTargetDisdance >= PushDistance && num <= PushPrec)
                {
                    int randomNum = Utility.Random.GetRandom(9, 11);
                    owner.m_Animator.SetInteger(AttackState, randomNum);
                }
                //else if (owner.CurrentTargetDisdance <= CloseRange)
                //{
                //    int randomNum = Utility.Random.GetRandom(7, 9);
                //    owner.m_Animator.SetInteger(AttackState, randomNum);
                //    owner.m_Animator.SetTrigger(attackT);
                //}
                else if (num >= 85)
                {
                    owner.m_Animator.SetInteger(AttackState, 10);
                }

                else if (num <= 50)
                {
                    Debug.Log("攻击--冲刺");
                    owner.ChangeStateEnemy(EnemyStateType.Push);
                }
                else if (num > 50 && num <= 75)
                {
                    int randomNum = Utility.Random.GetRandom(4, 7);
                    owner.m_Animator.SetInteger(AttackState, randomNum);
                }
                else
                {
                    int randomNum = Utility.Random.GetRandom(0, 4);
                    owner.m_Animator.SetInteger(AttackState, randomNum);
                }
            }
        }
    }

    //else if (owner.enemyData.HPRatio <= FRIST_PHASE && owner.enemyData.HPRatio >= SECOND_PHASE)
    //{
    //    //第二阶段
    //    owner.ReduceAttackTime = SECOND_PHASE;
    //    if (owner.CurrentTargetDisdance >= PushDistance && num <= PushPrec)
    //    {
    //        PushSkill();
    //    }
    //    else if (num <= 30)
    //    {
    //        SkillAttack();
    //    }
    //    else
    //    {
    //        NormalAttack();
    //    }
    //}
    //else if (owner.enemyData.HPRatio <= SECOND_PHASE)
    //{
    //    //第三阶段
    //    owner.ReduceAttackTime = FRIST_PHASE;
    //    if (owner.CurrentTargetDisdance >= PushDistance && num <= PushPrec)
    //    {
    //        PushSkill();
    //    }
    //    else if (num <= 50)
    //    {
    //        SkillAttack();
    //    }
    //    else
    //    {
    //        NormalAttack();
    //    }
    //}


}