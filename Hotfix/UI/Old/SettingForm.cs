using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Farm.Hotfix
{
    public class SettingFormOld : UGuiForm
    {
        [SerializeField]
        private GameObject volume_On;

        [SerializeField]
        private GameObject volume_Off;

        [SerializeField]
        private Button btn_Volume;

        [SerializeField]
        private Button btn_ForgetPsw;

        [SerializeField]
        private Button btn_InviteLink;

        [SerializeField]
        private Button btn_SwitchLanguage;

        [SerializeField]
        private Button btn_ExitGame;

        [SerializeField]
        private Button btn_Home;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            btn_Home.onClick.AddListener(OnClickClose);
            btn_Volume.onClick.AddListener(OnClickVolume);
            btn_ForgetPsw.onClick.AddListener(OnClickForgetPsw);
            btn_InviteLink.onClick.AddListener(OnClickInviteLink);
            btn_SwitchLanguage.onClick.AddListener(OnClickSwitchLanguage);
            btn_ExitGame.onClick.AddListener(OnClickExitGame);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        private void OnClickExitGame()
        {
            throw new NotImplementedException();
        }

        private void OnClickSwitchLanguage()
        {
            throw new NotImplementedException();
        }

        private void OnClickInviteLink()
        {
            throw new NotImplementedException();
        }

        private void OnClickForgetPsw()
        {
            throw new NotImplementedException();
        }

        private void OnClickVolume()
        {
            if (volume_On.activeSelf)
            {
                volume_On.SetActive(false);
                volume_Off.SetActive(true);
            }
            else
            {
                volume_On.SetActive(true);
                volume_Off.SetActive(false);
            }
        }

        private void OnClickClose()
        {
            var btnObj = EventSystem.current.currentSelectedGameObject;
            if (btnObj == null)
            {
                return;
            }
            Transform btnParent = btnObj.transform.parent;
            btnParent.gameObject.SetActive(false);
        }

    }

}
