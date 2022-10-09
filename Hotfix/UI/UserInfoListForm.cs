using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Farm.Hotfix
{
    public class UserInfoListForm : UGuiForm
    {
        public static UserInfoListForm Instance;
        [SerializeField]
        private Sprite sprite_Selected;

        [SerializeField]
        private Sprite sprite_UnSelected;

        [SerializeField]
        private Button btn_Close;

        [SerializeField]
        private Button btn_UserList;

        [SerializeField]
        private Button btn_MyAttention;

        [SerializeField]
        private GameObject obj_GridUserList;


        [SerializeField]
        private InputField input_Search;

        [SerializeField]
        private Button btn_PageLeft;

        [SerializeField]
        private Button btn_PageRight;

        [SerializeField]
        private Text text_PageNum;

        [Header("用户详细信息页面")]
        [SerializeField]
        private Image image_CountryIcon;

        [SerializeField]
        private Image image_HeadIcon;

        [SerializeField]
        private GameObject Starts;

        [SerializeField]
        private GameObject UserInfoDetailePanel;

        [SerializeField]
        private Text text_GameId;

        [SerializeField]
        private Text text_CoinCount;

        [SerializeField]
        private Text text_Attention;

        [SerializeField]
        private GameObject image_Check;

        [SerializeField]
        private Button btn_OtherFarm;

        [SerializeField]
        private Button btn_Attention;


        private void Awake()
        {
            Instance = this;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            btn_Close.onClick.AddListener(OnClickClose);
            btn_UserList.onClick.AddListener(OnClickUserList);
            btn_MyAttention.onClick.AddListener(OnClickMyAttention);
            btn_PageLeft.onClick.AddListener(OnClickPageLeft);
            btn_PageRight.onClick.AddListener(OnClickPageRight);
            btn_Attention.onClick.AddListener(OnClickAttention);
            btn_OtherFarm.onClick.AddListener(OnClickOtherFarm);

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        private void OnClickPageRight()
        {
            throw new NotImplementedException();
        }

        private void OnClickPageLeft()
        {
            throw new NotImplementedException();
        }

        private void OnClickMyAttention()
        {
            btn_UserList.image.sprite = sprite_UnSelected;
            btn_MyAttention.image.sprite = sprite_Selected;
        }

        private void OnClickUserList()
        {
            btn_UserList.image.sprite = sprite_Selected;
            btn_MyAttention.image.sprite = sprite_UnSelected;
        }

        private void OnClickClose()
        {
            gameObject.SetActive(false);
        }

        private void OnClickAttention()
        {
            if (image_Check.activeSelf)
            {
                text_Attention.text = "UserInfoDeti.UnAttention";
                image_Check.SetActive(false);
            }
            else
            {
                text_Attention.text = "UserInfoDeti.IsAttention";
                image_Check.SetActive(true);
            }
        }

        private void OnClickOtherFarm()
        {
            Debug.LogError("Go to other farm.");
        }

        public void SetDetailePanelPara(GameObject obj, Sprite countryIcon, Sprite headIcon)
        {
            image_CountryIcon.sprite = countryIcon;
            image_HeadIcon.sprite = headIcon;
            obj.SetActive(true);
        }
    }
}

