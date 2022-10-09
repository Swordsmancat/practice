using GameFramework;
using UnityEngine.UI;
using UnityEngine;
using UnityGameFramework.Runtime;
using System;
using GameFramework.DataTable;
using System.Collections.Generic;

namespace Farm.Hotfix
{
    public class DialogOptionsForm : UGuiForm
    {


        [SerializeField]
        private GameObject m_Option =null;

        [SerializeField]
        private Transform m_OptionGroup;

        private Button btn_Diglog;

        private Button btn_Transaction;


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);


            NPCData data = (NPCData)userData;
            if (data == null)
            {
                Log.Warning("data is invalid");
                return;
            }
            for (int i = 0; i < data.Options; i++)
            {
                GameObject obj;
                if (m_OptionGroup.childCount < i+1)
                {
                    obj = Instantiate(m_Option, m_OptionGroup);
                }
                else
                {
                    obj = m_OptionGroup.GetChild(i).gameObject;
                }
                switch (i)
                {
                    case 0:
                        if (data.IsDiglog)
                        {
                            btn_Diglog = obj.GetComponent<Button>();
                            btn_Diglog.onClick.AddListener(OnClickDiglog);
                            obj.GetComponentInChildren<Text>().text = GameEntry.Localization.GetString("DialogOptions.Diglog");
                        }
                        break;
                    case 1:
                        if (data.IsTransaction)
                        {
                            btn_Transaction = obj.GetComponent<Button>();
                            btn_Transaction.onClick.AddListener(OnClickTransaction);
                            obj.GetComponentInChildren<Text>().text = GameEntry.Localization.GetString("DialogOptions.Transaction");
                        }
                        break;


                }
                
            }
        }

        private void OnClickTransaction()
        {
            throw new NotImplementedException();
        }

        private void OnClickDiglog()
        {
            GameEntry.UI.OpenDialog(new DialogParams { });
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            btn_Diglog.onClick.RemoveListener(OnClickDiglog);
            btn_Transaction.onClick.RemoveListener(OnClickTransaction);
            base.OnClose(isShutdown, userData);
        }

      

    }
}
