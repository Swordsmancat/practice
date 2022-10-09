//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/9/20/周二 14:36:37
//------------------------------------------------------------
using UnityEngine;

namespace Farm.Hotfix
{
    public class ArrowData : EntityData
    {
        private int m_OwnerId = 0;

        private CampType m_OwnerCamp = CampType.Unknown;

        private int m_Attack = 0;

        private Vector3 m_ArrowImpulse;

        private Transform m_Parent;

        private bool m_IsLock;
        public ArrowData(int entityId, int typeId,int ownerId,CampType ownerCamp,int attack) : base(entityId, typeId)
        {
            m_OwnerId = ownerId;
            m_OwnerCamp = ownerCamp;
            m_Attack = attack;
        }

        public int OwnerId
        {
            get
            {
                return m_OwnerId;
            }
        }

        public CampType OwnerCamp
        {
            get
            {
                return m_OwnerCamp;
            }
        }

        public int Attack
        {
            get
            {
                return m_Attack;
            }
        }

        public Vector3 ArrowImpulse
        {
            get
            {
                return m_ArrowImpulse;
            }
            set
            {
                m_ArrowImpulse = value;
            }
        }

        public Transform Parent
        {
            get
            {
                return m_Parent;
            }
            set
            {
                m_Parent = value;
            }
        }

        public bool IsLock
        {
            get
            {
                return m_IsLock;
            }
            set
            {
                m_IsLock = value;
            }
        }
    }
}
