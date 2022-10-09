using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    public class PlayerMotionSound : MonoBehaviour
    {
        private PlayerData m_PlayerData;
        private PlayerLogic m_PlayerLogic;
        private void Start()
        {
            m_PlayerLogic = gameObject.GetComponent<PlayerLogic>();
            m_PlayerData = m_PlayerLogic.PlayerData;
        }

        public void PlayerWalkSound()
        {
            if (m_PlayerLogic.isMotion) return;
            GameEntry.Sound.PlaySound(m_PlayerData.WalkSoundId);
        }

        public void PlayerRunSound()
        {
            if (m_PlayerLogic.m_Attack) return;
            GameEntry.Sound.PlaySound(m_PlayerData.RunSoundId);
        }

        public void PlayerDodgeSound()
        {
            GameEntry.Sound.PlaySound(m_PlayerData.DodgeSoundId);
        }

        public void PlayerEquipWeaponSound()
        {
            GameEntry.Sound.PlaySound(m_PlayerData.EquipWeaponSoundId);
        }

        public void PlayerKnockDownSound()
        {
            GameEntry.Sound.PlaySound(m_PlayerData.KnockDownSoundId);
        }
    }
}

