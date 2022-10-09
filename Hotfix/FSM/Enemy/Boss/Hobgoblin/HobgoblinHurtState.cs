using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;

namespace Farm.Hotfix
{
    public class HobgoblinHurtState : EnemyHurtState
    {
        private static readonly int CanAvoid = Animator.StringToHash("CanAvoid");
        static int Hurt_Wind = 0;
        private float currentEnergy;
        protected static readonly int AvoidBacknum = Animator.StringToHash("AvoidBacknum");
        protected static readonly int AvoidBack = Animator.StringToHash("AvoidBack");
        protected static readonly int WeakHurt = Animator.StringToHash("WeakHurt");
        protected static readonly int m_Stoic_Enemy = Animator.StringToHash("Stoic_Enemy");
        protected static readonly int m_Counterattack = Animator.StringToHash("Counterattack");
        AnimatorStateInfo Info;



        int hurt_a;
        float hurt_time;

        //private HobgoblinLogic me;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            //me = owner as HobgoblinLogic;
            Info = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
            hurt_time = 0;
            ParryHurt();
            if (!owner.IsWeak)
            {
                Hurt_Wind++;
                hurt_a = Utility.Random.GetRandom(3, 6);
                if (Hurt_Wind >= hurt_a && !owner.Stoic_Enemy)
                {
                    owner.m_Animator.ResetTrigger(Hurt);
                    Hurt_Wind = 0;
                    owner.CounterattackNum = Utility.Random.GetRandom(1, 3);
                    //Debug.Log("受击 霸体" + owner.Stoic_Enemy);
                    owner.Stoic_Enemy = true;
                    //owner.Stoic = true;
                    //owner.m_Animator.SetInteger(AvoidBacknum, 0);
                    //owner.m_Animator.SetTrigger(AvoidBack);
                    //owner.m_Animator.SetBool(CanAvoid, true);
                    //Debug.Log("Change_Wind" + Hurt_Wind);
                    //if (owner.IsAnimPlayed)
                    //{
                    //    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
                    //}
                }
            }
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            hurt_time += Time.deltaTime;


            owner.m_Animator.SetInteger(m_Counterattack, owner.CounterattackNum);

            //Debug.Log("霸体 " + owner.Stoic_Enemy);
            owner.m_Animator.SetBool(m_Stoic_Enemy, owner.Stoic_Enemy);
            if (hurt_time > 0.5 && !owner.Stoic_Enemy)
            {
                owner.m_Animator.ResetTrigger(Hurt);
                hurt_time = 0;
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
            }
            else if (owner.Stoic_Enemy)
            {
                if(Info.IsName("attack2Forward_RM") || Info.IsName("attack3Forward_RM"))
                {
                    if(Info.normalizedTime > 0.9f)
                    {
                        owner.CounterattackNum = 0;
                        owner.Stoic_Enemy = false;
                        ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
                    }
                }
            }
        }
        
        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.m_Animator.SetInteger(WeakHurt, 0);
            owner.m_Animator.ResetTrigger(Hurt);
            owner.CounterattackNum = 0;
            owner.m_Animator.SetInteger(m_Counterattack, owner.CounterattackNum);

            //Debug.Log("N_Wind" + Hurt_Wind);
        }

        public static new HobgoblinHurtState Create()
        {
            HobgoblinHurtState state = ReferencePool.Acquire<HobgoblinHurtState>();
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

