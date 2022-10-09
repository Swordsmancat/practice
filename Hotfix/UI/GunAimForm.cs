using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.Events;


namespace Farm.Hotfix
{
    public class GunAimForm : UGuiForm
    {
        [SerializeField]
        private Image m_GunAimImage;

        private ProcedureMain m_procedureMain;

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
            m_GunAimImage.gameObject.SetActive(false);
        }

        public void ShowGunAim()
        {
            m_GunAimImage.gameObject.SetActive(true);
        }

        public void HideGunAim()
        {
            m_GunAimImage.gameObject.SetActive(false);
        }
    }
}

