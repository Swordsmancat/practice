using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class EnemyRandomMoveState : FsmState<EnemyLogic>, IReference
    {
        //退出时间、旋转角度、限制范围
        private readonly float EXIT_TIME = 3f;
        private readonly float ROTATE_TO_BACK = -180f;
        private readonly float ROTATE_TO_RIGHT = 90f;
        private readonly float ROTATE_TO_LEFT = -90f;
        private readonly float ROTATE_TO_FORWARD = 0f;
        //这里将范围看做一个10*10的矩形
        private readonly float LIMIT_AREA = 10f;


        private EnemyLogic owner;
        private float exitTimer = 0;

        //方向枚举
        private enum dir
        {
            forward,
            back,
            right,
            left
        };

        protected override void OnEnter(ProcedureOwner fsm)
        {
            base.OnEnter(fsm);
            owner = fsm.Owner;
            var entity = owner.gameObject;

            switch (Utility.Random.GetRandom(0,4))
            {
                case (int)dir.forward:
                    MoveDirection(ROTATE_TO_FORWARD, entity);
                    break;
                case (int)dir.back:
                    MoveDirection(ROTATE_TO_BACK, entity);
                    break;
                case (int)dir.right:
                    MoveDirection(ROTATE_TO_RIGHT, entity);
                    break;
                case (int)dir.left:
                    MoveDirection(ROTATE_TO_LEFT, entity);
                    break;
            }
           
            Log.Info("敌人随机移动状态");
        }

        protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            //向某个方向移动3秒
            exitTimer += elapseSeconds;
            if(exitTimer >= EXIT_TIME)
            {
                ChangeState<OrcIdleState>(fsm);
                ChangeState<UndeadIdleState>(fsm);
            }
            else
            {
                owner.m_Animator.SetFloat("MoveBlend", 1f);
            }

        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            exitTimer = 0;
        }

        public static EnemyRandomMoveState Create()
        {
            EnemyRandomMoveState state = ReferencePool.Acquire<EnemyRandomMoveState>();
            return state;
        }

        public void Clear()
        {
            owner = null;
        }

        //检查当前方向是否可移动
        //检查方法为用射线判断,怪物下方是否有物体,怪物前方是否有障碍物
        //同时限制怪物只能在指定范围内移动
        private bool CheckCanMove(in float angle,in GameObject obj)
        {
            Vector3 dir = new Vector3(0f, angle, 0f);
            Vector3 pos = obj.gameObject.transform.position;

            return false;
        }

        private void MoveDirection(in float angle,in GameObject obj)
        {
            obj.transform.Rotate(0f, angle, 0f);
        }
    }
}

