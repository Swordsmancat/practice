using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    /// <summary>
    /// 敌人脱战状态
    /// </summary>
    public class EnemyOutOfTheFight : EnemyBaseActionState
    {
        private readonly static float minDistance = 2f;
        private Quaternion m_MyQuaternion;
        private EnemyLogic owner;

        protected override void OnInit(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.enemyData.HP = owner.enemyData.MaxHP;
           // Debug.Log("进入脱战状态");
        }

        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
           // Debug.Log("脱战"+ owner.CurrentTargetDisdance + "----" + minDistance);
            if (owner.CurrentTargetDisdance <= minDistance)
            {
               // Debug.Log("站立状态");
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Idle));
                owner.EnemyAttackEnd();
            } 
            else
            {
                owner.SetRichAIMove();
                owner.SetSearchTarget(owner.OriginPoint);
              
            }
        }

        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.transform.rotation = owner.OriginRotate;
        }

        public static EnemyOutOfTheFight Create()
        {
            EnemyOutOfTheFight state = ReferencePool.Acquire<EnemyOutOfTheFight>();
            return state;
        }

    }
}

