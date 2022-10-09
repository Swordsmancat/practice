using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Farm.Hotfix
{
    public class InviteLinkForm : UGuiForm
    {
        [SerializeField]
        private InputField input_Link;

        [SerializeField]
        private Text text_CodeText;

        [SerializeField]
        private Image image_CodeImage;

        [SerializeField]
        private Button btn_CopyShare;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            btn_CopyShare.onClick.AddListener(OnClickCopyShare);
        }


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        private void OnClickCopyShare()
        {
            throw new NotImplementedException();
        }
    }
}

