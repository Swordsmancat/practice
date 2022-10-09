using System.Collections;
using System.Collections.Generic;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using UnityEngine;
using GameFramework;
namespace Farm.Hotfix
{
    public class GoatIdleState : EnemyIdleState
    {
        private readonly static int m_MoveBlend = Animator.StringToHash("MoveBlend");
        private readonly static int m_IsIdle = Animator.StringToHash("Isidle");
        private readonly static int m_IdleState = Animator.StringToHash("IdleState");
        private readonly static int m_IsUp = Animator.StringToHash("IsUp");
        
        AnimatorStateInfo info;
        //Õ£∂Ÿ ±º‰
        private float m_IdleTimeMax = 8f;  
        private float m_IdleTimeMin = 2f;
        private int allState = 3;
        private int inState;
        GameObject mainObject;

        WeregoatLogic weregoatLogic;
        private float inTimer;
        private float outTimer;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            
            base.OnEnter(procedureOwner);
            
            inTimer = Random.Range(m_IdleTimeMin, m_IdleTimeMax);
            mainObject = GameObject.Find("SK_Weregoat");
            if(mainObject!=null)
                weregoatLogic = mainObject.gameObject.transform.parent.GetComponent<WeregoatLogic>();
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            info = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
            outTimer += Time.deltaTime;
            if (weregoatLogic.IsLocking)
            {
                //owner.m_Animator.SetTrigger(m_IsUp);
                if (info.IsName("IdleSleep"))
                {
                    owner.m_Animator.SetTrigger(m_IsUp);
                    //ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
                }
                else
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            }
            else 
            {

                if (outTimer > inTimer)
                {

                    
                    //Debug.Log("∂Øª≠__" + info.IsName("IdleSleep"));
                    if (info.IsName("IdleSleep"))
                    {
                        if (outTimer > inTimer + 5f)
                        {
                            owner.m_Animator.SetTrigger(m_IsUp);
                            outTimer = 0;
                            inTimer = Random.Range(m_IdleTimeMin, m_IdleTimeMax);
                            inState = Utility.Random.GetRandom(0, allState);
                        }

                    }
                    else
                    {
                        inState = Utility.Random.GetRandom(0, allState);
                        owner.m_Animator.SetInteger(m_IdleState, inState);
                        outTimer = 0;
                        inTimer = Random.Range(m_IdleTimeMin, m_IdleTimeMax);
                    }
                }
            

                
               
                
            }
            

        }
        public static GoatIdleState Create()
        {
            GoatIdleState state = ReferencePool.Acquire<GoatIdleState>();
            return state;
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            owner.m_Animator.SetBool(m_IsIdle, false);
            owner.m_Animator.SetTrigger(m_IsUp);
            owner.m_Animator.SetInteger(m_IdleState, -1);
            outTimer = 0;
            base.OnLeave(fsm, isShutdown);
        }

    }
}
