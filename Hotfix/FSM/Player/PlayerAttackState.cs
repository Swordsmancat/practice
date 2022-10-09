using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerAttackState : PlayerBaseActionState
    {
        private PlayerLogic owner;
        private readonly float m_RotateSpeed = 2f;

        private float invincibleTime = 0.07f;
        private float invincibleFrame = 0f;
        private float dampTimeX = 0.1f;

        private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
        private static readonly int Dodge = Animator.StringToHash("Dodge");
        private static readonly int AttackTap = Animator.StringToHash("AttackTap");
        private static readonly int AttackJump = Animator.StringToHash("AttackJump");
        private static readonly int AttackThump = Animator.StringToHash("AttackThump");
        private static readonly int MoveBlend = Animator.StringToHash("MoveBlend");
        private static readonly int m_HashStateTime = Animator.StringToHash("StateTime");
        private static readonly int m_ClickAttackBtnDuration = Animator.StringToHash("ClickAttackBtnDuration");
        private static readonly int StepAtk = Animator.StringToHash("StepAtk");             //是否滑步 若为ture就执行特殊的滑步攻击 目前双剑有滑步
        //private static readonly int IsFourthAtk = Animator.StringToHash("IsFourthAtk");     
        private static readonly int ThumpNum = Animator.StringToHash("ThumpNum");
        private static readonly int DoubleClick = Animator.StringToHash("DoubleClick");
        private static readonly int Whirlwind = Animator.StringToHash("Whirlwind");


        //private static readonly int IsSecondSkill = Animator.StringToHash("IsSecondSkill");
        //private static readonly int AttackCount = Animator.StringToHash("AttackCount");


        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.m_Animator.SetFloat(m_ClickAttackBtnDuration, owner.m_ClickAttackBtnDuration);
            owner.m_Animator.SetFloat(m_HashStateTime, 0);
            if (owner.m_Animator.GetFloat(MoveBlend) > 1.8f)
            {
                //奔跑中抽刀并攻击时增加位移
                owner.PlayerShiftStartAnim(0.6f);
            }

              owner.m_moveBehaviour.isAttack = true;

        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            owner.m_Animator.SetFloat(MoveBlend, Mathf.Max(Mathf.Abs(owner.MoveX), Mathf.Abs(owner.MoveY)), dampTimeX, Time.deltaTime);
            
            ////瞄准敌人
            //if (owner.currentLockEnemy && owner.isAim)
            //{
            //    owner.transform.LookAt(owner.currentLockEnemy.LockTransform, Vector3.up);
            //}

            if (owner.currentLockEnemy)
            {
                owner.m_moveBehaviour.AttackRotating();
            }

            //实现普攻后能快速接翻滚
            //if (InputManager.IsClickDodge() && owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex("Base Layer")).normalizedTime > 0.4f)
            //{
            //    ChangeState<PlayerDodgeState>(procedureOwner);
            //}

            //实现预输入功能 提前输入打连招
            if (InputManager.IsClickDodge())
            {
                ChangeState<PlayerDodgeState>(procedureOwner);
            }
            if (InputManager.IsClickDownMouseRight())
            {
                ChangeState<PlayerAttackState>(procedureOwner);
            }
            if (InputManager.IsClickDownMouseLeft())
            {
                ChangeState<PlayerAttackState>(procedureOwner);
            }


            //角色滑步后实现特殊的攻击 目前只有双剑有滑步
            if (owner.isCanStepAtk)
            {
                owner.m_Animator.SetBool(StepAtk, true);
            }
            else
            {
                owner.m_Animator.SetBool(StepAtk, false);
                
            }

            //if (owner.isFourthAtk)
            //{
            //    owner.m_Animator.SetBool(IsFourthAtk, true);
            //    invincibleFrame += Time.deltaTime;
            //    if (owner.m_Animator.GetInteger(ThumpNum) == 8 && Input.GetKeyDown(KeyCode.E) && invincibleFrame > 1.2f)
            //    {
            //        owner.m_Animator.SetBool(IsSecondSkill, true);
            //    }
            //}
            //else
            //{
            //    owner.m_Animator.SetBool(IsFourthAtk, false);
            //}
            //3是BaseLayer层
            owner.m_Animator.SetFloat(m_HashStateTime, Mathf.Repeat(owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex("Base Layer")).normalizedTime, 1f));
            owner.m_Animator.ResetTrigger(AttackTrigger);
            owner.m_Animator.ResetTrigger(AttackTap);
            owner.m_Animator.ResetTrigger(AttackThump);
            owner.m_Animator.ResetTrigger(AttackJump);
            owner.m_Animator.ResetTrigger(DoubleClick);
            if (owner.m_Attack)
            {
                owner.m_Animator.SetTrigger(AttackTrigger);
                //攻击转向
               
                owner.m_moveBehaviour.AttackRotating();
                // Player is moving on ground, Y component of camera facing is not relevant.
                if (owner.m_DoubleClick)
                {
                    owner.m_Animator.SetTrigger(DoubleClick);
                    if (owner.m_Animator.GetCurrentAnimatorStateInfo(3).normalizedTime > 0.3f)
                    {
                        
                    }
                }
                else if (owner.m_IsAttackTap)
                {
                    if (owner.m_Animator.GetFloat(MoveBlend) < 1f)
                    {
                        owner.m_Animator.SetTrigger(AttackTap);
                    }
                }
                else if (owner.m_IsAttackThump)
                {
                    owner.m_Animator.SetTrigger(AttackThump);
                }
                else if (owner.m_IsAttackJump)
                {
                    if (owner.m_Animator.GetFloat(MoveBlend) > 0.6f)
                    {
                        owner.m_Animator.SetTrigger(AttackTap);
                    }
                    
                }
                
                //   owner.gameObject.transform.rotation = Quaternion.Euler(0,
                //      owner.CachedTransform.localRotation.eulerAngles.y + owner.MoveX, 0);
            }
            else
            {
                if (owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex("Base Layer")).normalizedTime > 0 && owner.m_Animator.GetAnimatorTransitionInfo(owner.m_Animator.GetLayerIndex("Base Layer")).nameHash !=0)
                {
                    ChangeState<PlayerMotionState>(procedureOwner);
                }
                if(owner.EquiState == EquiState.Pistol)
                {
                    if (owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex("Player")).normalizedTime > 0 && owner.m_Animator.GetAnimatorTransitionInfo(owner.m_Animator.GetLayerIndex("Player")).nameHash != 0)
                    {
                        ChangeState<PlayerMotionState>(procedureOwner);
                    }
                }
            }
            
            //实现大剑普攻状态下衔接9.蛮力肩撞
            if (Input.GetKeyDown(KeyCode.E)
                && owner.m_Animator.GetInteger(ThumpNum) == 9 
                && owner.m_Animator.GetCurrentAnimatorStateInfo(3).normalizedTime > 0.5
                && owner.EquiState == EquiState.GiantSword)
            {
                ChangeState<PlayerSkillState>(procedureOwner);
            }
           
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            invincibleFrame = 0;
            owner.m_Attack = false;
            owner.m_IsAttackTap = false;
            owner.m_IsAttackThump = false;
            owner.m_IsAttackJump = false;
            owner.Whirlwind = false;
            owner.m_ClickAttackBtnDuration = 0;
            owner.m_moveBehaviour.isAttack = false;
            //owner.m_Animator.ResetTrigger(Dodge);
            //owner.isFourthAtk = false;
            //owner.m_Animator.SetBool(IsFourthAtk, owner.isFourthAtk);
            //owner.m_Animator.SetBool(IsSecondSkill, false);
        }

        public static PlayerAttackState Create()
        {
            PlayerAttackState state = ReferencePool.Acquire<PlayerAttackState>();
            return state;
        }

    }
}
