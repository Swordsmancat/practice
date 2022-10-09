using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;

namespace Farm.Hotfix
{

    public class WeregoatHurtState :EnemyHurtState
    {
        private static readonly int m_HurtState = Animator.StringToHash("HurtState");
        private static readonly int CanAvoid = Animator.StringToHash("CanAvoid");
        private static readonly int FightD = Animator.StringToHash("FightD");
        //private static readonly int HurtThump = Animator.StringToHash("HurtThump");
        //private static readonly int Hurt = Animator.StringToHash("Hurt");
        private static readonly int IsStun = Animator.StringToHash("IsStun");
        private static readonly int KnockedDown = Animator.StringToHash("KnockedDown");
        private static readonly int IsUp = Animator.StringToHash("IsUp");
        private float downTime;
        private float downTimeout = 1.3f;
        private bool first = false;
        AnimatorStateInfo info;
        private int hurtNum;
        private int hurtLoss;
        private WeregoatLogic me;

        protected override void OnEnter(ProcedureOwner fsm)
        {
            //  Debug.Log("进入受击状态");
            owner = fsm.Owner;
            me = owner as WeregoatLogic;
            base.OnEnter(fsm);
            //myowner = new WeregoatLogic();
            owner.SetRichAiStop();
            
            //Debug.Log("背后武器状态" + me.BackWeapon.activeSelf);
            if (me.BackWeapon.activeSelf)
            {
               // Debug.Log("背后武器" + me.BackWeapon);
                first = true;
            }
                
            if (first)
            {
                owner.m_Animator.SetTrigger(FightD);
                
                first = false;
                owner.LockingEntity = owner.find_Player;
                
            }
            
            hurtLoss = Utility.Random.GetRandom(1, 4);

            owner.Energy -= (hurtLoss+5);
            hurtNum += 1;

            if (hurtNum > hurtLoss && !owner.Stoic && !owner.isKnockedDown)
            {
                Debug.Log("受击次数" + hurtNum);
                //owner.m_IsDefense = true;
                owner.IsDefense = true;
                hurtNum = 0;

            }
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            info = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
            HurtAnimationEnd();
            if (me.BackWeapon.activeSelf && info.IsName("BackWeaponClose") && info.normalizedTime > 0.15f)
                me.BackWeaponClose();
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        }
        private void HurtTap()
        {
            if (owner.isKnockedDown)
            {
                owner.m_Animator.SetTrigger(KnockedDown);
            }
            else
            {
                Vector3 target = owner.find_Player.transform.position - owner.transform.position;
                Vector3 obj = owner.transform.forward;
                //目标方位
                bool forward = (Vector3.Dot(target, obj) > 0);
                bool left = (Vector3.Cross(target, obj).y > 0);
                //owner.IsHurt = false;
                Debug.Log("轻攻击");
                if (forward)
                {
                    //Debug.Log("遭受普通攻击");
                    owner.m_Animator.SetTrigger(Hurt);
                    owner.m_Animator.SetInteger(m_HurtState, 0);
                    //GameEntry.Sound.PlaySound(ShoutId);
                }
                else if (left)
                {
                    owner.m_Animator.SetTrigger(Hurt);
                    owner.m_Animator.SetInteger(m_HurtState, 1);
                    //GameEntry.Sound.PlaySound(ShoutId);
                }
            }
            

        }
        private void AHurtThump()
        {
            if (owner.isKnockedDown)
            {
                owner.m_Animator.SetTrigger(KnockedDown);
            }
            else 
            {
                Vector3 target = owner.find_Player.transform.position - owner.transform.position;
                Vector3 obj = owner.transform.forward;
                //目标方位
                bool forward = (Vector3.Dot(target, obj) > 0);
                bool left = (Vector3.Cross(target, obj).y > 0);
                Debug.Log("重攻击");
                int num = Utility.Random.GetRandom(0, 10);
                if (forward)
                {
                    owner.m_Animator.SetTrigger(HurtThump);
                    owner.m_Animator.SetInteger(m_HurtState, 0);
                    //GameEntry.Sound.PlaySound(ShoutId);
                }

                if (left)
                {
                    owner.m_Animator.SetTrigger(HurtThump);
                    owner.m_Animator.SetInteger(m_HurtState, 1);
                    //GameEntry.Sound.PlaySound(ShoutId);
                }
                else if (!left)
                {
                    owner.m_Animator.SetTrigger(HurtThump);
                    owner.m_Animator.SetInteger(m_HurtState, 2);
                    //GameEntry.Sound.PlaySound(ShoutId);
                }

                //    if (num == 3 || num == 4)
                //    {
                //        owner.m_Animator.SetTrigger(HurtThump);
                //        owner.m_Animator.SetInteger(m_HurtState, num);
                //        //GameEntry.Sound.PlaySound(ShoutId);
                //    }
                else if (num == 0 || !forward)
                    owner.m_Animator.SetTrigger(IsStun);
            }
            
        }
        private void HurtOverwhelmed()
        {
            if (owner.isKnockedDown)
            {
                owner.m_Animator.SetTrigger(KnockedDown);
            }
            else
            {
                Vector3 target = owner.find_Player.transform.position - owner.transform.position;
                Vector3 obj = owner.transform.forward;
                //目标方位
                bool forward = (Vector3.Dot(target, obj) > 0);
                bool left = (Vector3.Cross(target, obj).y > 0);
                Debug.Log("双攻击");
                owner.m_Animator.SetTrigger(HurtThump);
                if (forward)
                {
                    owner.m_Animator.SetInteger(m_HurtState, 0);
                }
                else if (left)
                {
                    owner.m_Animator.SetInteger(m_HurtState, 1);
                }
                else if (!left)
                {
                    owner.m_Animator.SetInteger(m_HurtState, 2);
                }
                else
                {
                    owner.m_Animator.SetInteger(m_HurtState, 0);
                }
            }    
            
        }

