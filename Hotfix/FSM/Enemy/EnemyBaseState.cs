using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using System;

namespace Farm.Hotfix
{
    /// <summary>
    /// 敌人基类状态
    /// </summary>
    public class EnemyBaseState : FsmState<EnemyLogic>, IReference
    {
        private EnemyLogic owner;
        private static float xx_Time = 0;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
        }



        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            //Debug.Log("无敌 " + owner.Is_Invincible);


            if (owner.enemyData.HP <= 0)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Dead));
            }
            if (owner.enemyData.HP != owner.enemyData.HP)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Hurt));
            }
            //Debug.Log(owner.IsCanAttack);                                                     
            //Debug.Log("动画结束" + owner.IsAnimPlayed);
            if (!owner.IsCanAttack)
            {
                //敌人所有状态机维护同一个攻击速度
                owner.AttackTimer += elapseSeconds;
                //Debug.Log(owner.AttackTimer);
                if (owner.AttackTimer >= owner.enemyData.AttackSpeed - owner.ReduceAttackTime)
                {
                    //owner.AttackTimer = 0;
                    owner.PerformAttack();
                }
            }
            //判断空精状态并触发动画机参数
          //  owner.m_Animator.SetBool(Animator.StringToHash("Weak"), owner.IsWeak);
            //Debug.Log("精力" + owner.Energy);
            if (owner.Energy <= 0 && owner.IsWeak == false)
            {
                owner.IsWeak = true;
            }
            else if (owner.Energy > 0 && owner.IsWeak == true)
            {
                owner.IsWeak = false;
            }
            //Debug.Log("虚弱" + owner.IsWeak);
            if (owner.IsWeak)
            {
                xx_Time += Time.deltaTime;
                owner.Stoic = false;
                //Debug.Log("虚弱时间" + xx_Time);
                if (xx_Time > owner.WeakTime)
                {
                    //Debug.Log("虚弱结束");
                    owner.Energy = owner.MaxEnergy;
                    xx_Time = 0;
                }
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

        protected override void OnDestroy(ProcedureOwner fsm)
        {
            base.OnDestroy(fsm);
        }

        public void Clear()
        {

        }

    }
}