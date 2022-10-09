using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class MoneyBagForm : UGuiForm
    {
        [Title("提现按钮")]
        [SerializeField]
        private Button getBackMoney;

        [Title("提现窗口关闭按钮")]
        [SerializeField]
        private Button closeButton;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            getBackMoney.onClick.AddListener(OnClickBackMoney);
            closeButton.onClick.AddListener(OnClickClose);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        void OnClickBackMoney()
        {
            gameObject.transform.Find("GetMoneyForm").gameObject.SetActive(true);
        }
        private void OnClickClose()
        {
            gameObject.transform.Find("GetMoneyForm").gameObject.SetActive(false);
        }
    }
}