        protected override void EnemyHurtStateStart(ProcedureOwner fsm)
        {
            if (!owner.Stoic)
            {
                switch (me.m_BuffType)
                {
                    case BuffType.None:
                        break;
                    case BuffType.Tap:
                        HurtTap();
                        break;
                    case BuffType.Thump:
                        AHurtThump();
                        break;
                    case BuffType.Overwhelmed:
                        HurtOverwhelmed();
                        break;
                    default:
                        owner.m_Animator.SetTrigger(Hurt);
                        break;
                }
                owner.HideTrail();//角色倒地 关闭拖尾和攻击检测 防止怪物碰到角色武器会一直受伤
                owner.EnemyAttackEnd();//同上
            }
            
            owner.m_Animator.SetBool(CanAvoid, false);
        }
        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.m_Animator.SetInteger(m_HurtState, -1);
        }
            public static WeregoatHurtState Create()
        {
            WeregoatHurtState state = ReferencePool.Acquire<WeregoatHurtState>();
            return state;
        }

        protected override void EnemyHurtStateEnd(IFsm<EnemyLogic> procedureOwner)
        {
            owner.m_Animator.SetBool(CanAvoid, true);
            if (!owner.Stoic&&owner.IsAnimPlayed)
            {
                base.EnemyHurtStateEnd(procedureOwner);
            }

        }
        private void HurtAnimationEnd()
        {
            //Debug.Log("动画播放进度" + info.normalizedTime);
            ////判断动画是否播放完成
            if (owner.isKnockedDown)
            {
                if (info.IsName("倒地") && downTime > downTimeout)
                {
                    owner.AnimationEnd();
                    owner.m_Animator.SetTrigger(IsUp);
                    owner.isKnockedDown = false;
                    downTime = 0;
                }
            }
            else if(info.normalizedTime >= 0.80f)
            { 
                owner.AnimationEnd();
                //owner.m_Animator.SetInteger(m_HurtState, -1);
            }

        }
    }
}
