using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerKnockedDownState : PlayerBaseState
    {
        private PlayerLogic owner;
        private readonly string Layer = "Base Layer";
        private static readonly int KnockedDown = Animator.StringToHash("KnockedDown");
        private static readonly int DownRevolt = Animator.StringToHash("DownRevolt");
        private int DownRevoltNum = 2;
        private float outTime = 2f;
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
            owner.Buff.BuffTypeEnum = BuffType.None;
            owner.m_AnimationEventGetUp = false;
            owner.m_moveBehaviour.IsKnockLock(true);
            //_isAim = owner.m_LockEnemy;
            //if (owner.m_LockEnemy)
            //{
            //    owner.m_LockEnemy = false;
            //    owner.OnSetLock();
            //}
            //Log.Info("击倒状态");
            if (num > DownRevoltNum)
            {
                owner.m_Animator.SetTrigger(KnockedDown);

            }
            else 
            {
                owner.m_Animator.SetTrigger(DownRevolt);
                //站住
                //Log.Info("站住");
            }
            owner.HideTrail();//角色倒地 关闭拖尾和攻击检测 防止怪物碰到角色武器会一直受伤
            owner.AttackEnd();//同上
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
           // Log.Info( "方向" + owner.m_moveBehaviour.m_Horizontal +" " + owner.m_moveBehaviour.m_Vertical);
            if (owner.m_moveBehaviour.m_Horizontal != 0 || owner.m_moveBehaviour.m_Vertical != 0)
            {
                //Log.Info("切换翻滚");
                //owner.m_Animator.SetTrigger(Dodge);
                ChangeState<PlayerDodgeState>(procedureOwner);
            }

            currentTime += Time.deltaTime;
            //owner.transform.Rotate (owner.transform.rotation.x,0,owner.transform.rotation.z);
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

        public static PlayerKnockedDownState Create()
        {
            PlayerKnockedDownState state = ReferencePool.Acquire<PlayerKnockedDownState>();
            return state;
        }

    }
}
