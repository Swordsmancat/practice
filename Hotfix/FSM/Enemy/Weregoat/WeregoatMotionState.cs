using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using Pathfinding;
namespace Farm.Hotfix
{

    public class WeregoatMotionState : EnemyMotionState
    {

        private readonly static int m_MoveBlend = Animator.StringToHash("MoveBlend");
        private readonly static int m_Posture = Animator.StringToHash("Posture");
        private readonly static int m_IsPatrol = Animator.StringToHash("IsPatrol");
        private readonly static int m_IsAnger = Animator.StringToHash("IsAnger");
        private readonly static int m_PosX = Animator.StringToHash("PosX");
        private readonly static int m_PosY = Animator.StringToHash("PosY");
        private readonly static int m_IsIdle = Animator.StringToHash("IsIdle");
        private readonly static int m_IsMove = Animator.StringToHash("IsMove");
        private readonly static int m_FightD = Animator.StringToHash("FightD");
        private readonly static int m_Appel = Animator.StringToHash("Appel");
        private readonly static int m_AppelState = Animator.StringToHash("AppelState");
        private bool OnlyOnce = true;
        private readonly static int m_BlockStatePerc = 80;
        private float Scope = 15f; //巡逻范围
        private Vector3 wayPoint; //巡逻点位
        private Vector3 isTransform;
        private WeregoatLogic me;

        private bool IsInFight;
        private readonly static float m_GoRunRange = 8f;
        private readonly static int m_FightStateRange = 5;
        private readonly float SECOND_PHASE = 0.5f;
        private float FightMax = 5;
        private float FightMin = 2.5f;
        private int m_ChanceDo;
        AnimatorStateInfo info;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            //Debug.Log("进入移动状态");
            base.OnEnter(procedureOwner);
            isTransform = owner.OriginPoint;
            GetNewWayPoint();
            owner.m_Animator.SetBool(m_IsIdle, false);
            owner.m_Animator.SetBool(m_IsPatrol, false);
            owner.m_Animator.SetBool(m_IsAnger, false);
            owner.m_Animator.SetBool(m_IsMove, true);
            if (owner.Energy < 50)
                OnlyOnce = true;
            me = owner as WeregoatLogic;
            //if (me.BackWeapon != null)
            //{
            //    me.BackWeaponClose();
            //}
                m_ChanceDo = Utility.Random.GetRandom(0, 100);
            if (m_ChanceDo % 13 == 0 && m_ChanceDo < 20)
                GameEntry.Sound.PlaySound(26026);
            else if (m_ChanceDo % 7 == 0 && m_ChanceDo > 60)
                GameEntry.Sound.PlaySound(26032);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            owner.RestoreEnergy();
            
            if (owner.m_Animator.GetFloat(m_Posture)==0)
            {
                //OnDrawGizmosSelected();
                IsPatrol(procedureOwner);
            }
           
