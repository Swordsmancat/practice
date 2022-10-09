using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{

    //状态0:初始闲置状态下被攻击
    //状态1:地面状态下被攻击
    //状态2:飞行状态下被攻击
    public enum HurtStateType{
        RelaxIdleHurt,
        GroundIdleHurt,
        FlyIdleHurt
    }

    /// <summary>
    /// 黑龙逻辑
    /// </summary>
    public class DragonLogic : EnemyLogic
    {
        readonly private static int s_layerMask = 1 << 6;       // 地面层(过滤)
        readonly private static int s_obs = 1 << 7;
        int a = s_layerMask | s_obs;
        public float CurrentHight;                              // 当前高度
        public bool IsCanUp = false;                            // 是否上升
        public bool IsFying = false;                            // 是否在飞行中
        public bool IsCanForward = false;                       // 是否可以向前
        public bool IsFire = false;                             // 是否喷火
        public bool IsEffectAttack = false;                     // 是否在攻击用于判断显示什么特效
        public bool IsAvoid = false;                            // 躲避攻击(目前用于切换动画)
        public int AttackState;                                 // 攻击状态
        public int FireBallCount;                               // 火球攻击计数

        // 受伤状态
        public HurtStateType HurtState = HurtStateType.RelaxIdleHurt;
        #region Base

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            //Vector3 dir = find_Player.transform.position - transform.position;
            //Debug.DrawRay(transform.position, dir * 10, Color.red);
            //Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);
            //发出一条垂直向下的射线
            //最大检查距离为10,设为最大飞行高度
            Ray downRay = new Ray(transform.position + Vector3.up, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(downRay, out hit, 10, a))
            {
                //Debug.DrawLine(downRay.origin, hit.point);
                IsCanUp = true;
            }
            else
            {
                IsCanUp = false;
            }
            CurrentHight = downRay.origin.y - hit.point.y - 1;
        }

        protected override void OnDead(Entity attacker, Vector3 point)
        {
            base.OnDead(attacker,point);
        }

        public override void ApplyDamage(Entity attacker, int damageHP, Vector3 weapon)
        {
            if(IsAvoid)
            {
                return;
            }

            base.ApplyDamage(attacker, damageHP,weapon);

            if (m_PrevAttackTimeHP != enemyData.HP)
            {
                IsHurt = true;
                
                GameEntry.Sound.PlaySound(enemyData.ByAttackSoundId);
                GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), enemyData.BloodEffectId)
                {
                    Position = weapon,
                },
                    typeof(EnemyHurtEffectLogic));
                m_PrevAttackTimeHP = enemyData.HP;
            }
        }

        #endregion

        #region Fsm

        public override Type ChangeStateEnemy(EnemyStateType stateType)
        {
            switch (stateType)
            {
                case EnemyStateType.Idle:
                    return typeof(DragonIdleState);
                case EnemyStateType.Motion:
                    return typeof(DragonMotionState);
                case EnemyStateType.Hurt:
                    return typeof(DragonHurtState);
                case EnemyStateType.Attack:
                    return typeof(DragonAttackState);
                case EnemyStateType.Dead:
                    return typeof(DragonDeadState);
                case EnemyStateType.Block:
                    return typeof(EnemyBlockState);
                case EnemyStateType.Parry:
                    return typeof(EnemyParryState);
                case EnemyStateType.Rotate:
                    return typeof(DragonRotateState);
                case EnemyStateType.Shout:
                    return typeof(DragonShoutState);
                case EnemyStateType.OutOfFight:
                    return typeof(EnemyOutOfTheFight);
                case EnemyStateType.Fight:
                    return typeof(DragonFightState);
                default:
                    return typeof(EnemyIdleState);
            }
        }

        protected override void AddFsmState()
        {
            stateList.Add(DragonIdleState.Create());
            stateList.Add(DragonShoutState.Create());
            stateList.Add(DragonMotionState.Create());
            stateList.Add(DragonFightState.Create());
            stateList.Add(DragonAttackState.Create());
            stateList.Add(DragonHurtState.Create());
            stateList.Add(DragonFlyState.Create());
            stateList.Add(DragonRotateState.Create());
            stateList.Add(DragonDeadState.Create());
        }

        protected override void StartFsm()
        {
            fsm.Start<DragonIdleState>();
        }

        #endregion


        #region AnimationEvent

        /// <summary>
        /// 此事件放在To_Fly_Push动画 120针的位置
        /// </summary>
        public void AnimSetForward()
        {
            IsCanForward = true;
        }

        /// <summary>
        /// 此事件用在To_Fly_Push动画 120针的位置
        /// Land_Skill动画
        /// </summary>
        public void Fire()
        {
            IsUseSkill = true;
            IsFire = true;
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70014),
                typeof(BlackDragonFireLogic));
        }

        public void FireBall()
        {
            IsUseSkill = true;
            FireBallCount += 1;
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70018),
                typeof(BlackDragonFireBall));
        }

        public void AttackEffect()
        {
            IsUseSkill = true;
            IsEffectAttack = true;
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70017),
                typeof(BlackDragonSmokeLogic));
        }

        public void GroundSmoke()
        {
            IsUseSkill = true;
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70016),
                    typeof(BlackDragonSmokeLogic));
        }

        public void EndAllEffect()
        {
            IsFire = false;
        }

        public void Flying()
        {
            IsFying = true;
        }

        public void AvoidAttackStart()
        {
            IsAvoid = true;
        }

        public void AvoidAttackEnd()
        {
            IsAvoid = false;
        }

        #endregion
    }
}

