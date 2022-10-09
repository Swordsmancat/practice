using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Farm.Hotfix
{
    [Serializable]
   public abstract class TargetableObjectData :EntityData
    {
        [SerializeField]
        private CampType m_Camp = CampType.Unknown;

        [SerializeField, GUIColor(1, 0, 0), HideLabel]
        [LabelText("当前血量")]
        private float m_HP = 0;

        [SerializeField, GUIColor(0, 0, 1), HideLabel]
        [LabelText("当前精力")]
        private float m_MP = 0;

        [SerializeField]
        private int m_AttackBase = 0;

        [SerializeField]
        private int m_ArmorBase = 0;


        public TargetableObjectData(int entityId ,int typeId,CampType camp):base(entityId,typeId)
        {
            m_Camp = camp;
            m_HP = 0;
            m_MP = 0;
        }


        /// <summary>
        /// 角色阵营。
        /// </summary>
        public CampType Camp
        {
            get
            {
                return m_Camp;
            }
        }

        /// <summary>
        /// 当前生命。
        /// </summary>
        public float HP
        {
            get
            {
                return m_HP;
            }
            set
            {
                m_HP = value;
            }
        }

        /// <summary>
        /// 最大生命。
        /// </summary>
        public abstract int MaxHP
        {
            get;
        }
        public abstract int MaxMP
        {
            get;
        }

        /// <summary>
        /// 最大精力
        /// </summary>
        //public abstract int MaxMP
        //{
        //    get;
        //}
        /// <summary>
        /// 生命百分比。
        /// </summary>
        public float HPRatio
        {
            get
            {
                return MaxHP > 0 ? (float)HP / MaxHP : 0f;
            }
        }

        public float MPRatio
        {
            get
            {
                return MaxMP > 0 ? (float)MP / MaxMP : 0f;
            }
        }

        public float MP
        {
            get
            {
                return m_MP;
            }
            set
            {
                m_MP = value;
            }
        }

        public int AttackBase
        {
            get
            {
                return m_AttackBase;
            }
            set
            {
                m_AttackBase = value;
            }
        }

        public int ArmorBase
        {
            get
            {
                return m_ArmorBase;
            }
            set
            {
                m_ArmorBase = value;
            }
        }
    }
}
