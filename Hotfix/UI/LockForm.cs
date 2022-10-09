using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Farm.Hotfix
{
    public class LockForm:UGuiForm
    {

        [SerializeField]
        private Image m_LockImage;

        private ProcedureMain m_procedureMain;

        private Transform m_LockTransform;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_procedureMain = (ProcedureMain)userData;
            if (m_procedureMain == null)
            {
                Log.Warning("m_procedureMain is invalid when open LockFormPanel");
                return;
            }
            m_LockImage.gameObject.SetActive(false);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_LockTransform != null) 
            {
                this.transform.position = Camera.main.WorldToScreenPoint(m_LockTransform.position);
            }

        }

        public void ShowLock(Transform transform)
        {
            m_LockTransform = transform;
            this.transform.position = Camera.main.WorldToScreenPoint(m_LockTransform.position);
            //  Vector3 worldPosition = transform.position;
            //this.transform.localPosition = worldPosition;
            m_LockImage.gameObject.SetActive(true);
        }

        public void HideLock()
        {
            m_LockTransform = null;
            m_LockImage.gameObject.SetActive(false);
        }

    }
}
