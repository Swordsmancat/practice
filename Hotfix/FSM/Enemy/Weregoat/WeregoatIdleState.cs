using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
namespace Farm.Hotfix
{
    public class WeregoatIdleState : EnemyIdleState
    {
        private readonly static int m_MoveBlend = Animator.StringToHash("MoveBlend");
        private readonly static int m_IsPatrol = Animator.StringToHash("IsPatrol");
        private readonly static int m_IsIdle = Animator.StringToHash("IsIdle");
        private readonly static int m_IsMove = Animator.StringToHash("IsMove");
        private readonly static int m_Posture = Animator.StringToHash("Posture");
        private readonly static int m_Idle = Animator.StringToHash("Idle");
        private readonly static int m_FightD = Animator.StringToHash("FightD");
        float timer = 0;
        float timeEnd = 20;
        float parryTimeEnd = 3;
        private WeregoatLogic me;
        AnimatorStateInfo info;



        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            //owner = procedureOwner.Owner as WeregoatLogic;
            //BackWeaponObject = FindTools.FindFunc(owner.transform, s_BackWeapon).gameObject;
            base.OnEnter(procedureOwner);
            Log.Info("Õ¾Á¢×´Ì¬");
            owner.m_Animator.SetFloat(m_MoveBlend, 0f);
            //owner.m_Animator.SetBool(m_IsPatrol, false);
            owner.m_Animator.SetBool(m_IsIdle, true);
            owner.m_Animator.SetBool(m_IsMove, false);
            owner.m_Animator.SetTrigger(m_Idle);
            me = owner as WeregoatLogic;
            //me.BackWeaponOpen();

            //owner.m_Animator.SetInteger(m_Posture, 0);
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            timer += Time.deltaTime;
            if (owner.Energy > 0)
            {
                base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
                owner.RestoreEnergy();
                info = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
                
                if (!me.BackWeapon.activeSelf && info.IsName("BackWeaponOpen") && info.normalizedTime > 0.5f)
                    me.BackWeaponOpen();
                if (timer > timeEnd)
                {
                    Log.Info("ÇÐ»»Ñ²Âß×´Ì¬");
                    owner.m_Animator.SetBool(m_IsPatrol, true);

                    //owner.m_Animator.SetBool(m_IsMove, true);
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
                    timer = 0;
                }
            }
            else
            {
                //timer += Time.deltaTime;
                if (timer > parryTimeEnd)
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            }
            
            
            

        }
        public static WeregoatIdleState Create()
        {
            WeregoatIdleState state = ReferencePool.Acquire<WeregoatIdleState>();
            return state;
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            owner.m_Animator.SetBool(m_IsIdle, false);
            if (owner.m_Animator.GetBool(m_IsPatrol))
                owner.m_Animator.SetFloat(m_Posture, 0);
            else
            {
                owner.m_Animator.SetFloat(m_Posture, 1);
                owner.m_Animator.SetTrigger(m_FightD);

            }
                
            timer = 0;
            base.OnLeave(fsm, isShutdown);
        }
    }
}
