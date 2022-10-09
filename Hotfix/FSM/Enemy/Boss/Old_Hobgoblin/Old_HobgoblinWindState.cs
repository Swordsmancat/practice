using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using Pathfinding;



namespace Farm.Hotfix
{
    public class Old_HobgoblinWindState : EnemyBaseActionState
    {
        private EnemyLogic owner;
        private TargetableObject m_Plyaer;
        private int m_Num;
        private readonly static int m_AvoidBack = Animator.StringToHash("AvoidBack");
        protected static readonly int AvoidBacknum = Animator.StringToHash("AvoidBacknum");
        private  static int T_MoveBlend = Animator.StringToHash("MoveBlend");
        protected static readonly int AttackState = Animator.StringToHash("AttackState");
        protected static readonly int WindNum = Animator.StringToHash("WindNum");
        protected static readonly int m_CanAvoid = Animator.StringToHash("CanAvoid");


        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            owner = procedureOwner.Owner;
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Debug.Log("进入环绕状态");

            m_Num = Utility.Random.GetRandom(0, 100);
            T_MoveBlend = 0;
            //owner.m_Animator.applyRootMotion = true;
            owner.m_Animator.SetInteger(WindNum, Utility.Random.GetRandom(0, 2));
        }
        private float a_Time = 0;
        private float p_Time = 0;
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            float distance = AIUtility.GetDistance(owner.LockingEntity, owner);
            p_Time += elapseSeconds;
            owner.WindLockEntity(owner.find_Player);
            owner.SetSearchTarget(owner.LockingEntity.CachedTransform);
            if (distance <= owner.enemyData.AttackRange)
            {
                a_Time += elapseSeconds;
                if (m_Num <= 33)
                {
                    //Debug.Log("环绕距离" + distance);
                    //Debug.Log("攻击范围" + owner.enemyData.AttackRange);
                    //Debug.Log("环绕--攻击 1");
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
                }
                else if (m_Num >= 66)
                {
                    if(a_Time >= 1f)
                    {
                        //Debug.Log("环绕距离" + distance);
                        //Debug.Log("攻击范围" + owner.enemyData.AttackRange);
                        //Debug.Log("环绕--攻击 2");
                        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
                    }
                }
                else if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    Debug.Log("环绕闪避");
                    owner.m_Animator.SetBool(m_CanAvoid,true);
                    owner.m_Animator.SetTrigger(m_AvoidBack);
                    owner.m_Animator.SetInteger(AvoidBacknum, Utility.Random.GetRandom(0, 4));
                    //Debug.Log("环绕距离" + distance);
                    //Debug.Log("攻击范围" + owner.enemyData.AttackRange);
                    //Debug.Log("环绕--攻击 3");
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
                }
                else
                {
                    if (a_Time > 3f)
                    {
                        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
                    }
                }
            }
            else if (distance >= owner.enemyData.AttackRange * 3)
            {
                //Debug.Log("环绕距离" + distance);
                //Debug.Log("追击范围" + owner.enemyData.AttackRange * 3);
                //Debug.Log("环绕--移动 1");
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            }
            else if(p_Time >= 4f)
            {
                //Debug.Log("环绕--移动 2");
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            a_Time = 0;
            p_Time = 0;
            m_Num = 0;
            owner.m_Animator.SetInteger(WindNum, -1);
            owner.m_Animator.SetBool(m_CanAvoid, false);
            //owner.m_Animator.applyRootMotion = false;
            Debug.Log("退出环绕状态");

        }


        public static Old_HobgoblinWindState Create()
        {
            Old_HobgoblinWindState state = ReferencePool.Acquire<Old_HobgoblinWindState>();
            return state;
        }

        /// <summary>
        /// 敌人旋转状态开始
        /// </summary>
        protected virtual void EnemyRotateStateStart(EnemyLogic owner)
        {
            owner.SetRichAiStop();
            owner.LockEntity(owner.find_Player);
            m_Plyaer = owner.LockingEntity;
            owner.UnLockEntity();//解除对玩家的锁定不然怪物会直接瞬间面向玩家
            owner.IsBlock = true;
        }
    }
}

