using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerHurtEffectLogic : EffectLogic
    {

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            transform.SetPositionAndRotation(m_EffectData.Owner.transform.position + Vector3.up, 
                Quaternion.Euler(0, m_EffectData.Owner.transform.rotation.eulerAngles.y + Utility.Random.GetRandom(0,45),0));
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

    }
}

