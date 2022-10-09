using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;

namespace Farm.Hotfix
{
    public class PlayerDefenseState : PlayerBaseActionState
    {
        private PlayerLogic owner;
        private static readonly int HurtDefense = Animator.StringToHash("HurtDefense");
        private static readonly int Defense = Animator.StringToHash("Defense");
        private static readonly int DefenseOut = Animator.StringToHash("DefenseOut");
        private bool isOut;
        private bool OutHurt;
        private float currentMP;
        //private readonly string Layer = "Base Layer";


        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {

            base.OnEnter(procedureOwner);
            //Debug.Log("格挡状态");
            owner = procedureOwner.Owner;
            owner.m_Animator.SetBool(Defense, true);
            //Debug.Log("精力值:" + owner.PlayerData.MP);
            owner.Buff.BuffTypeEnum = BuffType.None;

        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (owner.underAttack)
            {
                DefenseHurt();
            }
            
            if (!owner.IsDefense)
            {
                ChangeState<PlayerMotionState>(procedureOwner);
            }
            if (isOut)
            {
                owner.m_Animator.SetTrigger(DefenseOut);
                
                ChangeState<PlayerMotionState>(procedureOwner);
                 
            }
            if (OutHurt) //owner.m_Animator.SetTrigger(DefenseOut);
            {
                //owner.m_Animator.SetTrigger(DefenseOut);
                ChangeState<PlayerHurtState>(procedureOwner); 
            }
            if (owner.m_Attack)
            {
                ChangeState<PlayerAttackState>(procedureOwner);
            }
            //ChangeState<>(procedureOwner);
        }
        private void DefenseHurt()
        {
            Debug.Log("受击状态"+owner.m_BuffType);
            switch (owner.m_BuffType)
            {
                case BuffType.None:
                    HurtState();
                    break;
                case BuffType.Tap:
                    HurtState();
                    break;
                case BuffType.Thump:
                    KnockedDownState();
                    break;
                case BuffType.Overwhelmed:
                    KnockedFlyState();
                    break;
                default:
                    HurtState();
                    break;
            }
            //owner.Buff.BuffTypeEnum = BuffType.None;
            owner.underAttack = false;
            owner.HideTrail();//角色格挡 关闭拖尾和攻击检测 防止怪物碰到角色武器会一直受伤
            owner.AttackEnd();//同上
            
        }
        private void HurtState()
        {
            MPCalculate(5, 15);
            if (owner.PlayerData.MP > 0)
            {
                owner.m_Animator.SetTrigger(HurtDefense);
            }
            else
            {
                isOut = true;
            }
        }
        private void KnockedDownState()
        {
            MPCalculate(15, 25);
            if (owner.PlayerData.MP > 0)
            {
                owner.m_Animator.SetTrigger(HurtDefense);
            }
            else
            {
                isOut = true;
            }
            
        }
        private void KnockedFlyState()
        {
            MPCalculate(25, 35);

            if (owner.PlayerData.MP > 0)
            {
                isOut = true;
            }
            else
            {
                OutHurt = true;
            }
            
        }

        private void MPCalculate(int minNum, int maxNum)
        {
            int energyLoss = Utility.Random.GetRandom(minNum, maxNum);
            currentMP = owner.PlayerData.MP - energyLoss;
            owner.PlayerData.MP = currentMP > 0 ? currentMP : 0;
            Debug.Log("当前精力" + currentMP);
            Debug.Log("总精力" + owner.PlayerData.MP);
        }
        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.Buff.BuffTypeEnum = BuffType.None;
            isOut = false;
            OutHurt = false;
            owner.IsDefense = false;
        }
        public static PlayerDefenseState Create()
        {
            PlayerDefenseState state = ReferencePool.Acquire<PlayerDefenseState>();
            return state;
        }

    }

}

