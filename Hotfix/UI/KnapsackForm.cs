using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Farm.Hotfix
{

    //public class KnapsackFormItemInfo : Object
    //{
    //    public int m_Count;
    //    public Sprite m_iconBg;
    //    public Sprite m_icon;
    //    public bool m_check;
    //}

    public class KnapsackForm : UGuiForm
    {
        public static KnapsackForm Instance;
        [SerializeField]
        private Text text_Coin;
        
        [SerializeField]
        private Text text_Diamond;

        [SerializeField]
        private Text text_Name;

        [SerializeField]
        private Text text_ArmsAllCount;

        [SerializeField]
        private Text text_PropAllCount;

        [SerializeField]
        private Text text_FoodAllCount;

        [SerializeField]
        private Image image_Character;

        [SerializeField]
        private Button btn_Home;

        [SerializeField]
        private Transform content;

        [Header("物品描述")]
        [SerializeField]
        private GameObject goodsInfo;
        [SerializeField]
        private Image goodsInfo_ImageIconBg;
        [SerializeField]
        private Image goodsInfo_ImageIcon;
        [SerializeField]
        private Text goodsInfo_TextGoodsName;
        [SerializeField]
        private Text goodsInfo_TextBuffA;
        [SerializeField]
        private Text goodsInfo_TextBuffB;
        [SerializeField]
        private Text goodsInfo_TextDescription;
        [SerializeField]
        private Text goodsInfo_TextCTCDes;
        [SerializeField]
        private Text goodsInfo_TextConfirm;
        [SerializeField]
        private Button goodsInfo_BtnConfirm;
        [SerializeField]
        private Button goodsInfo_BtnClose;

        [Header("物品出售")]
        [SerializeField]
        private GameObject goodsSell;
        [SerializeField]
        private Text goodsSell_TextResult;
        [SerializeField]
        private Button goodsSell_BtnClose;

        private void Awake()
        {
            Instance = this;
        }


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            btn_Home.onClick.AddListener(OnClickClose);
            goodsInfo_BtnClose.onClick.AddListener(OnClickClose);
            goodsSell_BtnClose.onClick.AddListener(OnClickClose);
            goodsInfo_BtnConfirm.onClick.AddListener(OnClickConfirm);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
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
        
        private void OnClickConfirm()
        {
            //TODO:判断类型是装备、道具还是食物，只有食物可售卖(弹出goodsSell)
            goodsInfo.SetActive(false);
            goodsSell.SetActive(true);
        }

        public void GetCurrentGoodsInfo(Sprite bg, Sprite icon)
        {
            goodsInfo_ImageIconBg.sprite = bg;
            goodsInfo_ImageIcon.sprite = icon;
            goodsInfo.SetActive(true);
        }

    }
}

