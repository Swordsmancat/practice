using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
namespace Farm.Hotfix
{
    public class WeregoatAttackState : EnemyAttackState
    {
        private readonly float FRIST_PHASE = 0.7f;
        private readonly float SECOND_PHASE = 0.5f;
        private static readonly int MoveBlend = Animator.StringToHash("MoveBlend");
        private readonly static int m_PosX = Animator.StringToHash("PosX");
        private readonly static int m_PosY = Animator.StringToHash("PosY");
        private readonly static int m_Summoner = Animator.StringToHash("Summoner");
        private readonly static int m_Command = Animator.StringToHash("Command");
        private readonly static int m_IsCommand = Animator.StringToHash("IsCommand");
        public bool isSummoner=false;
        AnimatorStateInfo info;
        private int randomNum;


        public static new WeregoatAttackState Create()
        {
            WeregoatAttackState state = ReferencePool.Acquire<WeregoatAttackState>();
            return state;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            owner.RestoreEnergy();
            //owner.SetRichAiStop();
            AIUtility.RotateToTarget(owner.find_Player, owner, -10, 10);
            info = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
            AttackAnimationEnd();
            owner.SetRichAiStop();
            if (owner.LockingEntity)
            {
                float distance = AIUtility.GetDistance(owner, owner.LockingEntity);
                if (distance < 0.5f)
                {
                    StopBlend(owner);
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
                }
            }

            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }
        protected override void EnemyAttackStateStart(EnemyLogic owner)
        {
            //if (owner.LockingEntity != null)
            //{
            //    float disdance = AIUtility.GetDistance(owner, owner.LockingEntity);
            //    IsBack(disdance);

            //}
            owner.SetRichAiStop();

            StopBlend(owner);
            int randomNum = Utility.Random.GetRandom(0, 10);

            //²»Í¬½×¶Î²»Í¬¹¥»÷·½Ê½
            if (owner.enemyData.HPRatio >= FRIST_PHASE)
            {
                owner.ReduceAttackTime = FRIST_PHASE;
               // Debug.Log("½×¶ÎÒ»¹¥»÷");
                NormalAttack(); //ÆÕÍ¨¹¥»÷


            }
            else if (SECOND_PHASE < owner.enemyData.HPRatio && owner.enemyData.HPRatio < FRIST_PHASE)
            {
                //Debug.Log("½×¶Î¶þ¹¥»÷");
                if (randomNum > 6)
                    SkillAttack(); //¼¼ÄÜ¹¥»÷
                else
                    NormalAttack(); //ÆÕÍ¨¹¥»÷
            }
            else if (owner.enemyData.HPRatio <= SECOND_PHASE)
            {
                //Debug.Log("¿ñ±©×´Ì¬");
                if (!isSummoner)
                {
                    owner.m_Animator.SetTrigger(m_Summoner);
                    isSummoner = true;
                    owner.m_Animator.SetTrigger(m_Command);
                    owner.m_Animator.SetBool(m_IsCommand, true);
                    //SummonGoat(6);
                }
                    
                RageAttack();
            }
            else
            {
                NormalAttack(); //ÆÕÍ¨¹¥»÷
            }
        }

            protected override void NormalAttack()
        {

            randomNum = Utility.Random.GetRandom(0, 4);
            //Debug.Log("ÆÕÍ¨¹¥»÷" + randomNum);
            owner.m_Animator.SetInteger(AttackState, randomNum);

        }
        protected override void SkillAttack()
        {

            float disdance = AIUtility.GetDistance(owner, owner.LockingEntity);
            if (disdance < 1.2f)
            {
                owner.m_Animator.SetInteger(AttackState, 4);

            }
            else
            {
                randomNum = Utility.Random.GetRandom(4, 9);
                owner.m_Animator.SetInteger(AttackState, randomNum);

            }

        }
        private void RageAttack()
        {
            
            float disdance = AIUtility.GetDistance(owner, owner.LockingEntity);
            if (disdance < 1.2f )
            {
                randomNum = Utility.Random.GetRandom(8, 10);
                owner.m_Animator.SetInteger(AttackState, randomNum);

            }
            else
            {
                randomNum = Utility.Random.GetRandom(10, 13);
                owner.m_Animator.SetInteger(AttackState, randomNum);
                //if (randomNum == 12)
                //{
                //    owner.m_Animator.SetTrigger(m_Command);
                //    owner.m_Animator.SetBool(m_IsCommand,true);
                //}
                    

            }


        }
        //private void SummonGoat(int num)
        //{
        //    int i = 0;
        //    while (i < num)
        //    {
        //        GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 60015)
        //        {
        //            Position = owner.transform.position,
        //            //Position = LockTransform.position,
        //            Scale = new UnityEngine.Vector3(1, 1, 1),
        //            //KeepTime = 1.5f
        //        },typeof(GoatLogic));
        //        i++;
        //    }

        //}

        protected override void StopBlend(EnemyLogic owner)
        {
            //owner.m_Animator.SetFloat(MoveBlend, Mathf.Lerp(owner.m_Animator.GetFloat(MoveBlend), 0, Time.deltaTime));
            //Debug.Log("Í£Ö¹ÒÆ¶¯");
            //if (owner.m_Animator.GetFloat(MoveBlend) > 0.8)
            //    owner.m_Animator.SetTrigger(m_stop);
            owner.m_Animator.SetFloat(MoveBlend, 0f);
            owner.m_Animator.SetFloat(m_PosX, 0f);
            owner.m_Animator.SetFloat(m_PosY, 0f);
            //owner.m_Animator.SetFloat(FightBlend, 0f);
        }
        private void AttackAnimationEnd()
        {
            //Debug.Log("¶¯»­²¥·Å½ø¶È" + info.normalizedTime);
            ////ÅÐ¶Ï¶¯»­ÊÇ·ñ²¥·ÅÍê³É
            if (info.normalizedTime >= 0.4f)
            {
                owner.AnimationEnd();
            }
            
        }

    }
}
