using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;


namespace Farm.Hotfix
{
    public class EnemyHurtState : EnemyBaseActionState
    {
        private readonly string Layer = "Base Layer";
        protected static readonly int Hurt = Animator.StringToHash("Hurt");
        protected static readonly int HurtThump = Animator.StringToHash("HurtThump");
        protected static readonly int HurtSky = Animator.StringToHash("HurtInSky");
        private static readonly int HurtDirX = Animator.StringToHash("HurtDirX");
        private static readonly int HurtDirY = Animator.StringToHash("HurtDirY");
        protected EnemyLogic owner;

        protected override void OnEnter(ProcedureOwner fsm)
        {
            base.OnEnter(fsm);
            //owner.transform.LookAt(owner.find_Player.transform);
            owner = fsm.Owner;
            owner.Buff.BuffTypeEnum = BuffType.None;
            owner.m_Animator.SetFloat(HurtDirX, owner.attackDir.x);
            owner.m_Animator.SetFloat(HurtDirY, owner.attackDir.y);
            if (owner.enemyData.HP <= 0)
            {
                ChangeState(fsm, owner.ChangeStateEnemy(EnemyStateType.Dead));
    			return;            
            }

            Debug.Log("进入敌人受伤状态");            
            EnemyHurtStateStart(fsm);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            //Debug.Log(owner.IsAnimPlayed);

            if (owner.enemyData.HP <= 0)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Dead));
    			return;            }

            if (owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex(Layer)).normalizedTime >= 0.6f)
            {
                EnemyHurtStateEnd(procedureOwner);
               // ChangeState<PlayerMotionState>(procedureOwner);
            }
            //动画播放完毕
            //if (owner.IsAnimPlayed)
            //{
            //}

        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.IsAnimPlayed = false;
            owner.EnemyAttackEnd();
            //Debug.Log("离开敌人受伤状态");
        }

        public static EnemyHurtState Create()
        {
            EnemyHurtState state = ReferencePool.Acquire<EnemyHurtState>();
            return state;
        }


        /// <summary>
        /// 受伤状态开始
        /// </summary>
        protected virtual void EnemyHurtStateStart(ProcedureOwner fsm)
        {
            owner.SetRichAiStop();
            owner.EnemyAttackEnd();
            owner.m_Animator.SetTrigger(Hurt);
        }

        /// <summary>
        /// 受伤状态结束
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected virtual void EnemyHurtStateEnd(ProcedureOwner procedureOwner)
        {
            ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Fight));
        }
    }
}