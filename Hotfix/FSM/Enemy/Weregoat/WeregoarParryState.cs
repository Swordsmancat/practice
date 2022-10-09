using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace Farm.Hotfix
{
    public class WeregoarParryState : EnemyParryState
    {
        private readonly static int m_IsParry = Animator.StringToHash("IsParry");
        private readonly static int m_ParryOut = Animator.StringToHash("ParryOut");
        private readonly static int m_Hurt = Animator.StringToHash("Hurt");
        private readonly static int m_Inparry = Animator.StringToHash("InParry");
        private readonly static int m_ParryHurtL = Animator.StringToHash("ParryHurtL");
        private readonly static int m_ParryHurtR = Animator.StringToHash("ParryHurtR");
        private readonly static int m_Counter = Animator.StringToHash("Counter");
        private static readonly int KnockedDown = Animator.StringToHash("KnockedDown");
        private float parryTime;
        private float toParryTime;
        private bool parryout;
        AnimatorStateInfo info;
        //private bool IsParry;
        private EnemyLogic owner;
        //private float Energy=100;
        private float currentEnergy;
        private int hurtNum;
        private int hurtLoss;
        //private float exitTimer = 0;
        private WeregoatLogic me;
        private float angleSpeed = 0.1f;
        private bool isRotate = true;
        private bool isDown;
        private bool toParry = true;
        protected override void OnInit(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner;
            me = owner as WeregoatLogic;
            //IsParry = owner.IsParry;
            owner.IsDefense = true;
            //Debug.Log("½øÈë¸ñµ²×´Ì¬");
            owner.IsCanAttack = false;
            //Energy = EnemyLogic.Energy;
            //hurtNum = EnemyLogic.hurtNum;
            hurtLoss = Utility.Random.GetRandom(1, 5);
            EnemyParryStateStart(owner);
            owner.SetRichAiStop();
            owner.HideTrail();//½ÇÉ«µ¹µØ ¹Ø±ÕÍÏÎ²ºÍ¹¥»÷¼ì²â ·ÀÖ¹¹ÖÎïÅöµ½½ÇÉ«ÎäÆ÷»áÒ»Ö±ÊÜÉË
            owner.EnemyAttackEnd();//Í¬ÉÏ
            parryout = true;
                      
        }
        public static WeregoarParryState Create()
        {
            WeregoarParryState state = ReferencePool.Acquire<WeregoarParryState>();
            return state;
        }
        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            
            parryTime += Time.deltaTime;
            //toParryTime += Time.deltaTime;
            //toParrytime += Time.deltaTime;
            ////Log.Info("¸ñµ²¼ä¸ôÊ±¼ä" + toParrytime);
            
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            Vector3 theForward = owner.find_Player.transform.position - owner.transform.position;
            Quaternion rotate = Quaternion.LookRotation(theForward);
            me.StartPoint.transform.forward = owner.find_Player.transform.position - owner.transform.position;
            if (Vector3.Angle(theForward, owner.transform.forward) < 0.1f)
            {
                isRotate = false;
            }
            else 
            {
                isRotate = true;
            }
            //Debug.Log("×ªÏò" + isRotate + Vector3.Angle(theForward, owner.transform.forward));    
            if (isRotate)
            {
                owner.transform.localRotation = Quaternion.Slerp(owner.transform.localRotation, rotate, angleSpeed);
            }

            if (owner.underAttack) 
            {
                ParryHurt();
            }
                
            
            OutParry(procedureOwner);
            
                


        }
        /// <summary>
        /// ¾«Á¦Öµ¼ÆËã
        /// </summary>
        /// <param name="maxNum"></param>
        /// <param name="minNum"></param>
        private void EnergyCalculate(int minNum, int maxNum)
        {
            int energyLoss = Utility.Random.GetRandom(minNum, maxNum);
            currentEnergy = owner.Energy - energyLoss;
            owner.Energy = currentEnergy > 0 ? currentEnergy : 0;
            Debug.Log("µ±Ç°¾«Á¦" + currentEnergy);
            Debug.Log("×Ü¾«Á¦" + owner.Energy);
        }

        private void HurtTap()
        {
            //owner.IsHurt = false;
            Debug.Log("Çá¹¥»÷");
            hurtNum += 1;

            EnergyCalculate(15, 25);
            if (owner.Energy > 0 && hurtNum < 3)
            {
                owner.m_Animator.SetTrigger(m_Hurt);
            }
            else if (hurtNum >= 3)
            {
                if (AIUtility.GetCross(owner, owner.LockingEntity) > 0)
                    owner.m_Animator.SetTrigger(m_ParryHurtL);
                else
                    owner.m_Animator.SetTrigger(m_ParryHurtR);
                hurtNum = 0;
            }
        }
        private void HurtThump()
        {
            Debug.Log("ÖØ¹¥»÷");
            EnergyCalculate(25, 35);
            //owner.m_Animator.SetTrigger(m_Hurt);
            hurtNum += 2;
            //owner.underAttack = false;
            if (owner.Energy > 0 && hurtNum < 3)
            {
                owner.m_Animator.SetTrigger(m_Hurt);
            }
            else if (hurtNum >= 3)
            {
                if (AIUtility.GetCross(owner, owner.LockingEntity) > 0)
                    owner.m_Animator.SetTrigger(m_ParryHurtL);
                else
                    owner.m_Animator.SetTrigger(m_ParryHurtR);
                hurtNum = 0;
            }
        }
        private void HurtOverwhelmed()
        {
            Debug.Log("Ë«¹¥»÷");
            EnergyCalculate(35, 45);
            //owner.m_Animator.SetTrigger(m_Hurt);
            if (owner.Energy > 0)
            {
                hurtNum += 2;
                owner.m_Animator.SetTrigger(m_ParryOut);
            }
            else
            {
                isDown = true;

            }
        }
        /// <summary>
        /// ¸ñµ²ÊÜ»÷
        /// </summary>
        private void ParryHurt()
        {
            switch (me.m_BuffType)
            {
                case BuffType.None:
                    break;
                case BuffType.Tap:
                    HurtTap();
                    break;
                case BuffType.Thump:
                    HurtThump();
                    break;
                case BuffType.Overwhelmed:
                    HurtOverwhelmed();
                    break;
                default:
                    break;
            }
            owner.underAttack = false;
            //toParry = false;



        }
        /// <summary>
        /// ¸ñµ²ÍË³ö·½Ê½
        /// </summary>
        /// <param name="procedureOwner"></param>
        private void OutParry(IFsm<EnemyLogic> procedureOwner)
        {
            //Õý³£½áÊø¸ñµ²×´Ì¬
            if (owner.Energy > 0 && parryTime >= 3.5f)
            {
                //IsParry = false;
                //Debug.Log("ÍË³ö·ÀÓù×´Ì¬");
                EnemyParryStateEnd(procedureOwner);
            }
            ////·´»÷½áÊø¸ñµ²×´Ì¬
            else if (hurtNum >= hurtLoss && owner.Energy > 0)
            {
                EnemyParryStateCounter(procedureOwner);
            }
            //ÆÆ·À
            else if (owner.Energy <= 0)
            {

                //owner.m_Animator.SetTrigger(m_ParryOut);
                EnemyParryOutStateEnd(procedureOwner);
            }
            else
            {
                if (parryTime > 4.0f)
                {
                    //SetAnimaSpeed();
                    EnemyParryStateEnd(procedureOwner);
                }

            }

        }
        protected override void OnLeave(IFsm<EnemyLogic> fsm, bool isShutdown)
        {
            Debug.Log("Àë¿ªÕÐ¼Ü×´Ì¬");
            base.OnLeave(fsm, isShutdown);
            owner.IsParry = false;
            owner.IsDefense = false;
            owner.IsCanAttack = true;
            //owner.toParry = true;
            //owner.IsDefense = false;
            parryTime = 0;
            isDown = false;
            owner.underAttack = false;
            owner.m_Animator.SetBool(m_IsParry, false);
        }
        /// <summary>
        /// ÕÐ¼Ü×´Ì¬¿ªÊ¼
        /// </summary>
        protected override void EnemyParryStateStart(EnemyLogic owner)
        {
            
            owner.m_Animator.SetBool(m_IsParry,true);
            owner.m_Animator.SetTrigger(m_Inparry);
            owner.EnemyAttackEnd();
            //owner.SetRichAiStop();
        }

        /// <summary>
        /// ÕÐ¼Ü×´Ì¬½áÊø
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected override void EnemyParryStateEnd(IFsm<EnemyLogic> procedureOwner)
        {
            //Debug.Log("Õý³£Àë¿ªÕÐ¼Ü×´Ì¬");
            
            //hurtNum = 0;
            if(owner.Energy<50)
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
                //owner.m_Animator.SetTrigger(m_Counter);
                //Debug.Log("ÇÐ»»ÒÆ¶¯×´Ì¬");
            }
                
            else
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Attack));
                //owner.m_Animator.SetTrigger(m_Counter);
                //Debug.Log("ÇÐ»»¹¥»÷×´Ì¬");
            }    
        }
        /// <summary>
        /// ÕÐ¼Ü·´»÷
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected override void EnemyParryStateCounter(IFsm<EnemyLogic> procedureOwner)
        {
            owner.m_Animator.SetTrigger(m_Counter);
            hurtNum = 0;
            base.EnemyParryStateCounter(procedureOwner);
        }
        /// <summary>
        /// ÕÐ¼ÜÆÆ·ÀÍË³ö
        /// </summary>
        /// <param name="procedureOwner"></param>
        protected override void EnemyParryOutStateEnd(IFsm<EnemyLogic> procedureOwner)
        {
            //info = owner.m_Animator.GetCurrentAnimatorStateInfo(0);
            //Debug.Log("ÆÆ·ÀÀë¿ªÕÐ¼Ü×´Ì¬");
            owner.IsDefense = false;
            if (parryout)
            {
                owner.m_Animator.SetTrigger(m_ParryOut);
                parryout = false;
                //owner.Energy += 1;
            }
            if (isDown)
            {
                //Debug.Log("»÷µ¹");
                //ChangeState<EnemyKnockedDownState>(procedureOwner);
                owner.isKnockedDown = true;
                owner.m_Animator.SetTrigger(KnockedDown);
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Hurt));
            }
            else
            {
                ChangeState(procedureOwner, owner.ChangeStateEnemy(EnemyStateType.Motion));
            }
                
            //if (info.IsName("¸ñµ²ÆÆ·À") && info.normalizedTime > 0.55f)
            //{
                
            //    //base.EnemyParryOutStateEnd(procedureOwner);
            //}

            //
        }

    }


}

