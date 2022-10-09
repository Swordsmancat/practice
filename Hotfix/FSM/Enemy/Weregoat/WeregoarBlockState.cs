using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace Farm.Hotfix
{
    /// <summary>
    /// ÆÆ·À/Òì³£×´Ì¬
    /// </summary>
    public class WeregoarBlockState : EnemyBlockState
    {
        private float parryOutEndTime = 3f;
        private float timer;
        private EnemyLogic owner;
        //private static int m_InParryOut = Animator.StringToHash("InParryOut");
        private static int m_IsDown = Animator.StringToHash("IsDown");
        private static int m_IsUp = Animator.StringToHash("IsUp");
        private static int m_DebuffState = Animator.StringToHash("DebuffState");
        protected override void OnEnter(IFsm<EnemyLogic> fsm)
        {
            base.OnEnter(fsm);
            owner = fsm.Owner;
            int debuffStateNum = Utility.Random.GetRandom(0, 2);
            owner.m_Animator.SetInteger(m_DebuffState, debuffStateNum);
            owner.m_Animator.SetTrigger(m_IsDown);
            //owner.m_Animator.SetBool(m_InParryOut, true);
            owner.SetRichAiStop();
        }
        protected override void OnUpdate(IFsm<EnemyLogic> fsm, float elapseSeconds, float realElapseSeconds)
        {
            timer += Time.deltaTime;
            
            if (timer > parryOutEndTime)
            {
                Debug.Log("Ê±¼ä" + timer);
                int debuffStateNum = Utility.Random.GetRandom(0, 2);
                owner.m_Animator.SetInteger(m_DebuffState, debuffStateNum);
                owner.m_Animator.SetTrigger(m_IsUp);
                
                

                ChangeState(fsm, owner.ChangeStateEnemy(EnemyStateType.Motion));
            
            }
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }
        public static WeregoarBlockState Create()
        {
            WeregoarBlockState state = ReferencePool.Acquire<WeregoarBlockState>();
            return state;
        }
        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            timer = 0;
            //owner.m_Animator.SetBool(m_InParryOut, false);
            owner.Energy = 100;
        }
    }
}
