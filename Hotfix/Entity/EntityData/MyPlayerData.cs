using System;
using UnityEngine;

namespace Farm.Hotfix
{
    [Serializable]
  public  class MyPlayerData :PlayerData
    {
        [SerializeField]
        private string m_Name = null;

        public MyPlayerData(int entityId, int typeId)
           : base(entityId, typeId, CampType.Player)
        {
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

    }
}
