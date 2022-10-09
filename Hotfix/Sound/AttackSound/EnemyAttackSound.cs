using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Farm.Hotfix
{
    public class EnemyAttackSound : MonoBehaviour
    {
        private WeaponData m_WeaponDataRight;
        private WeaponData m_WeaponDataLeft;

        private void Start()
        {
            EnemyLogic m_EnemyLogic = gameObject.GetComponent<EnemyLogic>();

            for (int i = 0; i < m_EnemyLogic.Weapons.Count; i++)
            {
                //获取右手武器声音
                //若右手无武器获得左手
                if(m_EnemyLogic.Weapons[i] is WeaponLogicRightHand)
                {
                    var rightHand = m_EnemyLogic.Weapons[i] as WeaponLogicRightHand;
                    m_WeaponDataRight = rightHand.weaponData;
                }
                else if(m_EnemyLogic.Weapons[i] is WeaponLogicLeftHand)
                {
                    var leftHand = m_EnemyLogic.Weapons[i] as WeaponLogicLeftHand;
                    m_WeaponDataLeft = leftHand.weaponData;
                }
            }
        }

    }

}

