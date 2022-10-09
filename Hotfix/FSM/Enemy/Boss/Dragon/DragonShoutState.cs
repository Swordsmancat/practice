using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    /// <summary>
    /// 黑龙吼叫(遇敌)状态
    /// </summary>
    public class DragonShoutState : EnemyShoutState
    {
        private readonly static int s_lockPlyer = Animator.StringToHash("LockPlyer");
        private readonly static int s_nearPlayer = Animator.StringToHash("NearPlayer");
        private readonly static int s_startRotateDistance = 10;         //开始旋转的距离
        private readonly static int s_downSpeed = 4;                    //下降速度
        private readonly static int s_timerToDown = 15;                 //触发下降的时间
        private readonly static int s_heightCheck = 5;                  //高度检测
        private readonly static float s_limitHeight = 0.5f;             //距离地面的极限高度
        private DragonLogic _owner;                                     //怪物逻辑
        private float _currentDistance;                                 //当前怪物与玩家之间的距离
        private float _downTimer;                                       //下降计时器

        protected override void EnterShoutState(IFsm<EnemyLogic> procedureOwner)
        {
            _owner = procedureOwner.Owner as DragonLogic;
            _owner.UnLockEntity();
            _owner.m_Animator.SetBool(s_lockPlyer, true);
            _owner.HurtState = HurtStateType.FlyIdleHurt;
        }

        protected override void UpdateShoutState(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            //更新玩家位置
            _owner.SetSearchTarget(_owner.find_Player.transform);
            _currentDistance = AIUtility.GetDistanceNoYAxis(_owner.find_Player.transform.position, _owner.transform.position);
            _downTimer += elapseSeconds;

            if(_downTimer >= s_timerToDown)
            {
                //下降
                AIUtility.RotateToTarget(_owner.find_Player, _owner, -10, 10, 1, 200);
                AIUtility.SmoothMove(_owner.transform.position,
                    _owner.transform.position + (-_owner.transform.up) +  (2 *_owner.transform.forward), 
                    _owner,
                    s_downSpeed);

                //指定高度切换动作
                if(_owner.CurrentHight >= s_heightCheck && _owner.m_Animator.GetBool(s_nearPlayer) == false)
                {
                    _owner.m_Animator.SetBool(s_nearPlayer, true);
                }
            }
            else
            {
                //在黑龙逻辑中维护此变量
                //到达最大高度后不在上升
                if (_owner.IsCanUp)
                {
                    //向上移动
                    AIUtility.SmoothMove(_owner.transform.position,
                        _owner.transform.position + _owner.transform.up,
                        _owner,
                        _owner.enemyData.MoveSpeedRun);
                }

                //由动画事件触发是否可向前移动
                if (_owner.IsCanForward)
                {
                    //向前移动
                    AIUtility.SmoothMove(_owner.transform.position,
                        _owner.transform.position + _owner.transform.forward,
                        _owner,
                        _owner.enemyData.MoveSpeedRun * 2);
                }

                //当前距离大于旋转距离时开始旋转
                if (_currentDistance >= s_startRotateDistance)
                {
                    //看向玩家
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
            //重置所有资源
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

