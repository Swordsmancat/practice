using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    /// <summary>
    /// 特效类。
    /// </summary>
    public class EffectLogic : Entity
    {
        [SerializeField]
        protected EffectData m_EffectData = null;
        
        private float m_ElapseSeconds = 0f;

        private float m_KeepTime =0;

        private bool m_IsFollow = false;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_EffectData = userData as EffectData;
            if (m_EffectData == null)
            {
                Log.Error("Effect data is invalid.");
                return;
            }
            m_ElapseSeconds = 0f;
            m_KeepTime = m_EffectData.KeepTime;
            m_IsFollow = m_EffectData.IsFollow;

            if (m_EffectData.IsFollow)
            {
                GameEntry.Entity.AttachEntityByFindChild(Entity, m_EffectData.Owner.Id, m_EffectData.ParentName);
            }
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if(m_KeepTime > 0)
            {
                m_ElapseSeconds += elapseSeconds;
                if (m_ElapseSeconds >= m_KeepTime)
                {
                    GameEntry.Entity.HideEntity(this);
                }
            }
         
        }

        /// <summary>
        /// 获取效果开始点
        /// </summary>
        /// <param name="owner">拥有该位置的对象</param>
        /// <param name="childName">开始点的名字</param>
        /// <returns>开始点对象</returns>
        protected GameObject GetEffectStartPoint(in GameObject owner, in string childName)
        {
            if (owner != null)
                return FindTools.FindFunc(owner.transform, childName).gameObject;

            return null;
        }


    }
}
