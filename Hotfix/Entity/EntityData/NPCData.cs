using GameFramework.DataTable;
using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Farm.Hotfix
{
    [Serializable]
    public class NPCData : EntityData
    {

        private int m_Options;

        private bool m_IsDiglog;

        private bool m_IsTransaction;
        public NPCData(int entityId ,int typeId) : base(entityId, typeId)
        {
            IDataTable<DRNPC> dtEnemies = GameEntry.DataTable.GetDataTable<DRNPC>();
            DRNPC drEnemy = dtEnemies.GetDataRow(TypeId);
            if(drEnemy == null)
            {
                return;
            }
            m_Options = drEnemy.Options;
            m_IsDiglog = drEnemy.Diglog;
            m_IsTransaction = drEnemy.Transaction;
        }

        public int Options
        {
            get
            {
                return m_Options;
            }
        }

        public bool IsDiglog
        {
            get
            {
                return m_IsDiglog;
            }
        }

        public bool IsTransaction
        {
            get
            {
                return m_IsTransaction;
            }
        }
    }
}
