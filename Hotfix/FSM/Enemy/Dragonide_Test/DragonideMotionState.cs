using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using UnityEngine;
using System;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class DragonideMotionState : EnemyMotionState
    {
        private readonly static int m_MoveBlend = Animator.StringToHash("MoveBlend");
        private readonly static int m_InFight = Animator.StringToHash("InFight");
        private readonly static int m_FightState = Animator.StringToHash("FightState");
        private readonly static int m_FightBlend = Animator.StringToHash("FightBlend");
        private readonly static int m_IsBack = Animator.StringToHash("IsBack");
        private readonly static int m_stop = Animator.StringToHash("Stop");
        private float currentMoveBlend;

        private readonly static float m_GoRunRange = 8f;
        private readonly static int m_FightStateRange = 5;
        //private bool IsRun = false;
        private bool IsInFight = false;  //攻击状态
        private int m_Chance_Do;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_Chance_Do = Utility.Random.GetRandom(0, 100);
            if (m_Chance_Do % 13 == 0 && m_Chance_Do < 40)
                GameEntry.Sound.PlaySound(26026);
            else if (m_Chance_Do % 7 == 0 && m_Chance_Do < 30)
                GameEntry.Sound.PlaySound(26027);
            else if (m_Chance_Do % 11 == 0 && m_Chance_Do < 50)
                GameEntry.Sound.PlaySound(26028);
            
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            
            //if (owner.LockingEntity == null)
            //{
            //    owner.LockEntity(owner.find_Player);  
            //}
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            float disdance = AIUtility.GetDistance(owner, owner.LockingEntity);
            owner.RestoreEnergy();
            if (owner.IsLocking)
            {
                //owner.transform.LookAt(owner.find_Player.transform.position, Vector3.up);
                if (disdance <= owner.enemyData.AttackRange + m_FightStateRange)
                {
                    IsInFight = true;
                }
                else
                {
                    IsInFight = false;
                }
                if (owner.LockingEntity != null && !owner.LockingEntity.IsDead)
                {
                    float distance = AIUtility.GetDistance(owner, owner.LockingEntity);
                    //判断玩家是否在攻击范围内
                    if (!owner.CheckInAttackRange(distance))
                    {
                        //若在攻击范围外
                        //设置混合树,设置怪物移动,设置怪物目标
                        MovementBlend();
                        owner.SetRichAIMove();
                        owner.SetSearchTarget(owner.LockingEntity.CachedTransform);
                        // Debug.Log("进行移动中`");
                    }
                    else if (!owner.CheckInAttackRange(distance)&&IsInFight)
                    {
                        //攻击范围外,战斗战斗状态中
                        StopMoveBlend();
                        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
                    }
                    else
                    {
                        StopMoveBlend();
                        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Attack));
                        // Debug.Log("进入攻击状态");
                    }
                }



            }
           // Debug.Log("距离" + disdance);
            


            //base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }
        public static DragonideMotionState Create()
        {
            DragonideMotionState state = ReferencePool.Acquire<DragonideMotionState>();
            return state;
        }
        protected override void MovementBlend()
        {
            currentMoveBlend = owner.m_Animator.GetFloat(m_MoveBlend);
            if (IsInFight)
            {
                if (owner.LockingEntity == null)
                {
                    owner.LockEntity(owner.find_Player);
                }
                float disdance = AIUtility.GetDistance(owner, owner.LockingEntity);
                owner.m_Animator.SetBool(m_InFight, true);
                //owner.m_Animator.SetFloat(m_FightBlend, 3);
                //owner.m_Animator.SetFloat(m_FightBlend,1, 0.25f, Time.deltaTime);
                //if (owner.LockingEntity != null)
                //{
                    //float disdance = AIUtility.GetDistance(owner, owner.LockingEntity);
                IsBack(disdance);

            //}

        }
            else
            {
                owner.m_Animator.SetBool(m_InFight, false);
                float disdance = AIUtility.GetDistance(owner, owner.LockingEntity);
                if (disdance >= m_GoRunRange)
                {
                    owner.m_Animator.SetFloat(m_MoveBlend, 1.5f, 0.5f, Time.deltaTime);
                    
                    owner.SetRichAIMove(owner.m_Animator.GetFloat(m_MoveBlend)* owner.enemyData.MoveSpeedRun);



                }
                //owner.transform.Translate(Vector3.forward * owner.m_Animator.GetFloat(m_MoveBlend)*8*Time.deltaTime);
                else
                {
                    owner.m_Animator.SetFloat(m_MoveBlend, 0.5f, 0.25f, Time.deltaTime);
                    owner.SetRichAIMove(owner.m_Animator.GetFloat(m_MoveBlend));
                    //owner.m_Animator.speed /= owner.m_Animator.humanScale;
                    //owner.transform.Translate(Vector3.forward * owner.m_Animator.GetFloat(m_MoveBlend) *8* Time.deltaTime);
                }

            }
            
        }
        /// <summary>
        /// 战斗状态移动
        /// </summary>
        /// <param name="disdance"></param>
        private void IsBack(float disdance)
        {
            //owner.m_Animator.SetBool(m_InFight, true);
            //目标方位
            //owner.transform.LookAt(owner.find_Player.transform.position, Vector3.up);
            Vector3 target = owner.find_Player.transform.position - owner.transform.position;
            Vector3 obj = owner.transform.forward;
            //bool left = (Vector3.Cross(target, obj).y > 0);
            float MoveX = owner.find_Player.MoveX;
            //Log.Info("水平玩家输入" + MoveX);
            bool left = MoveX>0;
            //Debug.Log("方位向量" + Vector3.Cross(target, obj).y);
            //Debug.Log("距离+++++" + disdance);
            
            if (disdance < 1.5)
            {
                owner.m_Animator.SetFloat(m_FightBlend, 0,0.5f,Time.deltaTime);
                owner.m_Animator.SetFloat(m_MoveBlend, 0);
                owner.SetRichAiStop();
                owner.m_Animator.SetBool(m_IsBack, true);
                //owner.SetRichAIMove(owner.m_Animator.GetFloat(m_MoveBlend));

            }

            else if (disdance > 2.5 && disdance < 5 && left)
            {
                //owner.m_Animator.SetFloat(m_FightBlend, 0.5f);
                owner.m_Animator.SetFloat(m_FightBlend, 1f,0.5f,2*Time.deltaTime);
                owner.m_Animator.SetFloat(m_MoveBlend, 0.8f, 0.5f, Time.deltaTime);
                //owner.SetRichAIMove(owner.m_Animator.GetFloat(m_MoveBlend));
                owner.m_Animator.SetBool(m_IsBack, false);

            }
            else if (disdance > 2.5 && disdance < 5 && !left)
            {
                //owner.m_Animator.SetFloat(m_FightBlend, 1.5f);
                owner.m_Animator.SetFloat(m_FightBlend, -1f, 0.5f, 2*Time.deltaTime);
                owner.m_Animator.SetFloat(m_MoveBlend,0.8f, 0.5f, Time.deltaTime);
                //owner.SetRichAIMove(owner.m_Animator.GetFloat(m_MoveBlend));

                owner.m_Animator.SetBool(m_IsBack, false);
            }
            else if (disdance >= 5)
            {
                //owner.m_Animator.SetFloat(m_FightBlend, 3f);
                owner.m_Animator.SetFloat(m_FightBlend, 0, 0.5f, 2*Time.deltaTime);
                owner.m_Animator.SetFloat(m_MoveBlend, 1f, 0.5f, Time.deltaTime);

                //owner.SetRichAIMove(owner.m_Animator.GetFloat(m_MoveBlend));

                owner.m_Animator.SetBool(m_IsBack, false);
            }
            


        }

        protected override void StopMoveBlend()
        {
            if (owner.m_Animator.GetFloat(m_MoveBlend) > 0.8)
                owner.m_Animator.SetTrigger(m_stop);
            owner.m_Animator.SetFloat(m_MoveBlend, 0f);
            owner.SetRichAiStop();
            //owner.SetRichAIMove(owner.m_Animator.GetFloat(m_MoveBlend));
            owner.m_Animator.SetFloat(m_FightBlend, 0.5f,0.5f, 2 * Time.deltaTime);
            //owner.m_Animator.SetFloat(m_FightBlend, Mathf.Lerp(owner.m_Animator.GetFloat(m_FightState), 0, 0.9f));
            //owner.m_Animator.SetTrigger(m_stop);
        }
       
    }
}
