
using GameFramework.DataTable;
using System;
using UnityEngine;

namespace Farm.Hotfix
{
    [Serializable]
    public class ArmorData : AccessoryObjectData
    {
        [SerializeField]
        private int m_MaxHP = 0;

        [SerializeField]
        private int m_Defense = 0;

        [SerializeField]
        private int m_HurtSoundId = 0;

        [SerializeField]
        private int m_HurtEffectId = 0;

       [SerializeField]
        private int m_StoicHurtSoundId = 0;


        [SerializeField]
        private int m_StoicHurtEffectId = 0;
		[SerializeField]
        private int m_HurtSoundId2 = 0;

        public ArmorData(int entityId, int typeId, int ownerId, CampType ownerCamp)
            : base(entityId, typeId, ownerId, ownerCamp)
        {
            IDataTable<DRArmor> dtArmor = GameEntry.DataTable.GetDataTable<DRArmor>();
            DRArmor drArmor = dtArmor.GetDataRow(TypeId);
            if (drArmor == null)
            {
                return;
            }

            m_MaxHP = drArmor.MaxHP;
            m_Defense = drArmor.Defense;
            m_HurtSoundId = drArmor.HurtSoundId;
            m_HurtSoundId2 = drArmor.HurtSoundId2;
            m_HurtEffectId = drArmor.HurtEffectId;
            m_StoicHurtSoundId = drArmor.StoicHurtSoundId;
            m_StoicHurtEffectId = drArmor.StoicHurtEffectId;
        }

        /// <summary>
        /// 最大生命。
        /// </summary>
        public int MaxHP
        {
            get
            {
                return m_MaxHP;
            }
        }

        /// <summary>
        /// 防御力。
        /// </summary>
        public int Defense
        {
            get
            {
                return m_Defense;
            }
        }
        /// <summary>
        /// 击中声音
        /// </summary>
        public int HurtSoundId
        {
            get
            {
                return m_HurtSoundId;
            }
        }

        public int HurtSoundId2
        {
            get
            {
                return m_HurtSoundId2;
            }
        }

        public int HurtEffectId
        {
            get
            {
                return m_HurtEffectId;
            }
        }

        public int StoicHurtSoundId
        {
            get
            {
                return m_StoicHurtSoundId;
            }
        }

        public int StoicHurtEffectId
        {
            get
            {
                return m_StoicHurtEffectId;
            }
        }
    }
}
