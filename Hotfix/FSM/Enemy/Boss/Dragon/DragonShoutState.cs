using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    /// <summary>
    /// �������(����)״̬
    /// </summary>
    public class DragonShoutState : EnemyShoutState
    {
        private readonly static int s_lockPlyer = Animator.StringToHash("LockPlyer");
        private readonly static int s_nearPlayer = Animator.StringToHash("NearPlayer");
        private readonly static int s_startRotateDistance = 10;         //��ʼ��ת�ľ���
        private readonly static int s_downSpeed = 4;                    //�½��ٶ�
        private readonly static int s_timerToDown = 15;                 //�����½���ʱ��
        private readonly static int s_heightCheck = 5;                  //�߶ȼ��
        private readonly static float s_limitHeight = 0.5f;             //�������ļ��޸߶�
        private DragonLogic _owner;                                     //�����߼�
        private float _currentDistance;                                 //��ǰ���������֮��ľ���
        private float _downTimer;                                       //�½���ʱ��

        protected override void EnterShoutState(IFsm<EnemyLogic> procedureOwner)
        {
            _owner = procedureOwner.Owner as DragonLogic;
            _owner.UnLockEntity();
            _owner.m_Animator.SetBool(s_lockPlyer, true);
            _owner.HurtState = HurtStateType.FlyIdleHurt;
        }

        protected override void UpdateShoutState(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            //�������λ��
            _owner.SetSearchTarget(_owner.find_Player.transform);
            _currentDistance = AIUtility.GetDistanceNoYAxis(_owner.find_Player.transform.position, _owner.transform.position);
            _downTimer += elapseSeconds;

            if(_downTimer >= s_timerToDown)
            {
                //�½�
                AIUtility.RotateToTarget(_owner.find_Player, _owner, -10, 10, 1, 200);
                AIUtility.SmoothMove(_owner.transform.position,
                    _owner.transform.position + (-_owner.transform.up) +  (2 *_owner.transform.forward), 
                    _owner,
                    s_downSpeed);

                //ָ���߶��л�����
                if(_owner.CurrentHight >= s_heightCheck && _owner.m_Animator.GetBool(s_nearPlayer) == false)
                {
                    _owner.m_Animator.SetBool(s_nearPlayer, true);
                }
            }
            else
            {
                //�ں����߼���ά���˱���
                //�������߶Ⱥ�������
                if (_owner.IsCanUp)
                {
                    //�����ƶ�
                    AIUtility.SmoothMove(_owner.transform.position,
                        _owner.transform.position + _owner.transform.up,
                        _owner,
                        _owner.enemyData.MoveSpeedRun);
                }

                //�ɶ����¼������Ƿ����ǰ�ƶ�
                if (_owner.IsCanForward)
                {
                    //��ǰ�ƶ�
                    AIUtility.SmoothMove(_owner.transform.position,
                        _owner.transform.position + _owner.transform.forward,
                        _owner,
                        _owner.enemyData.MoveSpeedRun * 2);
                }

                //��ǰ���������ת����ʱ��ʼ��ת
                if (_currentDistance >= s_startRotateDistance)
                {
                    //�������
                    AIUtility.RotateToTarget(_owner.find_Player, _owner, -10, 10, 1, 200);
                }
            }

            
            if(_owner.CurrentHight <= s_limitHeight && _owner.m_Animator.GetBool(s_nearPlayer) == true)
            {
                ChangeState(procedureOwner, _owner.ChangeStateEnemy(EnemyStateType.Motion));
            }

        }

        protected override void LeaveShoutState()
        {
            //����������Դ
            _owner.LockEntity(_owner.find_Player);
            _owner.m_Animator.SetBool(s_lockPlyer, false);
            _owner.m_Animator.SetBool(s_nearPlayer, false);
            _owner.IsCanForward = false;
            _owner.HurtState = HurtStateType.GroundIdleHurt;
            _owner.GroundSmoke();
        }

        public static new DragonShoutState Create()
        {
            DragonShoutState state = ReferencePool.Acquire<DragonShoutState>();
            return state;
        }
    }
}

