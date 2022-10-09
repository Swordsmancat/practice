using System;
using UnityEngine;
using GameFramework.DataTable;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Farm.Hotfix
{
    [Serializable]
   public class PlayerData:TargetableObjectData
    {

        [Title("基础属性")]
        [SerializeField,GUIColor(1,0,0),HideLabel][LabelText("最大血量")]
        private int m_MaxHP = 0;

        [SerializeField, GUIColor(1, 0, 0), HideLabel]
        [LabelText("最大精力")]
        private int m_MaxMP = 0;
        [SerializeField,GUIColor(0,0,1),HideLabel][LabelText("护甲")]
        private int m_Defense = 0;

        private int m_Attack = 0;

        [SerializeField,HideLabel][LabelText("武器列表")][Searchable][TableList]
        private List<WeaponData> m_WeaponDatas = new List<WeaponData>();

        [SerializeField, HideLabel][LabelText("护甲列表")]
        [TableList]
        [Searchable]
        private List<ArmorData> m_ArmorDatas = new List<ArmorData>();

        [SerializeField,HideLabel]
        [LabelText("死亡特效ID")]
        private int m_DeadEffectId = 0;

        [SerializeField, HideLabel]
        [LabelText("死亡音效ID")]
        private int m_DeadSoundId = 0;

        [SerializeField, HideLabel]
        [LabelText("行走声音ID")]
        private int m_WalkSoundId = 0;

        [SerializeField, HideLabel]
        [LabelText("跑步声音ID")]
        private int m_RunSoundId = 0;

        [SerializeField, HideLabel]
        [LabelText("翻滚声音ID")]
        private int m_DodgeSoundId = 0;

        private int m_EquipWeaponSoundId = 0;

        private int m_KnockDownSoundId = 0;

        private AbilityDataPlayer m_AbilityData;




        public PlayerData(int entityId,int typeId,CampType camp) : base(entityId, typeId, camp)
        {
            IDataTable<DRCharacter> dtCharacters = GameEntry.DataTable.GetDataTable<DRCharacter>();
            DRCharacter drCharacter = dtCharacters.GetDataRow(typeId);
            if(drCharacter == null)
            {
                return;
            }

            for (int index = 0, weaponId = 0; (weaponId = drCharacter.GetWeaponIdAt(index)) > 0; index++)
            {
                AttachWeaponData(new WeaponData(GameEntry.Entity.GenerateSerialId(), weaponId, Id, Camp));
            }

            for (int index = 0, armorId =0; (armorId =drCharacter.GetArmorId(index))> 0; index++)
            {
                AttachArmorData(new ArmorData(GameEntry.Entity.GenerateSerialId(), armorId, Id, camp));
            }
            m_WalkSoundId = drCharacter.MoveSoundId0;
            m_RunSoundId = drCharacter.MoveSoundId1;
            m_DodgeSoundId = drCharacter.DodgeSoundId;
            m_EquipWeaponSoundId = drCharacter.EquipWeaponSoundId;
            m_KnockDownSoundId = drCharacter.KnockDownSoundId;
            m_AbilityData = new AbilityDataPlayer();
            m_AbilityData.OnInit();
            m_MaxHP += m_AbilityData.HPBase;
            HP = m_MaxHP;
            m_Defense += m_AbilityData.ArmorBase;
            m_Attack += m_AbilityData.AttackBase;
            m_MaxMP = m_AbilityData.MPBase;
            MP = m_AbilityData.MPBase;
        }

        /// <summary>
        /// 最大生命。
        /// </summary>
        public override int MaxHP
        {
            get
            {
                return m_MaxHP;
            }
        }
        /// <summary>
        /// 最大精力值
        /// </summary>
        public override int MaxMP
        {
            get
            {
                return m_MaxMP;
            }
        }

        /// <summary>
        /// 防御。
        /// </summary>
        public int Defense
        {
            get
            {
                return m_Defense;
            }
        }

        public int Attack
        {
            get
            {
                return m_Attack;
            }
        }

        /// <summary>
        /// 速度。
        /// </summary>
        //public float Speed
        //{
        //    get
        //    {
        //        return m_ThrusterData.Speed;
        //    }
        //}

        public int DeadEffectId
        {
            get
            {
                return m_DeadEffectId;
            }
        }

        public int DeadSoundId
        {
            get
            {
                return m_DeadSoundId;
            }
        }

        public int WalkSoundId
        {
            get
            {
                return m_WalkSoundId;
            }
        }

        public int RunSoundId
        {
            get
            {
                return m_RunSoundId;
            }
        }

        public int DodgeSoundId
        {
            get
            {
                return m_DodgeSoundId;
            }
        }

        public int EquipWeaponSoundId
        {
            get
            {
                return m_EquipWeaponSoundId;
            }
        }

        public int KnockDownSoundId
        {
            get
            {
                return m_KnockDownSoundId;
            }
        }

        public List<ArmorData> GetAllArmorDatas()
        {
            return m_ArmorDatas;
        }
        public void AttachArmorData(ArmorData armorData)
        {
            if (armorData == null)
            {
                return;
            }

            if (m_ArmorDatas.Contains(armorData))
            {
                return;
            }

            m_ArmorDatas.Add(armorData);
            RefreshData();
        }

        public void DetachArmorData(ArmorData armorData)
        {
            if (armorData == null)
            {
                return;
            }

            m_ArmorDatas.Remove(armorData);
            RefreshData();
        }

        public List<WeaponData> GetAllWeaponDatas()
        {
            return m_WeaponDatas;
        }
        public void AttachWeaponData(WeaponData weaponData)
        {
            if (weaponData == null)
            {
                return;
            }

            if (m_WeaponDatas.Contains(weaponData))
            {
                return;
            }

            m_WeaponDatas.Add(weaponData);
            m_Attack += weaponData.Attack;
        }

        public void DetachWeaponData(WeaponData weaponData)
        {
            if (weaponData == null)
            {
                return;
            }

            m_WeaponDatas.Remove(weaponData);
            m_Attack -= weaponData.Attack;
        }



        private void RefreshData()
        {
            m_MaxHP = 0;
            m_Defense = 0;
            for (int i = 0; i < m_ArmorDatas.Count; i++)
            {
                m_MaxHP += m_ArmorDatas[i].MaxHP;
                m_Defense += m_ArmorDatas[i].Defense;
            }

            if (HP > m_MaxHP)
            {
                HP = m_MaxHP;
            }
        }


    }
}
