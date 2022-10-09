
using GameFramework;
using UMA;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    /// <summary>
    /// 武器类。
    /// </summary>
    public class WeaponLogicLeftHand : WeaponLogic
    {
        private const string AttachLeftPoint_Take = "Weapon_L";
        //private const string AttachLeftPoint_Put = "ShieldPos";

        private const string AttachLeftPointPlayer = "LeftHand";
        private const string AttachLeftPointPlayer_PutDown = "LeftShoulderAdjust";

    //private const string AttachRightPoint_TwoSwords_L_Take = "Weapon_TwoSwords_L";
        //private const string AttachRightPoint_TwoSwords_L_Put = "WeaponPos_TwoSwords_L";

        [SerializeField]
        private WeaponData m_WeaponData = null;

        public WeaponData weaponData => m_WeaponData;

        public bool IsShield;

        private float m_NextAttackTime = 0f;

        private UMAData m_UmaData;



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
            // GameEntry.Entity.AttachEntity(Entity, m_WeaponData.OwnerId, AttachRightPoint);
            m_UmaData = GameEntry.Entity.GetEntity(m_WeaponData.OwnerId).GetComponent<UMAData>();
            m_WeaponInfo = GameEntry.Entity.GetEntity(m_WeaponData.OwnerId).GetComponent<WeaponInfo>();
            if (m_UmaData != null)
            {
                //GameEntry.Entity.AttachEntity(Entity, m_WeaponData.OwnerId, m_UmaData.GetBoneGameObject(AttachLeftPointPlayer).transform);
                if (PlayerLogic.Instance.IsHands)
                {
                    GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachLeftPointPlayer);
                }
                else
                {
                    GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachLeftPointPlayer_PutDown);
                }
            }
            else
            {
                GameEntry.Entity.AttachEntityByFindChild(Entity, m_WeaponData.OwnerId, AttachLeftPoint_Take); 
            }
            IsShield = m_WeaponData.IsShield;
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


                Name = Utility.Text.Format("Shield of {0}", parentEntity.Name);

                //if (PlayerLogic.Instance.EquiState == EquiState.GiantSword)
                //{
                //    if (PlayerLogic.Instance.IsHands)
                //    {
                //        SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.GiantSword].leftHand);
                //    }
                //    else
                //    {
                //        SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.GiantSword_PutDown].leftHand);
                //    }
                //}
                switch (PlayerLogic.Instance.EquiState)
                {
                    case EquiState.None:
                        break;
                    case EquiState.SwordShield:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.SwordShield].leftHand);
                        }
                        else
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.SwordShield_PutDown].leftHand);
                        }
                        break;
                    case EquiState.GiantSword:
                        break;
                    case EquiState.Dagger:
                        break;
                    case EquiState.DoubleBlades:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.DoubleBlades].leftHand);
                        }
                        else
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.DoubleBlades_PutDown].leftHand);
                        }
                        break;
                    case EquiState.Pistol:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.Pistol].leftHand);
                        }
                        else
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.Pistol_PutDown].leftHand);
                        }
                        break;
                    case EquiState.RevengerDoubleBlades:
                        if (PlayerLogic.Instance.IsHands)
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.RevengerDoubleBlades].leftHand);
                        }
                        else
                        {
                            SetTransform(m_WeaponInfo.weaponDir[WeaponEnum.DoubleBlades_PutDown].leftHand);
                        }
                        break;
                    default:
                        break;
                }

            }
            else
            {
                Name = Utility.Text.Format("Shield of {0}", parentEntity.Name);
                CachedTransform.localPosition = Vector3.zero;
                CachedTransform.localRotation = Quaternion.identity;
            }

            
        }

        private void SetTransform(WeaponInfoTransform infoTransform)
        {
            CachedTransform.localPosition = infoTransform.position;
            CachedTransform.localRotation = infoTransform.rotation;
            CachedTransform.localScale = infoTransform.scale;

        }

        public ImpactData GetImpactData()
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
