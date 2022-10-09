using GameFramework.DataTable;
using GameFramework.Resource;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System.Linq;
using System.Collections;
using GameFramework.Fsm;
using GameFramework.Event;
using System;

namespace Farm.Hotfix
{
   public abstract class TargetableObject:Entity
    {
        [SerializeField]
        private TargetableObjectData m_targetableObjectData = null;
        public Animator m_Animator;
        private bool m_IsCanBreak =true;

        [SerializeField]
        private List<WeaponLogic> m_Weapons = new List<WeaponLogic>();  //武器逻辑

        public List<WeaponLogic> Weapons { get => m_Weapons; set => m_Weapons = value; }   //武器逻辑

        private bool m_Invulnerable;

        public float invulnerabiltyTime=0.1f;

        public float m_timeSinceLastHit = 0.0f;

        public bool IsDefense = false;

        private Dictionary<int, float> m_SkillCD;

        private Buff m_Buff;

        private bool m_IsInvincible = false;

        private bool m_InitEvent = false;


        public bool InitEvent
        {
            get
            {
                return m_InitEvent;
            }
            set
            {
                m_InitEvent = value;
            }
        }

        public bool Invulnerable
        {
            get
            {
                return m_Invulnerable;
            }
            set
            {
                m_Invulnerable = value;
            }
        }

        public Buff Buff
        {
            get
            {
               return m_Buff;
            }
            set
            {
                m_Buff = value;
            }
        }

        /// <summary>
        /// int 技能ID ，float技能CD时间;
        /// </summary>
        public Dictionary<int, float> SkillCD
        {
            get
            {
                return m_SkillCD;
            }
            set
            {
                m_SkillCD = value;
            }
        }

        public bool IsCanBreak
        {
            get
            {
                return m_IsCanBreak;
            }
            set
            {
                m_IsCanBreak = value;
            }
        }

        public TargetableObjectData TargetableObjectData
        {
            get
            {
                return m_targetableObjectData;
            }
        }

        public bool IsDead
        {
            get
            {
                return m_targetableObjectData.HP <= 0;
            }
        }

        public abstract ImpactData GetImpactData();

        public virtual void ApplyDamage(Entity attacker,int damageHP,Vector3 weapon)
        {
            //if (Invulnerable)
            //{
            //    return;
            //}
            float fromHPRatio = m_targetableObjectData.HPRatio;
            m_targetableObjectData.HP -= damageHP;
            float toHPRatio = m_targetableObjectData.HPRatio;
            if (fromHPRatio > toHPRatio)
            {
                GameHotfixEntry.HPBar.ShowHPBar(this, fromHPRatio, toHPRatio);
            }
            // isInvulnerable = true;
            if (m_targetableObjectData.HP <= 0)
            {
                if(attacker != null)
                    OnDead(attacker,weapon);
            }
        }

        public virtual void ApplySkillDamage(Entity attacker ,int damageHP)
        {
            float fromHPRatio = m_targetableObjectData.HPRatio;
            m_targetableObjectData.HP -= damageHP;
            float toHPRatio = m_targetableObjectData.HPRatio;
            if (fromHPRatio > toHPRatio)
            {
                GameHotfixEntry.HPBar.ShowHPBar(this, fromHPRatio, toHPRatio);
            }
            if (m_targetableObjectData.HP <= 0)
            {
               // OnDead(attacker);
            }
        }

        public virtual void ApplyEnergy(Entity attacker ,int damageMP)
        {
            float fromMPRatio = m_targetableObjectData.MPRatio;
            m_targetableObjectData.MP -= damageMP;
            float toMPRatio = m_targetableObjectData.MPRatio;
            if(fromMPRatio > toMPRatio)
            {
                //蓝条
            }

        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            //if (Invulnerable)
            //{
            //    m_timeSinceLastHit += Time.deltaTime;
            //    if (m_timeSinceLastHit > invulnerabiltyTime)
            //    {
            //        m_timeSinceLastHit = 0f;
            //        Invulnerable = false;

            //    }
            //}
            foreach (int item in m_SkillCD.Keys.ToList())
            {
                if (m_SkillCD[item] > 0)
                {
                    m_SkillCD[item] -= elapseSeconds;
                }
                else
                {
                    m_SkillCD[item] = 0;
                }
            }

        }

        protected override void OnInit(object userData)
        {  
            base.OnInit(userData);
            m_Animator = GetComponent<Animator>();
            if(m_Animator == null)
            {
                m_Animator = GetComponentInChildren<Animator>();
            }
            m_SkillCD = new Dictionary<int, float>();
            m_Buff = new Buff();
            //gameObject.SetLayerRecursively(Constant.Layer.TargetableObjectLayerId);
        }


        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_targetableObjectData = userData as TargetableObjectData;
            if(m_targetableObjectData == null)
            {
                Log.Error("Targetable object data is invalid.");
                return;
            }
        }

        protected virtual void OnDead(Entity attacker,Vector3 point)
        {
            GameEntry.Entity.HideEntity(this);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }


        private void OnTriggerEnter(Collider other)
        {
            Entity entity = other.gameObject.GetComponent<Entity>();
            if(entity == null)
            {
                return;
            }

            if(entity is TargetableObject && entity.Id >= Id)
            {
                // 碰撞事件由 Id 小的一方处理，避免重复处理
                return;
            }

           // AIUtility.PerformCollision(this, entity);
        }

    }
}
