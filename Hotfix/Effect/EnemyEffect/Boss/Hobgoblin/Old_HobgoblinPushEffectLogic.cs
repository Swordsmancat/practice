using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    public class Old_HobgoblinPushEffectLogic : EffectLogic
    {
        private Old_HobgoblinLogic _owner;
        private readonly static string chongci = "chongci";
        private EffectData m_Effeta;
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_Effeta = userData as EffectData;
            if(m_Effeta != null)
            {
                _owner = (Old_HobgoblinLogic)m_Effeta.Owner;
                _owner.IsEffectAttack = false;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

    }
}

