using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class HobgoblinFightState : EnemyFightState
    {
        private readonly static int m_AvoidBack = Animator.StringToHash("AvoidBack");
        protected static readonly int AttackState = Animator.StringToHash("AttackState");
        protected static readonly int Back_Hob = Animator.StringToHash("Back_Hob");
        private readonly static int m_AvoidPrec = 10;
        private readonly float AvoidSpeed = 15f;
        private bool OnlyOnce = true;
        private int m_ChanceDo;
        int u_num;
        float EnergyTime;
        int pp = 0;
        bool PXL = false;
        AnimatorStateInfo Info;



        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            //Debug.Log("½øÈëÕ½¶·×´Ì¬");
            owner = procedureOwner.Owner;
            u_num = Utility.Random.GetRandom(0, 100);
            //Log.Info("Attack_Wind" + u_num);
            //Debug.Log("ºóÍË¾àÀë " + owner.CurrentTargetDisdance + "u_num " + u_num);
            if (u_num <= 20 && !owner.IsWeak)
            {
                //Debug.Log("Õ½¶·--»·ÈÆ");
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Wind));
                u_num = 0;
            }
            else if (u_num>80 && !owner.IsWeak && owner.CurrentTargetDisdance < 4f)
            {
                //Debug.Log("ºóÍË");
                PXL = true;
                owner.m_Animator.SetTrigger(Back_Hob);

            }
            owner.SetRichAiStop();
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            owner.m_Animator.SetBool(Animator.StringToHash("Weak"), owner.IsWeak);
            //if (owner.IsWeak)
            //{
            //    EnergyTime += elapseSeconds;
            //    Debug.Log("ÐéÈõÊ±¼ä" + EnergyTime);
            //    if (EnergyTime >= 5)
            //    {
            //        owner.Energy = 100;
            //        EnergyTime = 0;
            //    }
            //}
            Debug.Log("ÎÞµÐ " + owner.Is_Invincible);
            if (Info.IsName("walkBack"))
            {
                //Log.Info("0000000000 " + Info_Hob.normalizedTime);
                owner.Is_Invincible = true;
            }
            else if (!Info.IsName("walkBack"))
            {
                //Log.Info("1111111111 " + Info_Hob.normalizedTime);
                owner.Is_Invincible = false;
                PXL = false;
            }
            if (!owner.IsWeak && !PXL)
            {
                float distance = AIUtility.GetDistance(owner.LockingEntity, owner);
                owner.AttackLockEntity(owner.find_Player);
                owner.SetSearchTarget(owner.LockingEntity.CachedTransform);
                //owner.RestoreEnergy();
                if (!owner.CheckInAttackRange(distance))
                {
                    //Debug.Log("Õ½¶·--ÒÆ¶¯");
                    ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
                    owner.m_Animator.SetInteger(AttackState, -1);
                }
            }

        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.Is_Invincible = false;
            OnlyOnce = true;
            //Debug.Log("ÍË³öÕ½¶·×´Ì¬");
            pp = 0;
            //owner.m_Animator.SetInteger(m_Counterattack1, 0);
        }

        public static new HobgoblinFightState Create()
        {
            HobgoblinFightState state = ReferencePool.Acquire<HobgoblinFightState>();
            return state;
        }

        protected override void InAttackRange(ProcedureOwner fsm)
        {
            base.InAttackRange(fsm);

            if (m_ChanceDo <= m_AvoidPrec && owner.find_Player.m_moveBehaviour.isAttack)
            {
                if(OnlyOnce && !owner.IsWeak)
                {
                    //int num = Utility.Random.GetRandom(0, 3);
                    float distance = AIUtility.GetDistance(owner.LockingEntity, owner);
                    if (distance <= owner.enemyData.AttackRange && pp < 1) 
                    {
                        pp++;
                        AvoidAttack(0);
                        OnlyOnce = false;
                    }
                }
            }
        }

        private void AvoidAttack(int num)
        {
            //Debug.Log("¶ã±Ü¶ã±Ü¶ã±Ü£¡£¡£¡");
            switch (num)
            {
                case (int)AvoidType.back:
                    AvoidToBack();
                    break;
                //case (int)AvoidType.left:
                //    owner.m_Animator.SetTrigger(m_BlockRollLeft);
                //    break;
                //case (int)AvoidType.right:
                //    owner.m_Animator.SetTrigger(m_BlockRollRight);
                //    break;
            }
        }
        protected static readonly int AvoidBacknum = Animator.StringToHash("AvoidBacknum");
        private void AvoidToBack()
        {
            //Debug.Log("Õ½¶·ÉÁ±Ü");
            owner.m_Animator.SetTrigger(m_AvoidBack);
            int randomNum = Utility.Random.GetRandom(0, 3);
            owner.m_Animator.SetInteger(AvoidBacknum, randomNum);
            owner.transform.Translate(AvoidSpeed * Time.deltaTime *  Vector3.back, Space.Self);
        }

    }
}
 
