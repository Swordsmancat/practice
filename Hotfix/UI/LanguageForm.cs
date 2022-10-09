using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Farm.Hotfix
{
    public class LanguageForm : UGuiForm
    {
        [SerializeField]
        private Button btn_Close;

        [SerializeField]
        private GameObject[] objArr_AllCountryIcon;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            btn_Close.onClick.AddListener(OnClickClose);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        private void OnClickClose()
        {
            gameObject.SetActive(false);
        }

        public void Refresh(int item)
        {
            for (int i = 0; i < objArr_AllCountryIcon.Length; i++)
            {
                objArr_AllCountryIcon[i].transform.Find("Image").gameObject.SetActive(false);
            }
            objArr_AllCountryIcon[item].transform.Find("Image").gameObject.SetActive(true);
        }
    }
}
