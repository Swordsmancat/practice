using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class EquipWeapon : WeaponLogic
    {
        PlayerLogic m_PlayerLogic;

        private Transform weapon_L_Take;
        private Transform weapon_R_Take;
        private Transform weapon_L_Put;
        private Transform weapon_R_Put;

        private string R_PutPathName = "RightShoulderAdjust";
        private string L_PutPathName = "LeftShoulderAdjust";
        private string L_TakePathName = "LeftHand";
        private string R_TakePathName = "RightHand";

        Transform RightWeapon;
        Transform LeftWeapon;



        private void Start()
        {
            m_PlayerLogic = gameObject.GetComponent<PlayerLogic>();
            weapon_R_Put = GameEntry.Entity.FindFunc(transform, R_PutPathName);
            weapon_L_Put = GameEntry.Entity.FindFunc(transform, L_PutPathName);
            weapon_R_Take = GameEntry.Entity.FindFunc(transform, R_TakePathName);
            weapon_L_Take = GameEntry.Entity.FindFunc(transform, L_TakePathName);
        }



        public void PutDownWeaponAnim()
        {
            switch (m_PlayerLogic.EquiState)
            {
                case EquiState.SwordShield:
                    SetWeaponParent(weapon_R_Put, weapon_L_Put, WeaponEnum.SwordShield_PutDown);
                    break;
                case EquiState.GiantSword:
                    SetWeaponParent(weapon_R_Put, null, WeaponEnum.GiantSword_PutDown);
                    break;
                case EquiState.Dagger:
                    SetWeaponParent(weapon_R_Put, null, WeaponEnum.Dagger_PutDown);
                    break;
                case EquiState.DoubleBlades:
                    SetWeaponParent(weapon_R_Put, weapon_L_Put, WeaponEnum.DoubleBlades_PutDown);
                    break;
                case EquiState.Pistol:
                    SetWeaponParent(weapon_R_Put, null, WeaponEnum.Pistol_PutDown);
                    break;
                case EquiState.RevengerDoubleBlades:
                    SetWeaponParent(weapon_R_Put, weapon_L_Take, WeaponEnum.DoubleBlades_PutDown);
                    break;
                default:
                    break;
            }
        }

        public void TakeOutWeaponAnim()
        {
            switch (m_PlayerLogic.EquiState)
            {
                case EquiState.SwordShield:
                    SetWeaponParent(weapon_R_Take, weapon_L_Take, WeaponEnum.SwordShield);
                    break;
                case EquiState.GiantSword:
                    SetWeaponParent(weapon_R_Take, null, WeaponEnum.GiantSword);
                    break;
                case EquiState.Dagger:
                    SetWeaponParent(weapon_R_Take, null, WeaponEnum.Dagger);
                    break;
                case EquiState.DoubleBlades:
                    Debug.Log("TakeOutWeaponFour");
                    SetWeaponParent(weapon_R_Take, weapon_L_Take, WeaponEnum.DoubleBlades);
                    break;
                case EquiState.Pistol:
                    SetWeaponParent(weapon_R_Take, null, WeaponEnum.Pistol);
                    break;
                case EquiState.RevengerDoubleBlades:
                    SetWeaponParent(weapon_R_Take, weapon_L_Take, WeaponEnum.RevengerDoubleBlades);
                    break;
                default:
                    break;
            }
        }


        private void SetWeaponParent(Transform putPos1, Transform putPos2, WeaponEnum m_weaponEnum)
        {
            FindWeaponTrans();
            RightWeapon.SetParent(putPos1);
            if (LeftWeapon != null)
            {
                LeftWeapon.SetParent(putPos2);
            }
            SetWeaponTrans(RightWeapon, LeftWeapon, m_weaponEnum);
        }
        private void SetWeaponTrans(Transform R, Transform L, WeaponEnum we)
        {
            if (R)
            {
                R.localPosition = m_PlayerLogic.m_WeaponInfo.weaponDir[we].rightHand.position;
                R.localRotation = m_PlayerLogic.m_WeaponInfo.weaponDir[we].rightHand.rotation;
                R.localScale = m_PlayerLogic.m_WeaponInfo.weaponDir[we].rightHand.scale;
            }
            if (L)
            {
                L.localPosition = m_PlayerLogic.m_WeaponInfo.weaponDir[we].leftHand.position;
                L.localRotation = m_PlayerLogic.m_WeaponInfo.weaponDir[we].leftHand.rotation;
                L.localScale = m_PlayerLogic.m_WeaponInfo.weaponDir[we].leftHand.scale;

            }
            RightWeapon = null;
            LeftWeapon = null;
        }
        private void FindWeaponTrans()
        {
            for (int i = 0; i < m_PlayerLogic.Weapons.Count; i++)
            {
                if (m_PlayerLogic.Weapons[i] is WeaponLogicRightHand)
                {
                    RightWeapon = m_PlayerLogic.Weapons[i].transform;
                }
                if (m_PlayerLogic.Weapons[i] is WeaponLogicLeftHand)
                {
                    LeftWeapon = m_PlayerLogic.Weapons[i].transform;
                }
            }
        }

    }

}
