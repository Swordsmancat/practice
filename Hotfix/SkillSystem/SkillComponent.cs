//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/7/5/周二 9:59:20
//------------------------------------------------------------
using UnityGameFramework.Runtime;
using UnityEngine;
using GameFramework.ObjectPool;
using System.Collections.Generic;
using GameFramework.DataTable;
using GameFramework.Resource;
using System;
using GameFramework;
using System.Collections;

namespace Farm.Hotfix
{
    public class SkillComponent : GameFrameworkComponent
    {
        [SerializeField]
        private SkillItem m_SkillItemTemplate = null;

        [SerializeField]
        private int m_InstancePoolCapacity = 16;

        private IObjectPool<SkillItemObject> m_SkillItemObjectPool = null;

        private Dictionary<int, Skill> m_SkillDatas = null;
        private List<SkillItem> m_ActiveSkillItems = null;

        private List<SkillTimer> m_SkillTimer = null;

        private List<TargetableObject> m_tarCache = null;

        private void Start()
        {
            m_SkillItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<SkillItemObject>("SkillItem", m_InstancePoolCapacity);
            m_ActiveSkillItems = new List<SkillItem>();
            m_SkillDatas = new Dictionary<int, Skill>();
            m_SkillTimer = new List<SkillTimer>();
        }

        public void GetAllSkillData()
        {
            IDataTable<DRSkill> dtSkills = GameEntry.DataTable.GetDataTable<DRSkill>();
            DRSkill[] drSkills = dtSkills.GetAllDataRows();
            foreach (var item in drSkills)
            {
                GameEntry.Resource.LoadAsset(AssetUtility.GetSkillDataAsset(item.FileName), new LoadAssetCallbacks(LoadAssetSuccess, LoadAssetFailu), item.Id);
            }
        }

        private void LoadAssetFailu(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Log.Warning(errorMessage);
        }

        private void LoadAssetSuccess(string assetName, object asset, float duration, object userData)
        {
            Log.Info("加载技能 {0} 成功", assetName);
            m_SkillDatas.Add((int)userData, (Skill)asset);
        }

        public void ShowSkill(TargetableObject entity, int skillID)
        {
            if (entity == null)
            {
                Log.Warning("Entity is invalid");
                return;
            }
            if (!CheckShowSkillCondition(entity, skillID, m_SkillDatas[skillID]))
            {
                return;
            }
            SkillItem skillItem = GetActiveSkillItem(entity, skillID);
            if (skillItem == null)
            {
                skillItem = CreateSkillItem(entity);
                m_ActiveSkillItems.Add(skillItem);
            }
            skillItem.Init(entity, skillID, m_SkillDatas[skillID]);
        }

        //private void Add

        private bool CheckShowSkillCondition(TargetableObject entity, int skillId, Skill skillData)
        {
            if (entity.SkillCD.ContainsKey(skillId))
            {
                if (entity.SkillCD[skillId] > 0)
                {
                    Log.Warning("技能CD中");
                    return false;
                }
            }

            if (entity.TargetableObjectData.HP <= skillData.NeedHPValue)
            {
                Log.Warning("血量不足无法施法{0}", skillData.SkillName);
                return false;
            }
            if (entity.TargetableObjectData.MP < skillData.NeedMPValue)
            {
                Log.Warning("精力不足无法施法{0}", skillData.SkillName);
                return false;
            }
            return true;
        }

        private void Update()
        {
            for (int i = 0; i < m_SkillTimer.Count; i++)
            {
                if (m_SkillTimer[i].Duration < 0)
                {
                    m_SkillTimer[i].Clear();
                    ReferencePool.Release(m_SkillTimer[i]);
                    m_SkillTimer.Remove(m_SkillTimer[i]);
                    continue;
                }
                if (m_SkillTimer[i].LossTime >= m_SkillTimer[i].Frequency)
                {
                    m_SkillTimer[i].LossTime = 0;
                    CheckSkillType(m_SkillTimer[i].Owner, m_SkillTimer[i].SkillData, false);
                }
                if (m_SkillTimer[i].EscapTime >= m_SkillTimer[i].SkillData.Buffs.TriggerTime)
                {
                    CheckSkillType(m_SkillTimer[i].Owner, m_SkillTimer[i].SkillData, true);
                }
                else
                {
                    m_SkillTimer[i].EscapTime += Time.deltaTime;
                }
                m_SkillTimer[i].Duration -= Time.deltaTime;
                m_SkillTimer[i].LossTime += Time.deltaTime;
            }
        }



