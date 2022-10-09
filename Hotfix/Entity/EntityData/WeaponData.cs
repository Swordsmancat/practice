
using GameFramework.DataTable;
using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Farm.Hotfix
{
    [Serializable]
    public class WeaponData : AccessoryObjectData
    {
        [SerializeField,HideLabel,LabelText("攻击力")]
        private int m_Attack = 0;

        [SerializeField]
        private float m_AttackInterval = 0f;

        [SerializeField]
        private int m_BulletSoundId = 0;

        private bool isShield;

        private EquiState m_EquiState;


        //武器声音
        public List<int> WeaponHitSounds;

        //武器击中物体效果
        private int m_WeaponHitEffectId = 0;

        public WeaponData(int entityId, int typeId, int ownerId, CampType ownerCamp)
            : base(entityId, typeId, ownerId, ownerCamp)
        {
            IDataTable<DRWeapon> dtWeapon = GameEntry.DataTable.GetDataTable<DRWeapon>();
            DRWeapon drWeapon = dtWeapon.GetDataRow(TypeId);
            if (drWeapon == null)
            {
                return;
            }
            WeaponHitSounds = new List<int>();

            m_Attack = drWeapon.Attack;
            isShield = Convert.ToBoolean(drWeapon.IsShield);
            m_AttackInterval = drWeapon.AttackInterval;
            m_WeaponHitEffectId = drWeapon.WeaponHitEffectId;
            WeaponHitSounds = drWeapon.WeaponHitSoundList;
        }


        /// <summary>
        /// 攻击力。
        /// </summary>
        public int Attack
        {
            get
            {
                return m_Attack;
            }
        }

        /// <summary>
        /// 攻击间隔。
        /// </summary>
        public float AttackInterval
        {
            get
            {
                return m_AttackInterval;
            }
        }

        public EquiState EquiState
        {
            get
            {
                return m_EquiState;
            }
        }

        /// <summary>
        /// 子弹声音编号。
        /// </summary>
        public int BulletSoundId
        {
            get
            {
                return m_BulletSoundId;
            }
        }

        /// <summary>
        /// 武器击中物体效果
        /// </summary>
        public int WeaponHitEffectId
        {
            get
            {
                return m_WeaponHitEffectId;
            }
        }

        /// <summary>
        /// 是否是盾牌
        /// </summary>
        public bool IsShield
        {
            get
            {
                return isShield;
            }
        }

        

    }
}
