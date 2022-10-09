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
        private readonly float FRIST_PHASE = 0.7f;      // ��һ�׶ν��������ٷֱ�
        private readonly float SECOND_PHASE = 0.5f;     // �ڶ��׶ν��������ٷֱ�
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
                //����巢����û�л��������������״̬
                _owner.AvoidAttackStart();
                ChangeState<DragonFlyState>(fsm);
                return;
            }

            if(_owner.enemyData.HPRatio >= SECOND_PHASE && _owner.enemyData.HPRatio <= FRIST_PHASE)
            {
                //�ڶ��׶�(��Ҫ���ڵ���ս��)
                _owner.SetSearchTarget(_owner.find_Player.transform);
                if (_owner.CurrentTargetDisdance >= s_fireBallDistance + 2)
                {
                    //���򹥻�
                    _owner.AttackState = 3;
                }
                else
                {
                    //���
                    _owner.AttackState = 4;
                }
               
            }
            else if(_owner.enemyData.HPRatio <= SECOND_PHASE)
            {
                //�����׶�
                //����Ӧ���ú������е������ִ�п��й���
                //int num = Utility.Random.GetRandom(0, 10);
                //if(!_owner.IsFying)
                //{
                //    Debug.Log("�ı�״̬");
                //    _owner.AvoidAttackStart();
                //    ChangeState<DragonFlyState>(fsm);
                //    return;
                //}
            }
            else
            {
                //��һ�׶�
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

