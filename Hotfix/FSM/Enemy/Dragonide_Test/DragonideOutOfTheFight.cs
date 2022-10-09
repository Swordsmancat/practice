using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class DragonideOutOfTheFight : EnemyOutOfTheFight
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
            Debug.Log("进入脱战状态");

        }

        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            owner.RestoreEnergy();
            //Debug.Log("脱战" + owner.CurrentTargetDisdance + "----" + minDistance);
            if (owner.CurrentTargetDisdance <= minDistance)
            {
                Debug.Log("站立状态");
                owner.m_Animator.SetTrigger("isIdle");
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Idle));
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
        public static DragonideOutOfTheFight Create()
        {
            DragonideOutOfTheFight state = ReferencePool.Acquire<DragonideOutOfTheFight>();
            return state;
        }
    }

}
