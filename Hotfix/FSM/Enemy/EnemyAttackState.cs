using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

namespace Farm.Hotfix
{
    /// <summary>
    /// 敌人攻击状态默认为霸体(攻击中不会被打断)
    /// </summary>
    public class EnemyAttackState : EnemyBaseActionState
    {
       // protected static readonly int ShieldState = Animator.StringToHash("ShieldState");
        private static readonly int MoveBlend = Animator.StringToHash("MoveBlend");
        protected static readonly int AttackState = Animator.StringToHash("AttackState");
        protected static readonly int Shout = Animator.StringToHash("Shout");       
        protected static readonly int SkillState = Animator.StringToHash("SkillState");        
        protected static readonly int DefaultState = -1;
        private readonly float FRIST_PHASE = 0.7f;//第一阶段
        private readonly float SECOND_PHASE = 0.5f;//第二阶段
        private int IndexOrigin = 0;
        protected EnemyLogic owner;


        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            //获得攻击状态开始的下标
            //假设敌人攻击从Attack_01开始且是一个子状态机
            IndexOrigin = GetClipIndex(procedureOwner.Owner.m_Animator, "Attack_01");
            //owner.AttackTimer = 5;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            EnemyAttackStateStart(owner);
            owner.SetRichAiStop();
            //ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));

        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);


            if (owner.enemyData.HP <= 0)
            {
                owner.EnemyAttackEnd();
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Dead));
            }
            //if (owner.enemyData.HP != owner.enemyData.HP)
            //{

            //    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Hurt));

            //}
            if (owner.IsAnimPlayed)
            {
                float angle = AIUtility.GetPlaneAngle(owner.find_Player, owner);
                if (!AIUtility.CheckInAngle(-45f, 45f, angle))
                {
                    //Debug.Log("基础攻击--转向");
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Rotate));
                }
                else
                {
                    if (!owner.Stoic)
                    {
                        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
                        owner.m_Animator.SetInteger(AttackState, DefaultState);
                    }
                    //Debug.Log("基础攻击--战斗");
                    
                }
            }

        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.EnemyAttackEnd();
            owner.ResetAttack();
            owner.IsAnimPlayed = true;
            ResetAttackState();
        }
        
        public static EnemyAttackState Create()
        {
            EnemyAttackState state = ReferencePool.Acquire<EnemyAttackState>();
            return state;
        }

        public void Clear()
        {
            owner = null;
        }

        /// <summary>
        /// 获得动画机内指定动画片段的下标
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="clip">需要查找的动画片段名称</param>
        /// <returns>返回下标</returns>
        protected static int GetClipIndex(Animator animator, string clip)
        {
            if (null == animator || string.IsNullOrEmpty(clip) || null == animator.runtimeAnimatorController)
                return 0;

            RuntimeAnimatorController ac = animator.runtimeAnimatorController;
            AnimationClip[] tAnimationClips = ac.animationClips;
            if (null == tAnimationClips || tAnimationClips.Length <= 0)
                return 0;

            AnimationClip tAnimationClip;
            for (int tCounter = 0, tLen = tAnimationClips.Length; tCounter < tLen; tCounter++)
            {
                tAnimationClip = tAnimationClips[tCounter]; 
                if (null != tAnimationClip && tAnimationClip.name == clip)
                    return tCounter;
            }

            return 0;
        }

        /// <summary>
        /// 冲刺技能
        /// </summary>
        protected virtual void PushSkill()
        {
            int skill = GetClipIndex(owner.m_Animator, "Push");
            int playstate = skill - IndexOrigin;
            owner.m_Animator.SetInteger(AttackState, playstate);
            Debug.Log("冲刺");
        }

        /// <summary>
        /// 普通攻击
        /// </summary>
        protected virtual void NormalAttack()
        {
            int randomNum = Utility.Random.GetRandom(0, 2);
            owner.m_Animator.SetInteger(AttackState, randomNum);
        }
        protected virtual void MoveAttack()
        {
            int randomNum = Utility.Random.GetRandom(8, 10);
            owner.m_Animator.SetInteger(AttackState, randomNum);
        }
        /// <summary>
        /// 技能攻击
        /// </summary>
        protected virtual void SkillAttack()
        {
            int randomNum = Utility.Random.GetRandom(3, 7);
            owner.m_Animator.SetInteger(AttackState, randomNum);
        }
   
        protected virtual void SkillAttackSecond()
        {
            //int randomNum = Utility.Random.GetRandom(3, 6);
            owner.m_Animator.SetInteger(AttackState, 7);
        }
        /// <summary>
        /// 盾牌攻击
        /// </summary>
        protected virtual void ShieldAttack()
        {
            int randomNum = Utility.Random.GetRandom(0, 3);
           // owner.m_Animator.SetInteger(ShieldState, randomNum);
        }
        /// <summary>
        /// 重置攻击状态
        /// </summary>
        protected virtual void ResetAttackState()
        {
            owner.m_Animator.SetInteger(AttackState, DefaultState);
           // owner.m_Animator.SetInteger(ShieldState, DefaultState);
            //owner.m_Animator.SetInteger(SkillState, DefaultState);
        }

        /// <summary>
        /// 停止移动(混合树)
        /// </summary>
        protected virtual void StopBlend(EnemyLogic owner)
        {
            owner.m_Animator.SetFloat(MoveBlend, 0f);
        }

        /// <summary>
        /// 敌人攻击状态开始
        /// </summary>
        /// <param name="owner"></param>
        protected virtual void EnemyAttackStateStart(EnemyLogic owner)
        {
            owner.SetRichAiStop();
            StopBlend(owner);
        }
        
    }
}
