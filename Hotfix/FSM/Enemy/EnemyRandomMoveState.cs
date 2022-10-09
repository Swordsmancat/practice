using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class EnemyRandomMoveState : FsmState<EnemyLogic>, IReference
    {
        //�˳�ʱ�䡢��ת�Ƕȡ����Ʒ�Χ
        private readonly float EXIT_TIME = 3f;
        private readonly float ROTATE_TO_BACK = -180f;
        private readonly float ROTATE_TO_RIGHT = 90f;
        private readonly float ROTATE_TO_LEFT = -90f;
        private readonly float ROTATE_TO_FORWARD = 0f;
        //���ｫ��Χ����һ��10*10�ľ���
        private readonly float LIMIT_AREA = 10f;


        private EnemyLogic owner;
        private float exitTimer = 0;

        //����ö��
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
           
            Log.Info("��������ƶ�״̬");
        }

        protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            //��ĳ�������ƶ�3��
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

        //��鵱ǰ�����Ƿ���ƶ�
        //��鷽��Ϊ�������ж�,�����·��Ƿ�������,����ǰ���Ƿ����ϰ���
        //ͬʱ���ƹ���ֻ����ָ����Χ���ƶ�
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

