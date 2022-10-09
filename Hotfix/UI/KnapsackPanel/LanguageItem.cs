using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Farm.Hotfix
{
    public class LanguageItem : UGuiForm
    {
        [SerializeField]
        private Button btn_Self;

        [SerializeField]
        private GameObject obj_Check;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            btn_Self.onClick.AddListener(OnClickSelf);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        private void OnClickSelf()
        {
            var btnObj = EventSystem.current.currentSelectedGameObject;
            if (btnObj == null)
            {
                return;
            }
            LanguageForm lf = new LanguageForm();
            lf.Refresh(int.Parse(btnObj.name));
        }


    }

}
