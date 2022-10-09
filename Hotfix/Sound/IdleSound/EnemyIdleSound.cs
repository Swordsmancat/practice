using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Farm.Hotfix
{
    public class EnemyIdleSound : MonoBehaviour
    {
        private PlayerData m_PlayerData;

        void Start()
        {
            PlayerLogic m_PlayerLogic = gameObject.GetComponent<PlayerLogic>();
            m_PlayerData = m_PlayerLogic.PlayerData;
        }

        public void EnemyLookPlayerSound()
        {

        }
    }
}

