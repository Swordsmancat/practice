using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class UITopBar : UGuiForm
    {
        [SerializeField]
        private Button btn_Close;

        [SerializeField]
        private Button btn_Home;
        
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            btn_Home.onClick.AddListener(OnClickClose);
            btn_Close.onClick.AddListener(OnClickClose);
        }

        
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }


        public void OnClickClose()
        {
            Log.Info("OnClickClose");
            GameEntry.UI.CloseUIForm(GetCurrentUIFormId(this.transform.parent.name));
        }

        private int GetCurrentUIFormId(string name)
        {
            switch (name)
            {
                case "SelectRoleForm":
                    return 104;
                case "ArenaForm":
                    return 105;
                case "BlacksmithForm":
                    return 106;
                case "EquipmentForm":
                    return 107;
                case "UndergroundCityForm":
                    return 108;
                case "SettingForm":
                    return 109;
                case "UIRanking":
                    return 111;
                case "UISelectHero":
                    return 112;
            }
            return 0;
        }

    }
}