            else
            {
                
                info = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
                if (me.BackWeapon.activeSelf && info.IsName("BackWeaponClose") && info.normalizedTime > 0.15f)
                    me.BackWeaponClose();
                if (m_ChanceDo <= m_BlockStatePerc && owner.find_Player.m_Attack ) //owner.find_Player.m_moveBehaviour.isAttack
                {
                    //闪避
                    if (OnlyOnce)
                    {
                        owner.m_Animator.SetTrigger(m_Appel);
                        int num = Utility.Random.GetRandom(0, 3);
                        owner.m_Animator.SetInteger(m_AppelState, num);
                        owner.IsRoll = true;
                        OnlyOnce = false; 
                        
                        MoveAnimationEnd();
                    }

                }
                base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
                float disdance = AIUtility.GetDistance(owner, owner.LockingEntity);
                //OnDrawGizmosSelected();
                //owner.m_Animator.SetBool(m_IsIdle, false);
                if (owner.IsLocking)
                {
                    
                    if (disdance <= owner.enemyData.AttackRange + m_FightStateRange)
                    {
                        IsInFight = true;
                    }
                    else if (owner.enemyData.HPRatio < SECOND_PHASE && disdance <= owner.enemyData.AttackRange + m_FightStateRange * 3)
                        IsInFight = true;
                    else
                    {
                        IsInFight = false;
                    }
                    if (owner.LockingEntity != null && !owner.LockingEntity.IsDead)
                    {
                        float distance = AIUtility.GetDistance(owner, owner.LockingEntity);
                        //玩家是否在攻击范围内

                        if (!owner.CheckInAttackRange(distance))
                        {
                            //若在攻击范围外
                            //设置混合树,设置怪物移动,设置怪物目标
                            MovementBlend();
                            owner.SetRichAIMove();
                            owner.SetSearchTarget(owner.LockingEntity.CachedTransform);
                            // Debug.Log("进行移动中`");
                        }
                        else if (!owner.CheckInAttackRange(distance) && IsInFight)
                        {
                            //攻击范围外,战斗战斗状态中
                            //StopMoveBlend();
                            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
                        }
                        else
                        {
                            
                            StopMoveBlend();
                            if (owner.IsCanAttack)
                                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Attack));
                            // Debug.Log("进入攻击状态");
                        }
                    }
                }
            }
            
            
            
        }
        private void MoveAnimationEnd()
        {
            //Debug.Log("动画播放进度" + info.normalizedTime);
            ////判断动画是否播放完成
            if (info.normalizedTime >= 0.8f)
            {
                owner.IsRoll = false;

                owner.AnimationEnd();
                //Debug.Log("+++动画播放完成++++++");
            }

        }

        protected override void MovementBlend()
        {
            float disdance = AIUtility.GetDistance(owner, owner.LockingEntity);
            if (!IsInFight)
                NormalMobile(disdance);
            else if (owner.enemyData.HPRatio > SECOND_PHASE && IsInFight)
                FightMobile(disdance);
            else if (owner.enemyData.HPRatio < SECOND_PHASE)
                RageMobile(disdance);



        }
        public static WeregoatMotionState Create()
        {
            WeregoatMotionState state = ReferencePool.Acquire<WeregoatMotionState>();
            return state;
        }
        /// <summary>
        /// 普通移动
        /// </summary>
        private void NormalMobile(float disdance)
        {
            //owner.m_Animator.SetBool(m_IsIdle, true);
            owner.m_Animator.SetFloat(m_Posture, 1);
            if(disdance >= m_GoRunRange)
                owner.m_Animator.SetFloat(m_MoveBlend, 1, 0.5f, 2 * Time.deltaTime);
            else
                owner.m_Animator.SetFloat(m_MoveBlend, 0.5f, 0.25f, 2 * Time.deltaTime);
            owner.SetRichAIMove(owner.m_Animator.GetFloat(m_MoveBlend) * owner.enemyData.MoveSpeedRun * 0.5f);
        }
        /// <summary>
        /// 战斗移动
        /// </summary>
        /// <param name="disdance"></param>
        private void FightMobile(float disdance)
        {
            owner.m_Animator.SetFloat(m_Posture, 2,0.5f, Time.deltaTime);
            float MoveX = owner.find_Player.MoveX;
            
            bool left = MoveX > 0;
            owner.SetRichAIMove(owner.m_Animator.GetFloat(m_PosY) * owner.enemyData.MoveSpeedRun * 0.6f);

            if (disdance < 1.5)
            {
                owner.m_Animator.SetFloat(m_PosY, -2f, 0.5f, 2 * Time.deltaTime);
                owner.m_Animator.SetFloat(m_PosX, 0, 0.5f, 2 * Time.deltaTime);
                owner.SetRichAiStop();
            }

            else if (disdance > 1.5 && disdance < 5 && left)
            {
                //owner.m_Animator.SetFloat(m_FightBlend, 0.5f);
                owner.m_Animator.SetFloat(m_PosX, 2f, 0.5f, 2 * Time.deltaTime);
                owner.m_Animator.SetFloat(m_PosY, 1, 0.5f, 2 * Time.deltaTime);

            }
            else if (disdance > 1.5 && disdance < 5 && !left)
            {
                //owner.m_Animator.SetFloat(m_FightBlend, 1.5f);
                owner.m_Animator.SetFloat(m_PosX, -2f, 0.5f, 2 * Time.deltaTime);
                owner.m_Animator.SetFloat(m_PosY, 1, 0.5f, 2 * Time.deltaTime);
            }
            else if (disdance >= 6)
            {
                //owner.m_Animator.SetFloat(m_FightBlend, 3f);
                owner.m_Animator.SetFloat(m_PosY, 2f, 0.5f, 2 * Time.deltaTime);
                owner.m_Animator.SetFloat(m_PosX, 0, 0.5f, 2 * Time.deltaTime);
            }
            else
            {
                owner.m_Animator.SetFloat(m_PosY, 0, 0.5f, 2 * Time.deltaTime);
                owner.m_Animator.SetFloat(m_PosX, 0, 0.5f, 2 * Time.deltaTime);
            }



        }

        /// <summary>
        /// 狂暴移动
        /// </summary>
        /// <param name="disdance"></param>
        private void RageMobile(float disdance)
        {
            owner.m_Animator.SetFloat(m_Posture, 3);
            owner.m_Animator.SetBool(m_IsAnger, true);
            float MoveX = owner.find_Player.MoveX;
            bool left = MoveX > 0;
            owner.SetRichAIMove(owner.m_Animator.GetFloat(m_PosY) * owner.enemyData.MoveSpeedRun * 0.6f);
            if (disdance <= 1.5)
            {
                owner.m_Animator.SetFloat(m_PosY, -2f, 0.5f, 2 * Time.deltaTime);
                owner.m_Animator.SetFloat(m_PosX, 0, 0.5f, 2 * Time.deltaTime);
                owner.SetRichAiStop();
            }

            else if (disdance > 2.5 && disdance < 5 && left)
            {
                //owner.m_Animator.SetFloat(m_FightBlend, 0.5f);
                owner.m_Animator.SetFloat(m_PosX, 2f, 0.5f, 2 * Time.deltaTime);
                owner.m_Animator.SetFloat(m_PosY, 0, 0.5f, 2 * Time.deltaTime);
            }
            else if (disdance > 2.5 && disdance < 5 && !left)
            {
                //owner.m_Animator.SetFloat(m_FightBlend, 1.5f);
                owner.m_Animator.SetFloat(m_PosX, -2f, 0.5f, 2 * Time.deltaTime);
                owner.m_Animator.SetFloat(m_PosY, 0, 0.5f, 2 * Time.deltaTime);
            }
            else if (disdance >= 6)
            {
                //owner.m_Animator.SetFloat(m_FightBlend, 3f);
                owner.m_Animator.SetFloat(m_PosY, 3f, 0.5f, Time.deltaTime);
                owner.m_Animator.SetFloat(m_PosX, 0, 0.5f, 2 * Time.deltaTime);
            }
            else
            {
                owner.m_Animator.SetFloat(m_PosY, 0, 0.5f, 2 * Time.deltaTime);
                owner.m_Animator.SetFloat(m_PosX, 0, 0.5f, 2 * Time.deltaTime);
            }
        }

        /// <summary>
        /// 巡逻
        /// </summary>
        private void IsPatrol(ProcedureOwner procedureOwner)
        {
            //owner.m_Animator.SetFloat(m_Posture, 0);
            //Log.Info("距离目标点" + Vector3.Distance(wayPoint, owner.transform.position));
            owner.m_Animator.SetFloat(m_MoveBlend, 1, 0.5f, 2 * Time.deltaTime);
            float disdance = Vector3.Distance(owner.find_Player.transform.position, owner.transform.position);
            //Debug.Log("与玩家距离" + disdance + "是否在是视野内" + owner.CheckInSeekRange(disdance));
            // Debug.Log("是否在是视野内"+ owner.CheckInSeekRange(disdance));
            float angle = AIUtility.GetAngleInSeek(owner, owner.find_Player);
            //if (owner.CheckInSeekAngle(angle))
            if (owner.CheckInSeekRange(angle)&& owner.CheckInSeekRange(disdance))
            {
                Log.Info("发现玩家,进入追击");
                owner.LockEntity(owner.find_Player);
                owner.m_Animator.SetBool(m_IsIdle, false);
                owner.m_Animator.SetBool(m_IsPatrol, false);
                owner.m_Animator.SetTrigger(m_FightD);
                //me.BackWeaponClose();
                //ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Idle));
                //owner.transform.rotation = owner.OriginRotate;
                owner.m_Animator.SetFloat(m_Posture, 1);
                
                
                
                //Debug.Log("状态"+ owner.m_Animator.GetFloat(m_Posture));
            }
                
            else if (Vector3.Distance(wayPoint, owner.transform.position) <= 2)
            {
                owner.m_Animator.SetBool(m_IsPatrol, true);
                owner.m_Animator.SetFloat(m_MoveBlend, 0, 0.5f, 2 * Time.deltaTime);
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Idle));
                GetNewWayPoint();
            }
            else
            {
                owner.SetRichAIMove();
                owner.SetSearchTarget(wayPoint);
                //owner.m_Animator.SetFloat(m_MoveBlend, 1, 0.5f, 2 * Time.deltaTime);
                //owner.SetSearchTarget(wayPoint);
                //owner.SetSearchTarget(wayPoint);
                
                //m_AI.destination = wayPoint;
                //owner.transform.position = Vector3.Lerp(owner.transform.position, wayPoint, Time.deltaTime * 0.2f);
                //owner.transform.LookAt(wayPoint, Vector3.up);
            }
            
        }

        private void GetNewWayPoint()
        {
            float randomX = Random.Range(-Scope, Scope);
            float randomZ = Random.Range(-Scope, Scope);

            Vector3 randomPoint = new Vector3(isTransform.x + randomX, owner.transform.position.y+0.5f, isTransform.z + randomZ);
            if (Vector3.Distance(randomPoint, owner.transform.position) > 8)
                //wayPoint = randomPoint;
                wayPoint = AstarPath.active.GetNearest(randomPoint).position;
            else
                wayPoint = owner.transform.position;
            //Log.Info("目标点" + wayPoint);
        }
        /// <summary>
        /// 绘制目标巡逻范围
        /// </summary>
        //private void OnDrawGizmosSelected()
        //{
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawWireSphere(owner.transform.position, Scope);
        //}
        

        protected override void StopMoveBlend()
        {
            owner.m_Animator.SetFloat(m_MoveBlend, 0, 0.5f, 2 * Time.deltaTime);
            owner.m_Animator.SetFloat(m_PosY, 0, 0.5f, 2 * Time.deltaTime);
            owner.m_Animator.SetFloat(m_PosX, 0, 0.5f, 2 * Time.deltaTime);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            owner.m_Animator.SetBool(m_IsIdle, false);
            base.OnLeave(fsm, isShutdown);
        }
    }

}