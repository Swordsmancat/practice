using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Farm.Hotfix
{
    public class UserItemInfo : UGuiForm
    {
        [SerializeField]
        private Sprite OnLine;
        [SerializeField]
        private Sprite OffLine;

        [SerializeField]
        private Image image_HeadIcon;

        [SerializeField]
        private Text text_Name;

        [SerializeField]
        private Text text_Count;

        [SerializeField]
        private Image image_UserState;

        [SerializeField]
        private GameObject obj_Mask;

        [SerializeField]
        private GameObject obj_IsAttention;

        [SerializeField]
        private Transform transf_Starts;

        [SerializeField]
        private Image image_CountryIcon;

        [SerializeField]
        private Text text_TimeCountDown;

        [SerializeField]
        private Button btn_Item;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            btn_Item.onClick.AddListener(OnClickItem);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        private void OnClickItem()
        {
            UserInfoListForm.Instance.SetDetailePanelPara(gameObject, 
                image_CountryIcon.sprite, image_HeadIcon.sprite);
        }

    }

}
