using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerAttackSound : MonoBehaviour
    {

        private WeaponData m_WeaponDataRight = null;
        private WeaponData m_WeaponDataLeft = null;
        private PlayerLogic m_PlayerLogic = null;

        bool isFive = false;
        private void Start()
        {
            m_PlayerLogic = gameObject.GetComponent<PlayerLogic>();
            UpdateWeaponSound();
        }

        
        private void Update()
        {
            if(m_PlayerLogic.isChangeWeapon)
            {
                UpdateWeaponSound();
            }

        }

        private void UpdateWeaponSound()
        {
            //Log.Info(m_PlayerLogic.Weapons.Count);
            for (int i = 0; i < m_PlayerLogic.Weapons.Count; i++)
            {
                //»ñÈ¡ÓÒÊÖÎäÆ÷ÉùÒô
                if (m_PlayerLogic.Weapons[i] is WeaponLogicRightHand)
                {
                    var rightHand = m_PlayerLogic.Weapons[i] as WeaponLogicRightHand;
                    m_WeaponDataRight = rightHand.weaponData;
                }
                else if (m_PlayerLogic.Weapons[i] is WeaponLogicLeftHand)
                {
                    var letfHand = m_PlayerLogic.Weapons[i] as WeaponLogicLeftHand;
                    m_WeaponDataLeft = letfHand.weaponData;
                }
            }
            m_PlayerLogic.isChangeWeapon = false;
        }

    }
}

