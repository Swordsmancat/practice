using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System;
namespace Farm.Hotfix
{
    public class GoatLogic : EnemyLogic
    {
        GameObject mainObject;
        protected override void AddFsmState()
        {
            Debug.Log("����¼�");
            stateList.Add(GoatIdleState.Create());
            stateList.Add(GoatMotionState.Create());
            //stateList.Add(WeregoatAttackState.Create());
            stateList.Add(GoatDeadState.Create());
            stateList.Add(GoatAttackState.Create());
            //stateList.Add(WeregoatDeadState.Create());
            stateList.Add(EnemyRotateState.Create());
            mainObject = find_Player.m_Attacker;
        }
        public override Type ChangeStateEnemy(EnemyStateType stateType)
        {

            switch (stateType)
            {
                case EnemyStateType.Idle:
                    //BackWeaponOpen();
                    //Debug.Log("վ��");
                    return typeof(GoatIdleState);
                //return typeof(EnemyIdleState);
                case EnemyStateType.Motion:
                    //Debug.Log("�ƶ�");
                    //if (m_Animator.GetFloat(m_Posture)>0)
                    //{ BackWeaponClose(); }
                    return typeof(GoatMotionState);
                case EnemyStateType.Dead:
                    return typeof(GoatDeadState);
                case EnemyStateType.Attack:
                    return typeof(GoatAttackState);
                case EnemyStateType.Rotate:
                    return typeof(EnemyRotateState);
                default:
                    //return typeof(EnemyIdleState);
                    //BackWeaponOpen();
                    return typeof(GoatIdleState);
            }
        }

        protected override void StartFsm()
        {
            //
            //fsm.Start<EnemyIdleState>();
            fsm.Start<GoatIdleState>();

        }
    }

//    AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo();
//    �ж϶����Ƿ񲥷����
//         if (info.normalizedTime >= 1.0f)
//         {
//             DoSomething();
//             }
//}
    
}
