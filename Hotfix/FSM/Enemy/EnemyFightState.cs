using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;


namespace Farm.Hotfix
{
    public class EnemyFightState : EnemyBaseActionState
    {
        private readonly static float WaitTime = 0.5f;  //等待时间
        protected EnemyLogic owner;
        private float TimeDoChangeState = 0;
        private int _rotateSpeed = 3;

        protected enum AvoidType
        {
            back,
            left,
            right,
            front
        }
        

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.SetRichAiStop();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            //战斗状态下敌人面向敌人
            if (!owner.IsWeak)
            {
                AIUtility.RotateToTarget(owner.find_Player, owner, -10, 10);
                float disdance = AIUtility.GetDistance(owner, owner.find_Player);
                if (!owner.CheckInAttackRange(disdance))
                {
                   TimeDoChangeState += elapseSeconds;
                    if (TimeDoChangeState >= WaitTime)
                    {
                        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
                        TimeDoChangeState = 0;
                    
                    }
                }
                else
                {
                    InAttackRange(procedureOwner);
                }
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.IsAnimPlayed = false;
            TimeDoChangeState = 0;
        }

        /// <summary>
        /// 攻击范围内
        /// </summary>
        protected virtual void InAttackRange(ProcedureOwner fsm)
        {
            
            if (owner.IsCanAttack)
            {
                //float angle = AIUtility.GetAngleInSeek(owner, owner.find_Player);

                //if (owner.CheckInSeekAngle(angle))
                //Debug.Log("基础战斗--攻击");
                ChangeState(fsm, owner.ChangeStateEnemy(EnemyStateType.Attack));
            }
        }

        public static EnemyFightState Create()
        {
            EnemyFightState state = ReferencePool.Acquire<EnemyFightState>();
            return state;
        }



        protected void SetRotateSpeed(int speed)
        {
            _rotateSpeed = speed;
        }

    }
}

