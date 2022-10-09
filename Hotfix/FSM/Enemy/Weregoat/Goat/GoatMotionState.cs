using System.Collections;
using System.Collections.Generic;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework;
namespace Farm.Hotfix
{
    public class GoatMotionState : EnemyMotionState
    {
        private readonly static int m_MoveBlend = Animator.StringToHash("MoveBlend");
        private readonly static int m_IsIdle = Animator.StringToHash("Isidle");
        private readonly static int m_AttackState = Animator.StringToHash("AttackState");
        private readonly static int m_IsCommand = Animator.StringToHash("IsCommand");
        private readonly static int m_AttackIntend = Animator.StringToHash("AttackIntend");
        private bool notFight = true;
        GameObject mainObject;
        
        WeregoatLogic weregoatLogic;
        Vector3 wayPoint;
        private float Scope = 8f;
        private Vector3 isTransform;
        private float timer;
        private Vector3 recordTransform;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            recordTransform = owner.transform.position;
            //mainObject = owner.find_Player.m_Attacker;
            mainObject = GameObject.Find("SK_Weregoat");
            //Log.Info("获取物体名称" + mainObject.name);
            if (mainObject != null)
            {
                weregoatLogic = mainObject.gameObject.transform.parent.GetComponent<WeregoatLogic>();
                isTransform = weregoatLogic.transform.position;
            }
            GetNewWayPoint();
        }

        public static GoatMotionState Create()
        {
            GoatMotionState state = ReferencePool.Acquire<GoatMotionState>();
            return state;
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            
            if (mainObject != null)
                isTransform = weregoatLogic.transform.position;
            //float disdance = AIUtility.GetDistance(owner, weregoatLogic);
            if (notFight)
                IsPatrol(procedureOwner);
            else
            {
                
                owner.m_Animator.SetFloat(m_MoveBlend, 1, 0.5f, 2 * Time.deltaTime);
                owner.SetRichAIMove(owner.m_Animator.GetFloat(m_MoveBlend) * owner.enemyData.MoveSpeedRun);

                if (owner.IsCanAttack)
                {
                    owner.LockEntity(weregoatLogic.find_Player);

                    base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
                }
                else
                {
                    owner.SetRichAIMove();
                    owner.SetSearchTarget(wayPoint);

                    //owner.transform.LookAt(wayPoint, Vector3.up);
                    if (Vector3.Distance(wayPoint, owner.transform.position) <= 2)
                    {
                        owner.IsCanAttack = true;
                        owner.m_Animator.SetTrigger(m_AttackIntend);
                        
                    }
                }
                
            }

        }

        private void IsPatrol(ProcedureOwner procedureOwner)
        {
           
            //Log.Info("距离目标点" + Vector3.Distance(wayPoint, owner.transform.position));
            owner.m_Animator.SetFloat(m_MoveBlend, 0.5f, 0.2f, 2 * Time.deltaTime);
            
            timer += Time.deltaTime;
            if (timer > 3f)
            {
                //Log.Info("行走时间" + timer);
                recordTransform = owner.transform.position;
                timer = 0;
            }
            //Log.Info("移动距离" + Vector3.Distance(recordTransform, owner.transform.position));
            if (weregoatLogic.m_Animator.GetBool(m_IsCommand))
            {
                notFight = false;
            }
            
            else if (Vector3.Distance(wayPoint, owner.transform.position) <= 2 || Vector3.Distance(recordTransform, owner.transform.position)<0.3f)
            {
                //Log.Info("重新获取目标节点");
                //owner.m_Animator.SetBool(m_IsPatrol, true);
                //owner.transform.rotation = new Quaternion(owner.transform.rotation.x, owner.transform.rotation.y + 30, owner.transform.rotation.z,1); 
                //owner.m_Animator.SetFloat(m_MoveBlend, 0.3f, 0.5f, 2 * Time.deltaTime);
                //ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Idle));
                GetNewWayPoint();
            }
            else
            {
                //owner.transform.LookAt(wayPoint, Vector3.up);
                owner.SetRichAIMove();
                owner.SetSearchTarget(wayPoint);
                //owner.SetSearchTarget


            }

        }

        //private float GetAngle()
        //{
        //    Vector3 direction = wayPoint - owner.transform.position;
        //    float degree = Vector3.Angle(direction, owner.transform.forward);
        //    return degree;
        //}

        private void GetNewWayPoint()
        {
            float randomX = Random.Range(0, Scope);
            float randomZ = Random.Range(0, Scope);

            Vector3 randomPoint = new Vector3(isTransform.x + randomX, owner.transform.position.y + 0.5f, isTransform.z + randomZ);
            if (Vector3.Distance(randomPoint, owner.transform.position) > 5 && Vector3.Distance(randomPoint, owner.transform.position)<30)
                //wayPoint = randomPoint;
                wayPoint = AstarPath.active.GetNearest(randomPoint).position; 

            else
                wayPoint = owner.transform.position;
            //Log.Info("目标点" + wayPoint);
        }
        protected override void StopMoveBlend()
        {
            owner.m_Animator.SetFloat(m_MoveBlend, 0, 0.5f, 2 * Time.deltaTime);
            owner.SetRichAiStop();
            notFight = false;
        }
    }
}