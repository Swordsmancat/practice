using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    public class DragonFightState : EnemyFightState
    {
        private readonly static int s_rotateSpeed = 1;
        private readonly static int s_fireBallDistance = 6;
        private readonly float FRIST_PHASE = 0.7f;      // 第一阶段结束生命百分比
        private readonly float SECOND_PHASE = 0.5f;     // 第二阶段结束生命百分比
        private DragonLogic _owner;
        



        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            _owner = procedureOwner.Owner as DragonLogic;
            SetRotateSpeed(s_rotateSpeed);
        }




        protected override void InAttackRange(IFsm<EnemyLogic> fsm)
        {   
            if(_owner.FireBallCount > 5)
            {
                //如果五发火球都没有击中玩家则进入飞行状态
                _owner.AvoidAttackStart();
                ChangeState<DragonFlyState>(fsm);
                return;
            }

            if(_owner.enemyData.HPRatio >= SECOND_PHASE && _owner.enemyData.HPRatio <= FRIST_PHASE)
            {
                //第二阶段(主要是在地面战斗)
                _owner.SetSearchTarget(_owner.find_Player.transform);
                if (_owner.CurrentTargetDisdance >= s_fireBallDistance + 2)
                {
                    //火球攻击
                    _owner.AttackState = 3;
                }
                else
                {
                    //喷火
                    _owner.AttackState = 4;
                }
               
            }
            else if(_owner.enemyData.HPRatio <= SECOND_PHASE)
            {
                //第三阶段
                //这里应该让黑龙飞行到天空中执行空中攻击
                //int num = Utility.Random.GetRandom(0, 10);
                //if(!_owner.IsFying)
                //{
                //    Debug.Log("改变状态");
                //    _owner.AvoidAttackStart();
                //    ChangeState<DragonFlyState>(fsm);
                //    return;
                //}
            }
            else
            {
                //第一阶段
                _owner.SetSearchTarget(_owner.find_Player.transform);
                if (_owner.CurrentTargetDisdance >= s_fireBallDistance)
                {
                    _owner.AttackState = 3;
                }
                else
                {
                    _owner.AttackState = Utility.Random.GetRandom(0, 3);
                }
            }
            base.InAttackRange(fsm);


        }






        public static new DragonFightState Create()
        {
            DragonFightState state = ReferencePool.Acquire<DragonFightState>();
            return state;
        }
    }
}

