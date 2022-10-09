
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using UMA;

namespace Farm.Hotfix
{
    /// <summary>
    /// 武器类。
    /// </summary>
    public class WeaponLogic : Entity
    {
        protected WeaponInfo m_WeaponInfo;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
        }



    }
}
