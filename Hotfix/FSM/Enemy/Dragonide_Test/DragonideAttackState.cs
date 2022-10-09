using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
namespace Farm.Hotfix
{
    public class DragonideAttackState : EnemyAttackState
    {
        private readonly float FRIST_PHASE = 0.7f;
        private readonly float SECOND_PHASE = 0.5f;
        private readonly float PushDistance = 2f; //距离
        private bool isDefense = false;
        AnimatorStateInfo info;
        private static readonly int MoveBlend = Animator.StringToHash("MoveBlend");
        private static readonly int FightBlend = Animator.StringToHash("FightBlend");
        private static readonly int FrenzyEnd = Animator.StringToHash("FrenzyEnd");
        private static readonly int Frenzy = Animator.StringToHash("Frenzy");
        private readonly static int m_stop = Animator.StringToHash("Stop");
        private readonly static int m_InFight = Animator.StringToHash("InFight");
        private bool isFrenzyEnd;
        //private readonly static int m_isback = Animator.StringToHash("IsBack");
        //private readonly static int m_isleft = Animator.StringToHash("IsLeft");
        //private readonly static int m_isright = Animator.StringToHash("IsRight");
        //private readonly static int m_WalkSpeed = Animator.StringToHash("WalkSpeed");
        //AnimatorStateInfo stateinfo;
        public static new DragonideAttackState Create()
        {
            DragonideAttackState state = ReferencePool.Acquire<DragonideAttackState>();
            return state;
        }
        

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            info = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
            AttackAnimationEnd();
            AIUtility.RotateToTarget(owner.find_Player, owner, -10, 10);
            owner.SetRichAiStop();
            //stateinfo = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
            //Log.Info("当前动画名" + stateinfo.IsName);
            //AniEnd();
            if (owner.LockingEntity)
            {
                float distance = AIUtility.GetDistance(owner, owner.LockingEntity);
                if (distance < 0.5f && info.normalizedTime >= 0.8f)
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
            
            owner.m_Animator.SetBool(m_InFight, true);
            int num = Utility.Random.GetRandom(0, 10);
            
            //不同阶段不同攻击方式
            if (owner.enemyData.HPRatio >= FRIST_PHASE)
            {
                owner.ReduceAttackTime = FRIST_PHASE;
                //Debug.Log("阶段一攻击");
                NormalAttack(); //普通攻击
                

            }
            else if (SECOND_PHASE < owner.enemyData.HPRatio && owner.enemyData.HPRatio < FRIST_PHASE)
            {
                //Debug.Log("阶段二攻击");
                if (num > 6)
                    SkillAttack(); //技能攻击
                else
                    NormalAttack(); //普通攻击
                

            }
            else if (owner.enemyData.HPRatio <= SECOND_PHASE && owner.enemyData.HPRatio>0.25)
            {

                //Debug.Log("进入狂暴状态");
                if (!isFrenzyEnd)
                {

                    owner.m_Animator.SetTrigger(Frenzy);
                    //GameEntry.Sound.PlaySound(26019);
                    //if (info.IsName("Protector_Attack05_Root"))
                    //{
                    //    owner.m_Animator.SetBool(FrenzyEnd, true);
                    //}
                    
                    StopBlend(owner);
                    Debug.Log("进入狂暴状态");
                    owner.IsDefense = false;
                    isFrenzyEnd = true;
                }
               

                owner.ReduceAttackTime = SECOND_PHASE;

                if (num > 8)
                    SkillAttack(); //技能攻击
                else
                    RageAttack(); //狂暴攻击
            }
            else if (owner.enemyData.HPRatio <= 0.25)
            {
                owner.m_Animator.SetInteger(AttackState, 14);
            }
            else
            {
                NormalAttack(); //普通攻击
               

            }
        }
        protected override void NormalAttack()
        {
            
            int randomNum = Utility.Random.GetRandom(0, 4);
            //Debug.Log("普通攻击" + randomNum);
            owner.m_Animator.SetInteger(AttackState, randomNum);
            
        }
        protected override void SkillAttack()
        {
            
            float disdance = AIUtility.GetDistance(owner, owner.LockingEntity);
            if (disdance < 1.5)
            {
                owner.m_Animator.SetInteger(AttackState, 4);
                
            }
            else
            {
                int randomNum = Utility.Random.GetRandom(4, 9);
                owner.m_Animator.SetInteger(AttackState, randomNum);
                
            }
            
        }
        private void RageAttack()
        {
            
            float disdance = AIUtility.GetDistance(owner, owner.LockingEntity);
            if (disdance < 1.5)
            {
                int randomNum = Utility.Random.GetRandom(12, 14);
                owner.m_Animator.SetInteger(AttackState, randomNum);
               
            }
            else 
            {
                int randomNum = Utility.Random.GetRandom(9, 11);
                owner.m_Animator.SetInteger(AttackState, randomNum);
                
            }
            
        }
        private void AttackAnimationEnd()
        {
            //Debug.Log("动画播放进度" + info.normalizedTime);
            ////判断动画是否播放完成
            if (info.normalizedTime >= 0.8f)
            {
                owner.AnimationEnd();
            }

        }


        protected override void StopBlend(EnemyLogic owner)
        {
            //owner.m_Animator.SetFloat(MoveBlend, Mathf.Lerp(owner.m_Animator.GetFloat(MoveBlend), 0, Time.deltaTime));
            //Debug.Log("停止移动");
            if(owner.m_Animator.GetFloat(MoveBlend)>0.8)
                owner.m_Animator.SetTrigger(m_stop);
            owner.m_Animator.SetFloat(MoveBlend, 0f);
            owner.m_Animator.SetFloat(FightBlend, 0f);
        }


        
    }
}
