using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.EnemyLogic>;
using GameFramework;

namespace Farm.Hotfix
{
    public class DragonideHurtState : EnemyHurtState
    {
        private static readonly int m_HurtState = Animator.StringToHash("HurtState");
        private static readonly int CanAvoid = Animator.StringToHash("CanAvoid");
        //private static readonly int infight = Animator.StringToHash("Infight");
        private static readonly int IsStun = Animator.StringToHash("IsStun");
        //private static readonly int FrenzyEnd = Animator.StringToHash("FrenzyEnd");
        //private static readonly int Frenzy = Animator.StringToHash("Frenzy");
        //private static readonly int m_Hurt = Animator.StringToHash("Hurt");
        private static readonly int m_HurtThump = Animator.StringToHash("HurtThump");
        private static readonly int KnockedDown = Animator.StringToHash("KnockedDown");
        private static readonly int IsUp = Animator.StringToHash("IsUp");
        private float downTime;
        private float downTimeout = 3f;
        private DragonideLogic me;
        AnimatorStateInfo info;
        private int hurtNum;
        private int hurtLoss;
        private bool Rage = false;
        private bool Buffend=true;


        public static DragonideHurtState Create()
        {
            DragonideHurtState state = ReferencePool.Acquire<DragonideHurtState>();
            return state;
        }
        protected override void OnEnter(ProcedureOwner fsm)
        {
            owner = fsm.Owner;
            me = owner as DragonideLogic;
                //owner.m_Animator.SetBool(infight, true);
            base.OnEnter(fsm);
            if (!owner.Stoic && !owner.isKnockedDown)
            {
                hurtLoss = Utility.Random.GetRandom(1, 4);
                //owner = fsm.Owner;
                owner.Energy -= (hurtLoss + 5);
                hurtNum += 1;
                if (hurtNum > hurtLoss)
                {
                    Debug.Log("受击次数" + hurtNum);
                    //owner.m_IsDefense = true;
                    owner.IsDefense = true;
                    hurtNum = 0;
                }
            }
            else 
            {
                owner.IsDefense = false;
            }
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            info = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
            if (owner.isKnockedDown)
            {
                downTime += Time.deltaTime;
            }
            HurtAnimationEnd();
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        }
        protected override void EnemyHurtStateStart(ProcedureOwner fsm)
        {
            //Debug.Log("霸体" + owner.Stoic);
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
        }
            
            owner.m_Animator.SetBool(CanAvoid, false);
        }
        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            owner.m_Animator.SetInteger(m_HurtState, -1);
        }
        private void HurtAnimationEnd()
        {
            //Debug.Log("动画播放进度" + info.normalizedTime);
            ////判断动画是否播放完成
            if (owner.isKnockedDown)
            {
                if (info.IsName("倒地") && downTime>downTimeout)
                {
                    owner.AnimationEnd();
                    owner.m_Animator.SetTrigger(IsUp);
                    owner.isKnockedDown = false;
                    downTime = 0;
                }
            }
            else if (info.normalizedTime >= 0.90f)
            {
                owner.AnimationEnd();
                //owner.m_Animator.SetInteger(m_HurtState, -1);
            }

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
                    owner.m_Animator.SetTrigger(m_HurtThump);
                    owner.m_Animator.SetInteger(m_HurtState, 0);
                    //GameEntry.Sound.PlaySound(ShoutId);
                }

                if (left)
                {
                    owner.m_Animator.SetTrigger(m_HurtThump);
                    owner.m_Animator.SetInteger(m_HurtState, 1);
                    //GameEntry.Sound.PlaySound(ShoutId);
                }
                else if (!left)
                {
                    owner.m_Animator.SetTrigger(m_HurtThump);
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
            owner.m_Animator.SetTrigger(m_HurtThump);
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
        protected override void EnemyHurtStateEnd(IFsm<EnemyLogic> procedureOwner)
        {
            owner.m_Animator.SetBool(CanAvoid, true);
            //owner.m_Animator.SetInteger(m_HurtState, -1);
            if (!owner.Stoic && !owner.isKnockedDown)
            {
                base.EnemyHurtStateEnd(procedureOwner);
            }
            
        }
    }
}