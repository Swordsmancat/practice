using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityGameFramework.Runtime;
using System.Collections;
using GameFramework.Event;

namespace Farm.Hotfix
{
    public class LoadingForm:UGuiForm
    {
        [SerializeField]
        public Slider slider_Process;

        [SerializeField]
        public Text m_text;

        private ProcedureChangeScene m_ProcedureChangeScene = null;


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_ProcedureChangeScene = (ProcedureChangeScene)userData;
            if(m_ProcedureChangeScene == null)
            {
                Log.Warning("ProcedureChangeScene is invalid when open LoadingPanel");
                return;
            }
            Log.Info("加载页面");

        }


        protected override void OnClose(bool isShutdown, object userData)
        {

            m_ProcedureChangeScene = null;
            base.OnClose(isShutdown, userData);
        }
    }
}
