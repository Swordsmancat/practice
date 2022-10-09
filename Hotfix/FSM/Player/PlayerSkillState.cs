using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerSkillState : PlayerBaseActionState
    {
        private PlayerLogic owner;

        private float invincibleTime = 8f;
        private float invincibleFrame = 0f;

        private static readonly int SkillTrigger = Animator.StringToHash("SkillTrigger");
        private static readonly int m_ClickAttackBtnDuration = Animator.StringToHash("ClickAttackBtnDuration"); //按下攻击键的持续时间
        private static readonly int IsFourthAtk = Animator.StringToHash("IsFourthAtk");                         //是否是技能攻击
        private static readonly int ThumpNum = Animator.StringToHash("ThumpNum");                               //第几个技能
        private static readonly int IsSecondSkill = Animator.StringToHash("IsSecondSkill");                     //是否激活后续技能 目前用于大剑冲刺后在按技能按钮接过肩斩等
        private static readonly int SkillLev = Animator.StringToHash("SkillLev");                               //当前播放的技能等级
        private static readonly int AttackCount = Animator.StringToHash("AttackCount");                         //记录普攻的第几招 用于蛮力肩撞技能后点击攻击键可触发下一招普攻等
        private static readonly int SkillStateTime = Animator.StringToHash("SkillStateTime");                   //当前技能播放的时间 用于退出当前状态

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            owner.m_Animator.SetBool(IsSecondSkill, false) ;
            owner.m_Animator.SetBool(IsFourthAtk, owner.isFourthAtk);
            owner.m_Animator.SetFloat(SkillStateTime, 0);
            if (owner.m_Animator.GetInteger(ThumpNum) != 12)
            {
                owner.m_moveBehaviour.IsSkill(true);
            }
            if (owner.m_Animator.GetInteger(ThumpNum) is 11)
            {
                owner.isSpecialAtk = true;
            }
            else
            {
                owner.isSpecialAtk = false;
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            invincibleFrame += Time.deltaTime;
            if (invincibleFrame >= invincibleTime)
            {
                ChangeState<PlayerMotionState>(procedureOwner);
            }
            ////瞄准敌人
            //if (owner.currentLockEnemy && owner.isAim)
            //{
            //    owner.transform.LookAt(owner.currentLockEnemy.LockTransform, Vector3.up);
            //}

            owner.m_Animator.SetFloat(SkillStateTime, Mathf.Repeat(owner.m_Animator.GetCurrentAnimatorStateInfo(3).normalizedTime, 1));

            owner.m_Animator.SetTrigger(SkillTrigger);

            //大剑状态使用蛮力突刺后再次点击技能键 实现特殊技能 把剑插在敌人身上摔
            if (owner.m_Animator.GetInteger(ThumpNum) == 8 
                && Input.GetKeyDown(KeyCode.E) 
                && owner.m_Animator.GetCurrentAnimatorStateInfo(3).normalizedTime > 0.4
                && owner.EquiState ==EquiState.GiantSword)
            {
                owner.isSecondSkill = true;
            }
            if (owner.isSecondSkill)
            {
                owner.m_Animator.ResetTrigger(SkillTrigger);
                owner.isFourthAtk = false;
                ChangeState<PlayerSecondSkillState>(procedureOwner);
            }

            //大剑状态下使用蛮力肩撞再点击攻击可衔接一招普攻
            if (owner.m_Animator.GetCurrentAnimatorStateInfo(3).IsTag("Skill") 
                && owner.m_Animator.GetCurrentAnimatorStateInfo(3).IsName("GreatSword_蛮力肩撞9-1")
                && InputManager.IsClickDownMouseLeft())
            {
                if (owner.attackCount == 3)
                {
                    owner.m_Animator.SetInteger(AttackCount, 1);
                }
                else
                {
                    owner.m_Animator.SetInteger(AttackCount, owner.attackCount + 1);
                }
                owner.isFourthAtk = false;
                ChangeState<PlayerAttackState>(procedureOwner);
            }

            //判断是否到了切换状态的时间点
            if (owner.m_Animator.GetCurrentAnimatorStateInfo(3).IsTag("Skill"))
            {
                ChangeState(procedureOwner);
            }

            //为了释放技能后接翻滚更流畅
            if (InputManager.IsClickDodge() && owner.m_Animator.GetCurrentAnimatorStateInfo(3).normalizedTime > 0.45f)
            {
                ChangeState<PlayerDodgeState>(procedureOwner);
            }

        }

        //角色释放技能后切换到普通状态的ExitTime 不同的技能Exit的时间不同 如果不处理会出现角色技能释放后会短暂罚站
        //角色不同技能的退出时间记录在WeaponInfo脚本的SkillInfo类中 通过ThumpNum和SkillLev来找到当前动画的ExitTime
        private void ChangeState(ProcedureOwner procedureOwner)
        {
            switch (owner.EquiState)
            {
                case EquiState.SwordShield://剑盾
                    ExitCurrentState(owner.m_SkillInfo.skillDir[WeaponEnum.SwordShield][owner.m_Animator.GetInteger(ThumpNum) - 1].m_SkillLevel[owner.m_Animator.GetInteger(SkillLev) - 1].exitTime,procedureOwner);
                    break;
                case EquiState.GiantSword://大剑
                    ExitCurrentState(owner.m_SkillInfo.skillDir[WeaponEnum.GiantSword][owner.m_Animator.GetInteger(ThumpNum) - 1].m_SkillLevel[owner.m_Animator.GetInteger(SkillLev) - 1].exitTime, procedureOwner);
                    break;
                case EquiState.Dagger://短刀
                    ExitCurrentState(owner.m_SkillInfo.skillDir[WeaponEnum.Dagger][owner.m_Animator.GetInteger(ThumpNum) - 1].m_SkillLevel[owner.m_Animator.GetInteger(SkillLev) - 1].exitTime, procedureOwner);
                    break;
                case EquiState.DoubleBlades://双剑
                    ExitCurrentState(owner.m_SkillInfo.skillDir[WeaponEnum.DoubleBlades][owner.m_Animator.GetInteger(ThumpNum) - 1].m_SkillLevel[owner.m_Animator.GetInteger(SkillLev) - 1].exitTime, procedureOwner);
                    break;
                case EquiState.Pistol://远程
                    ExitCurrentState(owner.m_SkillInfo.skillDir[WeaponEnum.Pistol][owner.m_Animator.GetInteger(ThumpNum) - 1].m_SkillLevel[owner.m_Animator.GetInteger(SkillLev) - 1].exitTime, procedureOwner);
                    break;
                default:
                    break;
            }
        }
        private void ExitCurrentState(float m_NormalizedTime, ProcedureOwner m_ProcedureOwner)
        {
            if (owner.m_Animator.GetCurrentAnimatorStateInfo(3).normalizedTime > m_NormalizedTime)
            {
                owner.isFourthAtk = false;
                ChangeState<PlayerMotionState>(m_ProcedureOwner);
            }
        }


        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.m_ClickAttackBtnDuration = 0;
            owner.isFourthAtk = false;
            owner.m_Animator.ResetTrigger(SkillTrigger);
            owner.m_Animator.SetBool(IsFourthAtk, owner.isFourthAtk);
            owner.m_moveBehaviour.IsSkill(false);
            owner.m_Animator.SetFloat(m_ClickAttackBtnDuration, owner.m_ClickAttackBtnDuration);
            invincibleFrame = 0;
            owner.isSpecialAtk = false;
        }

        public static PlayerSkillState Create()
        {
            PlayerSkillState state = ReferencePool.Acquire<PlayerSkillState>();
            return state;
        }

    }
}