        public void HideSkill(SkillItem skillItem)
        {
            skillItem.Reset();
            m_ActiveSkillItems.Remove(skillItem);
            m_SkillItemObjectPool.Unspawn(skillItem);
        }

        private SkillItem GetActiveSkillItem(Entity entity, int skillID)
        {
            if (entity == null || skillID == 0)
            {
                return null;
            }

            for (int i = 0; i < m_ActiveSkillItems.Count; i++)
            {
                if (m_ActiveSkillItems[i].Owner == entity && m_ActiveSkillItems[i].SkillID == skillID && !m_ActiveSkillItems[i].gameObject.activeInHierarchy)
                {
                    return m_ActiveSkillItems[i];
                }
            }
            return null;
        }

        private SkillItem CreateSkillItem(Entity entity)
        {
            SkillItem skillItem = null;
            SkillItemObject skillItemObject = m_SkillItemObjectPool.Spawn();
            if (skillItemObject != null)
            {
                skillItem = (SkillItem)skillItemObject.Target;
            }
            else
            {
                skillItem = Instantiate(m_SkillItemTemplate);
                Transform transform = skillItem.GetComponent<Transform>();
                transform.SetParent(entity.transform);
                m_SkillItemObjectPool.Register(SkillItemObject.Create(skillItem), true);
            }

            return skillItem;
        }

        private void CheckSkillType(TargetableObject owner, Skill skillData, bool isBuff = false)
        {
            switch (skillData.TypeEnum)
            {
                case SkillType.None:
                    CalculationWeaponDamage(owner, skillData, isBuff);
                    break;
                case SkillType.Circular:
                    CalculationCircular(owner, skillData, isBuff);
                    break;
                case SkillType.Sector:
                    CalculationSector(owner, skillData, isBuff);
                    break;
                case SkillType.Rectangle:
                    CalculationRectangle(owner, skillData, isBuff);
                    break;
                default:
                    break;
            }
        }

        private void CalculationWeaponDamage(TargetableObject owner, Skill skillData, bool isBuff)
        {
            for (int i = 0; i < owner.Weapons.Count; i++)
            {
                WeaponAttackPoint attackWeapon = owner.Weapons[i].GetComponent<WeaponAttackPoint>();
                if (attackWeapon != null)
                {
                    attackWeapon.SetAttackPointSkill(owner, skillData);
                }
            }

        }

