using UnityEngine;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Farm.Hotfix.PlayerLogic>;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerHurtState :PlayerBaseActionState
    {
        private PlayerLogic owner;
        private readonly string Layer = "Base Layer";
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        //private static readonly int DownHurt = Animator.StringToHash("DownHurt");
        //private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
        private static readonly int HoldClick = Animator.StringToHash("HoldClick");
        private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
        private static readonly int HurtDirX = Animator.StringToHash("HurtDirX");
        private static readonly int HurtDirY = Animator.StringToHash("HurtDirY");
        private static readonly int KnockedDown = Animator.StringToHash("KnockedDown");
        private static readonly int DownRevolt = Animator.StringToHash("DownRevolt");
        private static readonly int KnockedFly = Animator.StringToHash("KnockedFly");
        private static readonly int RandomFly = Animator.StringToHash("RandomFly");
        private static readonly int FlyRevolt = Animator.StringToHash("FlyRevolt");
        private int DownRevoltNum = 2;
        private bool IsDown;
        private bool Isfly;



        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            //Log.Info("受伤状态");
            
            owner = procedureOwner.Owner;
            if (!owner.isStoic)
            {
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
                owner.HideTrail();//角色倒地 关闭拖尾和攻击检测 防止怪物碰到角色武器会一直受伤
                owner.AttackEnd();//同上
                owner.m_moveBehaviour.IsKnockLock(true);//防止角色倒地后 按方向键会转动
                                                        //受伤声音测试
                GameEntry.Sound.PlaySound(owner.Armors[0].ArmorData.HurtSoundId);
                GameEntry.Sound.PlaySound(owner.Armors[0].ArmorData.HurtSoundId2);
                GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(),
                    owner.Armors[0].ArmorData.HurtEffectId)
                {
                    Owner = owner
                },
                    typeof(PlayerHurtEffectLogic));
            }
            else 
            {
                GameEntry.Sound.PlaySound(owner.Armors[0].ArmorData.StoicHurtSoundId);
                GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(),
                    owner.Armors[0].ArmorData.StoicHurtEffectId)
                {
                    Owner = owner
                },
                    typeof(PlayerHurtEffectLogic));

            }
            
            //owner.Buff.BuffTypeEnum = BuffType.None;
           
            
        }
        private void KnockedDownState()
        {
            if (!owner.isKnockedDown)
            {
                //owner.isKnockedDown = true;
                int num = Random.Range(0, 10);
                IsDown = true;
                //owner.isKnockedDown = true;
                owner.Buff.BuffTypeEnum = BuffType.None;
                owner.m_AnimationEventGetUp = false;
                owner.m_moveBehaviour.IsKnockLock(true);
                if (num > DownRevoltNum)
                {
                    owner.m_Animator.SetTrigger(KnockedDown);

                }
                else
                {
                    owner.m_Animator.SetTrigger(DownRevolt);
                }
            }
            else
            {
                owner.m_Animator.SetTrigger(KnockedDown);
            }

        }
        private void KnockedFlyState()
        {
            if (!owner.isKnockedDown)
            {
                owner.isKnockedDown = true;
                int num = Random.Range(0, 10);

                Isfly = true;
                if (num > 2)
                {
                    owner.m_Animator.SetInteger(RandomFly, Utility.Random.GetRandom(0, 4));
                    owner.m_AnimationEventGetUp = false;
                    owner.m_Animator.SetTrigger(KnockedFly);
                }
                else
                {
                    owner.m_Animator.SetTrigger(FlyRevolt);
                }
                
            }
            else 
            {
                owner.m_Animator.SetTrigger(KnockedDown);
            }
            
        }
        private void HurtState()
        {
            if (!owner.isKnockedDown)
            {
                //Debug.Log("普通受击");
                owner.m_Animator.SetFloat(HurtDirX, owner.attackDir.x);
                owner.m_Animator.SetFloat(HurtDirY, owner.attackDir.y);
                owner.m_Animator.SetTrigger(Hurt);
                owner.m_Animator.ResetTrigger(AttackTrigger);
                owner.isHurt = true;
                //  owner.m_Animator.SetFloat(HoldClick, -1);

            }
            else
            {
                owner.m_Animator.SetTrigger(KnockedDown);
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (IsDown || Isfly)
            {
                if (owner.m_moveBehaviour.m_Horizontal != 0 || owner.m_moveBehaviour.m_Vertical != 0)
                {
                    //Log.Info("切换翻滚");
                    //owner.m_Animator.SetTrigger(Dodge);
                    owner.isKnockedDown = false;
                    IsDown = false;
                    Isfly = false;
                    ChangeState<PlayerDodgeState>(procedureOwner);
                    
                }
                
            }

            if (owner.isStoic)
            {
                ChangeState<PlayerAttackState>(procedureOwner);
            }
            if (owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex(Layer)).normalizedTime >= 0.9f)
            {
                if (owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex(Layer)).IsName("HurtDown 0") ||
                   owner.m_Animator.GetCurrentAnimatorStateInfo(owner.m_Animator.GetLayerIndex(Layer)).IsName("HitDown_OneHand_F_02 0"))
                {
                    IsDown = false;
                    Isfly = false;
                    owner.isKnockedDown = false;
                }
                if (!owner.isKnockedDown)
                { ChangeState<PlayerMotionState>(procedureOwner); }
                
            }
            
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            //owner.m_Animator.SetFloat(HurtDirX, 0);
            //owner.m_Animator.SetFloat(HurtDirY, 0);
            owner.isHurt = false;
            owner.m_moveBehaviour.IsKnockLock(false);
            owner.Buff.BuffTypeEnum = BuffType.None;
        }

        public static PlayerHurtState Create()
        {
            PlayerHurtState state = ReferencePool.Acquire<PlayerHurtState>();
            return state;
        }

    }
}
