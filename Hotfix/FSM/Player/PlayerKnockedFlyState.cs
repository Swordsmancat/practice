using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerKnockedFlyState : PlayerBaseState
    {
        private PlayerLogic owner;

        private readonly string Layer = "Base Layer";
        private static readonly int KnockedFly = Animator.StringToHash("KnockedFly");
        private static readonly int RandomFly = Animator.StringToHash("RandomFly");
        private static readonly int FlyRevolt = Animator.StringToHash("FlyRevolt");
        private float outTime = 3f;
        private float currentTime;
        //private bool _isAim;
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            int num = Random.Range(0, 10);
            owner = procedureOwner.Owner;
            owner.isKnockedDown = true;
            //_isAim = owner.m_LockEnemy;
            //if (owner.m_LockEnemy)
            //{
            //    owner.m_LockEnemy = false;
            //    owner.OnSetLock();
            //}
            owner.Buff.BuffTypeEnum = BuffType.None;
            if (num > 2)
            {
                owner.m_Animator.SetInteger(RandomFly, Utility.Random.GetRandom(0, 4));
                owner.m_AnimationEventGetUp = false;
                owner.m_Animator.SetTrigger(KnockedFly);
            }
            else 
            {
                owner.m_Animator.SetTrigger(FlyRevolt);
            }
            owner.m_moveBehaviour.IsKnockLock(true);//防止角色倒地后 按方向键会转动
            owner.HideTrail();//角色倒地 关闭拖尾和攻击检测 防止怪物碰到角色武器会一直受伤
            owner.AttackEnd();//同上
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (owner.m_moveBehaviour.m_Horizontal != 0 || owner.m_moveBehaviour.m_Vertical != 0)
            {
                //Log.Info("切换翻滚");
                //owner.m_Animator.SetTrigger(Dodge);
                ChangeState<PlayerDodgeState>(procedureOwner);
            }
            //owner.transform.right = 
            owner.transform.Rotate(owner.transform.rotation.x, 0, owner.transform.rotation.z);
            currentTime += Time.deltaTime;
            if (currentTime > outTime)
            {
                owner.GetUpAnimationEvent();
                currentTime = 0;
            }
            if (owner.m_AnimationEventGetUp)
            {
                ChangeState<PlayerIdleState>(procedureOwner);
            }
            //if (owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex(Layer)).normalizedTime >= 0.9f)
            //{
            //    ChangeState<PlayerGetUpState>(procedureOwner);
            //}

        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            //owner.isAim = _isAim;
            //owner.OnSetLock();
            owner.isKnockedDown = false;
            owner.m_moveBehaviour.IsKnockLock(false);
        }

        public static PlayerKnockedFlyState Create()
        {
            PlayerKnockedFlyState state = ReferencePool.Acquire<PlayerKnockedFlyState>();
            return state;
        }

    }
}
