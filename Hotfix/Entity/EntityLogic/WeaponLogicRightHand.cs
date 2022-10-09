
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using UMA;

namespace Farm.Hotfix
{
    /// <summary>
    /// 武器类。
    /// </summary>
    public class WeaponLogicRightHand : WeaponLogic
    {
        private const string AttachRightPoint_Take = "Weapon_R";
        //private const string AttachRightPoint_Put = "WeaponPos"; 
        
        private const string AttachRightPointPlayer = "RightHand";
        private const string AttachRightPointPlayer_PutDown = "RightShoulderAdjust";

        //private const string AttachRightPoint_ShortHand = "Weapon_ShortHand";
        //private const string AttachRightPoint_TwoHands = "Weapon_TwoHands";
        //private const string AttachRightPoint_ShortHand_Take = "Weapon_ShortHand";
        //private const string AttachRightPoint_ShortHand_Put = "WeaponPos_ShortHand";

        //private const string AttachRightPoint_TwoHands_Take = "Weapon_TwoHands";
        //private const string AttachRightPoint_TwoHands_Put = "WeaponPos_TwoHands";

        //private const string AttachRightPoint_TwoSwords_R_Take = "Weapon_TwoSwords_R";
        //private const string AttachRightPoint_TwoSwords_R_Put = "WeaponPos_TwoSwords_R";

        [SerializeField]
        private WeaponData m_WeaponData = null;

        public WeaponData weaponData => m_WeaponData;

        private UMAData m_UmaData;

        private float m_NextAttackTime = 0f;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_WeaponData = userData as WeaponData;
            if (m_WeaponData == null)
            {
                Log.Error("Weapon data is invalid.");
                return;
            }
            m_UmaData = GameEntry.Entity.GetEntity(m_WeaponData.OwnerId).GetComponent<UMAData>();
            m_WeaponInfo = GameEntry.Entity.GetEntity(m_WeaponData.OwnerId).GetComponent<WeaponInfo>();
            if (m_UmaData != null)
            {
                if (PlayerLogic.Instance.IsHands)
                {
                    GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachRightPointPlayer);
                }
                else
                {
                    GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachRightPointPlayer_PutDown);
                }
            }
            else
            {
                GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachRightPoint_Take);
            }
        }


        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
            if (m_UmaData != null)
            {
                if (m_WeaponInfo == null)
                {
                    Log.Warning("Weaponinfo invalid");
                    return;
                }

                
                switch (PlayerLogic.Instance.EquiState)
                {
                    case EquiState.SwordShield:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.SwordShield].rightHand);
                        }
                        else
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.SwordShield_PutDown].rightHand);
                        }
                        break;
                    case EquiState.GiantSword:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.GiantSword].rightHand);
                        }
                        else
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.GiantSword_PutDown].rightHand);
                        }
                        break;
                    case EquiState.Dagger:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.Dagger].rightHand);
                        }
                        else
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.Dagger_PutDown].rightHand);
                        }
                        break;
                    case EquiState.DoubleBlades:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.DoubleBlades].rightHand);
                        }
                        else
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.DoubleBlades_PutDown].rightHand);
                        }
                        break;
                    case EquiState.Pistol:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.Pistol].rightHand);
                        }
                        else
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.Pistol_PutDown].rightHand);
                        }
                        break;
                    case EquiState.RevengerDoubleBlades:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.RevengerDoubleBlades].rightHand);
                        }
                        else
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.DoubleBlades_PutDown].rightHand);
                        }
                        break;
                    default:
                        SetTransform(Vector3.zero, Quaternion.identity, Vector3.one);
                        break;

                }
            }
            else
            {
                CachedTransform.localPosition = Vector3.zero;
                CachedTransform.localRotation = Quaternion.identity;
                CachedTransform.localScale = Vector3.one;
            }

            Name = Utility.Text.Format("Weapon of {0}", parentEntity.Name);
        }

        private void SetTransform(WeaponInfoTransform infoTransform)
        {
            CachedTransform.localPosition = infoTransform.position;
            CachedTransform.localRotation = infoTransform.rotation;
            CachedTransform.localScale = infoTransform.scale;

        }

        private void SetTransform(Vector3 position, Quaternion rotation,Vector3 scale)
        {
            CachedTransform.localPosition = position;
            CachedTransform.localRotation = rotation;
            CachedTransform.localScale = scale;

        }

        public  ImpactData GetImpactData()
        {
            return new ImpactData(m_WeaponData.OwnerCamp, 0, m_WeaponData.Attack, 0);
        }


        public void TryAttack()
        {
            if (Time.time < m_NextAttackTime)
            {
                return;
            }

            m_NextAttackTime = Time.time + m_WeaponData.AttackInterval;
            GameEntry.Sound.PlaySound(m_WeaponData.BulletSoundId);
        }

        
    }
}
