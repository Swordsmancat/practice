using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;

namespace Farm.Hotfix
{
    public class Old_HobgoblinHurtState : EnemyHurtState
    {
        private static readonly int CanAvoid = Animator.StringToHash("CanAvoid");
        int Hurt_Wind = 0;
        private float currentEnergy;
        protected static readonly int AvoidBacknum = Animator.StringToHash("AvoidBacknum");
        protected static readonly int AvoidBack = Animator.StringToHash("AvoidBack");
        protected static readonly int WeakHurt = Animator.StringToHash("WeakHurt");

        //private HobgoblinLogic me;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            //me = owner as HobgoblinLogic;
            Hurt_Wind++;
            ParryHurt();


            if (!owner.IsWeak && Hurt_Wind >= 3)
            {
                Hurt_Wind = 0;
                //owner.m_Animator.SetInteger(AvoidBacknum, 0);
                //owner.m_Animator.SetTrigger(AvoidBack);
                //owner.m_Animator.SetBool(CanAvoid, true);
                //Debug.Log("Change_Wind" + Hurt_Wind);
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
            }

        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }
        
        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.m_Animator.SetInteger(WeakHurt, 0);
            //Debug.Log("N_Wind" + Hurt_Wind);
        }

        public static new Old_HobgoblinHurtState Create()
        {
            Old_HobgoblinHurtState state = ReferencePool.Acquire<Old_HobgoblinHurtState>();
            return state;
        }
        protected override void EnemyHurtStateStart(ProcedureOwner fsm)
        {
            if (owner.find_Player.isThump)
            {
                owner.m_Animator.SetTrigger(HurtThump);
            }
            else
            {
                owner.m_Animator.SetTrigger(Hurt);
            }
            owner.m_Animator.SetBool(CanAvoid, false);
        }
        private void EnergyCalculate(int minNum, int maxNum)
        {
            int energyLoss = Utility.Random.GetRandom(minNum, maxNum);
            currentEnergy = owner.Energy - energyLoss;
            owner.Energy = currentEnergy > 0 ? currentEnergy : 0;
            //Debug.Log("当前精力" + currentEnergy);
            Debug.Log("总精力" + owner.Energy);
        }
        private void ParryHurt()
        {
            if (owner.find_Player.m_IsAttackThump)
            {
                //Debug.Log("重攻击");
                EnergyCalculate(10, 15);
                if(owner.Energy < 0)
                {
                    owner.m_Animator.SetInteger(WeakHurt, 1);
                }
            }
            else if (owner.find_Player.m_DoubleClick)
            {
                //Debug.Log("双攻击");
                EnergyCalculate(25, 30);
                if (owner.Energy < 0)
                {
                    owner.m_Animator.SetInteger(WeakHurt, 2);
                }

            }
            else
            {
                //Debug.Log("轻攻击");
                EnergyCalculate(5, 6);
            }
        }



        protected override void EnemyHurtStateEnd(IFsm<EnemyLogic> procedureOwner)
        {
            base.EnemyHurtStateEnd(procedureOwner);
            owner.m_Animator.SetBool(CanAvoid, true);
        }
    }
}

