using System;
using UnityEngine;

namespace Farm.Hotfix
{
  public  class EffectData :EntityData
    {
        [SerializeField]
        private float m_KeepTime = 10f;

        [SerializeField]
        private Vector3 m_Position;

        private TargetableObject owner;

        private bool m_IsFollow = false;

        private string m_ParentName;

        public EffectData(int entityId, int typeId=0)
           : base(entityId, typeId)
        {
        }

        public string ParentName
        {
            get
            {
                return m_ParentName;
            }
            set
            {
                m_ParentName = value;
            }
        }

        public bool IsFollow
        {
            get
            {
                return m_IsFollow;
            }
            set
            {
                m_IsFollow = value;
            }
        }

        public float KeepTime
        {
            get
            {
                return m_KeepTime;
            }
            set
            {
                m_KeepTime = value;
            }
        }

        public TargetableObject Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
            }
        }


    }
}
