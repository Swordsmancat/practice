using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    public class DragonFlyState : EnemyBaseActionState
    {
        private readonly static int s_flying = Animator.StringToHash("Flying");
        private readonly static int s_nearPlayer = Animator.StringToHash("NearPlayer");
        private readonly static float s_exitTime = 10;
        private readonly static float s_limitHeight = 0.2f;
        private readonly static float s_downSpeed = 4f;
        private DragonLogic owner;
        private float _currentDistance;
        private float _timer;

        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner as DragonLogic;
            owner.m_Animator.SetBool(s_flying, true);
            owner.HurtState = HurtStateType.FlyIdleHurt;
            owner.UnLockEntity();
            owner.IsFying = true;
            owner.FireBallCount = 0;
            //owner.IsAvoid = true;
            Debug.Log("进入飞行状态");
        }

        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            
            owner.SetSearchTarget(owner.find_Player.transform);
            _currentDistance = AIUtility.GetDistanceNoYAxis(owner.find_Player.transform.position, owner.transform.position);
            _timer += elapseSeconds;
            AIUtility.RotateToTarget(owner.find_Player, owner, -10, 10, 1, 200);

            if (owner.IsCanUp == true
                && owner.m_Animator.GetBool(s_flying) == true)
            {
                //up
                if(_timer >= 1)
                {
                    AIUtility.SmoothMove(owner.transform.position,
                            owner.transform.position + owner.transform.up,
                            owner,
                            owner.enemyData.MoveSpeedRun);
                }
            }
            

            if(owner.IsCanForward)
            {
                // forward
                AIUtility.SmoothMove(owner.transform.position,
                    owner.transform.position + owner.transform.forward,
                    owner,
                    owner.enemyData.MoveSpeedRun * 2);
            }

            if(_timer >= s_exitTime)
            {
                if(owner.m_Animator.GetBool(s_flying) == true)
                {
                    owner.m_Animator.SetBool(s_flying, false);
                    owner.m_Animator.SetBool(s_nearPlayer, true);
                }
                // down
                AIUtility.SmoothMove(owner.transform.position,
                    owner.transform.position + (-owner.transform.up),
                    owner,
                    s_downSpeed);
            }

            if(owner.CurrentHight <= s_limitHeight && owner.m_Animator.GetBool(s_flying) == false)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            }

        }

        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            Debug.Log("离开飞行状态");
            owner.m_Animator.SetBool(s_nearPlayer, false);
            owner.m_Animator.SetBool(s_flying, false);
            owner.LockEntity(owner.find_Player);
            owner.IsCanForward = false;
            owner.HurtState = HurtStateType.GroundIdleHurt;
            owner.GroundSmoke();
            _timer = 0;
        }


        public static DragonFlyState Create()
        {
            DragonFlyState state = ReferencePool.Acquire<DragonFlyState>();
            return state;
        }
    }

}