        public void CalculationCircular(TargetableObject owner, Skill skillData, bool isBuff)
        {
            Collider[] colliders = Physics.OverlapSphere(owner.transform.position + skillData.CircularDistance * Vector3.forward, skillData.CircularRadius, 1 << LayerMask.NameToLayer("Targetable Object"));
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null)
                {
                    TargetableObject entity = colliders[i].gameObject.GetComponent<TargetableObject>();
                    if (entity == null)
                    {
                        entity = colliders[i].gameObject.GetComponentInParent<TargetableObject>();
                        if (entity == null)
                        {
                            continue;
                        }
                    }
                    if (isBuff)
                    {
                        TargetBuff targetBuff = new TargetBuff();
                        targetBuff.Target = entity;
                        targetBuff.Buff = skillData.Buffs;
                        GameEntry.Event.Fire(owner, ApplyBuffEventArgs.Create(targetBuff));
                    }
                    else
                    {
                        SkillCollision(owner, entity, skillData);
                    }

                }
            }
        }

        public void CalculationSector(TargetableObject owner, Skill skillData, bool isBuff)
        {
            Collider[] colliders = Physics.OverlapSphere(owner.transform.position, skillData.SectorRadius, 1 << LayerMask.NameToLayer("Targetable Object"));
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null)
                {
                    TargetableObject entity = colliders[i].gameObject.GetComponent<TargetableObject>();
                    if (entity == null)
                    {
                        entity = colliders[i].gameObject.GetComponentInParent<TargetableObject>();
                        if (entity == null)
                        {
                            continue;
                        }
                    }

                    Vector3 direction = entity.transform.position - owner.transform.position;
                    float degree = Vector3.Angle(direction, owner.transform.forward);
                    if (degree < skillData.SectorAngle / 2f)
                    {
                        SkillCollision(owner, entity, skillData);
                    }

                }
            }
        }

        public void CalculationRectangle(TargetableObject owner, Skill skillData, bool isBuff)
        {
            Collider[] colliders = Physics.OverlapBox(owner.transform.position + skillData.RectangleDistance * Vector3.forward, skillData.RectangleHalfExtents, Quaternion.identity, 1 << LayerMask.NameToLayer("Targetable Object"));
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null)
                {
                    TargetableObject entity = colliders[i].gameObject.GetComponent<TargetableObject>();
                    if (entity == null)
                    {
                        entity = colliders[i].gameObject.GetComponentInParent<TargetableObject>();
                        if (entity == null)
                        {
                            continue;
                        }
                    }
                    SkillCollision(owner, entity, skillData);
                }
            }
        }

        public void SkillCollision(TargetableObject entity, TargetableObject other, Skill skillData)
        {
            ImpactData entityImpactData = entity.GetImpactData();
            ImpactData otherImpactData = other.GetImpactData();
            if (AIUtility.GetRelation(entityImpactData.Camp, otherImpactData.Camp) == RelationType.Friendly)
            {
                return;
            }
            int entityDamageHP = CalcDamageHP(entityImpactData.Attack + skillData.Damage, otherImpactData.Defense);

            // AddTimer(entity, other, skillData.Duration, skillData.DamageFrequency, entityDamageHP);
            other.ApplySkillDamage(entity, entityDamageHP);//TODO：伤害频率的添加计算
        }

        private int CalcDamageHP(int attack, int defense)
        {
            //TODO: 根据人物属性 以及攻击力 计算伤害，
            return attack - defense;//a例子简单计算
        }


        public int AddTimer(TargetableObject owner, Skill skillData)
        {
            if (owner == null || skillData == null)
            {
                Log.Warning("添加技能计时器失败");
            }
            SkillTimer skillTimer = SkillTimer.Create(owner, skillData);
            m_SkillTimer.Add(skillTimer);
            return skillTimer.ID;
        }

        public void RemoveTimerSkill(int ID)
        {
            var item = m_SkillTimer.Find(a => a.ID == ID);
            if (item != null)
            {
                item.Clear();
                ReferencePool.Release(item);
                m_SkillTimer.Remove(item);
            }
        }

        //public void AddTimer(TargetableObject entity, TargetableObject other, float duration, float frequency,int entityDamageHP)
        //{
        //    if (duration < 0 || frequency < 0)
        //    {
        //        Log.Warning("添加技能计时器失败");
        //    }
        //    SkillTimer skillTimer = SkillTimer.Create(entity,other, duration, frequency, entityDamageHP);
        //    m_SkillTimer.Add(skillTimer);
        //}


        private class SkillTimer : IReference
        {
            private static int m_SerialId;

            static SkillTimer()
            {
                m_SerialId = 0;
            }

            public TargetableObject Owner
            {
                get;
                set;
            }

            public Skill SkillData
            {
                get;
                set;
            }

            public float Duration
            {
                get;
                set;
            }

            public float Frequency
            {
                get;
                set;
            }

            public int ID
            {
                get;
                private set;
            }

            public float LossTime
            {
                get;
                set;
            }

            public float EscapTime
            {
                get;
                set;
            }

            public static SkillTimer Create(TargetableObject owner, Skill skillData)
            {
                SkillTimer timer = ReferencePool.Acquire<SkillTimer>();
                timer.ID = m_SerialId++;
                timer.Owner = owner;
                timer.SkillData = skillData;
                timer.Duration = skillData.Duration;
                timer.Frequency = skillData.DamageFrequency;
                timer.LossTime = skillData.DamageFrequency;
                timer.EscapTime = 0;
                return timer;
            }

            public void Clear()
            {
                ID = -1;
                Duration = 0;
                Frequency = 0;
                LossTime = 0;
                Owner = null;
                SkillData = null;
                EscapTime = 0;
            }
        }

    }

    public class TargetBuff
    {
        private TargetableObject m_Target;
        private Buff m_Buff;

        public TargetableObject Target
        {
            get
            {
                return m_Target;
            }
            set
            {
                m_Target = value;
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
    }
}
