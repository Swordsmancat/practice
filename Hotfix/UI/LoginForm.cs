using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Farm.Hotfix
{
    public class LoginForm:UGuiForm
    {
        [SerializeField]
        private GameObject pop_Login;

        //[SerializeField]
        //private TMP_InputField input_Account;

        //[SerializeField]
        //private TMP_InputField input_Password;

        [SerializeField]
        private Toggle toggle_Remember;

        [SerializeField]
        private Button btn_Login;

        [SerializeField]
        private Button btn_Forget;

        [SerializeField]
        private Button btn_Close;

        [SerializeField]
        private Button btn_ToLogin;

        [SerializeField]
        private Sprite input_SpriteOn;

        [SerializeField]
        private Sprite input_SpriteOff;

        private ProcedureLogin m_procedureLogin;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            btn_Login.onClick.AddListener(OnClickLogin);
            btn_Forget.onClick.AddListener(OnClickForget);
            btn_Close.onClick.AddListener(OnClickClose);
         //   btn_ToLogin.onClick.AddListener(OnClickToLogin);
        }

        

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_procedureLogin = (ProcedureLogin)userData;
            if (m_procedureLogin == null)
            {
                Log.Warning("ProcedureChangeScene is invalid when open LoadingPanel");
                return;
            }
        }

        private void OnClickClose()
        {
            pop_Login.SetActive(false);
        }

        private void OnClickForget()
        {
            throw new NotImplementedException();
        }

        private void OnClickLogin()
        {
            m_procedureLogin.StartGame();
        }
        //public void OnAccountSelect()
        //{
        //    input_Account.image.sprite = input_SpriteOn;
        //}
        //private void OnClickToLogin()
        //{
        //    pop_Login.SetActive(true);
        //}
        //public void OnAccountDeSelect()
        //{
        //    input_Account.image.sprite = input_SpriteOff;
        //}

        //public void OnPasswordSelect()
        //{
        //    input_Password.image.sprite = input_SpriteOn;
        //}

        //public void OnPasswordDeSelect()
        //{
        //    input_Password.image.sprite = input_SpriteOff;
        //}


    }
}
