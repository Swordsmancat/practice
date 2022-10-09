using UnityEngine;
using System;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    
    public class DragonideIdleState : EnemyIdleState
    {
        //protected EnemyLogic owner;
        private float idletime;
        //private readonly static int m_FightBlend = Animator.StringToHash("FightBlend");
        private readonly static int m_MoveBlend = Animator.StringToHash("MoveBlend");
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner.m_Animator.SetFloat("MoveBlend", 0f);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            owner.RestoreEnergy();
            idletime += Time.deltaTime;
            if(idletime > 20f)
            DragonideIdleStateStart(owner);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }
        public static new DragonideIdleState Create()
        {
            DragonideIdleState state = ReferencePool.Acquire<DragonideIdleState>();
            return state;
        }
        protected override void LockPlayerDo(ProcedureOwner procedureOwner)
        {
            owner.LockEntity(owner.find_Player);
            //Debug.Log("嘲讽");
            owner.m_Animator.SetTrigger("Shout");
            //测试音频
            //GameEntry.Sound.PlaySound(26020);
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Shout));
            //owner.m_Animator.SetFloat(m_MoveBlend, 0f);
            //owner.m_Animator.SetFloat(m_FightBlend, 0.5f);


        }
        protected  void DragonideIdleStateStart(EnemyLogic owner)
        {
            int num = Utility.Random.GetRandom(15, 30);
            


            //Debug.Log("时间数" + idletime);
            //Debug.Log("随机数" + num);
            if (idletime > num)
            {
                //Debug.Log("张望");
                owner.m_Animator.SetTrigger("Idle1");
                idletime = 0;

            }
            else
            {
                //Debug.Log("捶肩");
                owner.m_Animator.SetTrigger("Idle2");
                idletime = 0;
            }
                    
                

            if (owner.find_Player.IsDead)
            {
                owner.m_Animator.SetInteger("Idle", 3);
            }
        }
        
    }
}
