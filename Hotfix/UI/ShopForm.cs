using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Farm.Hotfix
{
    public class ShopForm : UGuiForm
    {
        public static ShopForm Instance;
        [SerializeField]
        private Text text_Coin;
        [SerializeField]
        private Text text_Diamond;
        [SerializeField]
        private Text text_EquipmentBuy;
        [SerializeField]
        private Text text_PropBuy;

        [SerializeField]
        private Image[] image_PageArr;
        [SerializeField]
        private Sprite sprite_CurrentPage;
        [SerializeField]
        private Sprite sprite_NotCurrentPage;
        [SerializeField]
        private Sprite sprite_CoinIcon;
        [SerializeField]
        private Sprite sprite_DiamondIcon;
        [SerializeField]
        private Sprite sprite_CoinConfirmBg;
        [SerializeField]
        private Sprite sprite_DiamondConfirmBg;
        [SerializeField]
        private Transform grid_Equip;
        [SerializeField]
        private Transform grid_Prop;

        [SerializeField]
        private Button btn_Left;
        [SerializeField]
        private Button btn_Right;
        [SerializeField]
        private Button btn_Home;

        [Header("商品购买信息页面")]
        [SerializeField]
        private GameObject GoodsBuyInfoPanel;
        [SerializeField]
        private Image image_IconBg;
        [SerializeField]
        private Image image_Icon;
        [SerializeField]
        private Image image_CurrencyIcon;
        [SerializeField]
        private Image image_ConfirmBg;
        [SerializeField]
        private Text text_Price;
        [SerializeField]
        private Button btn_InfoConfirm;
        [SerializeField]
        private Button btn_InfoClose;

        [Header("商品购买支付页面")]
        [SerializeField]
        private GameObject GoodsBuyConfirmPanel;
        [SerializeField]
        private InputField input_PasswordA;
        [SerializeField]
        private InputField input_PasswordB;
        [SerializeField]
        private Sprite input_SpriteOn;
        [SerializeField]
        private Sprite input_SpriteOff;
        [SerializeField]
        private Button btn_PayConfirm;
        [SerializeField]
        private Button btn_PayCancel;
        [SerializeField]
        private Button btn_PayClose;


        [Header("商品购买结果页面")]
        [SerializeField]
        private GameObject GoodsBuyResultPanel;
        [SerializeField]
        private Image image_ResultGoodsIcon;
        [SerializeField]
        private Text text_ResultGoodsName;
        [SerializeField]
        private Button btn_ResultClose;

        private Sprite goodsIcon;
        private void Awake()
        {
            Instance = this;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            btn_Left.onClick.AddListener(OnClickTurnPage);
            btn_Right.onClick.AddListener(OnClickTurnPage);
            btn_InfoConfirm.onClick.AddListener(OnClickInfoConfirm);
            btn_PayConfirm.onClick.AddListener(OnClickPayConfirm);

            btn_Home.onClick.AddListener(OnClickClose);
            btn_InfoClose.onClick.AddListener(OnClickClose);
            btn_InfoConfirm.onClick.AddListener(OnClickClose);
            btn_PayCancel.onClick.AddListener(OnClickClose);
            btn_PayClose.onClick.AddListener(OnClickClose);
            btn_PayConfirm.onClick.AddListener(OnClickClose);
            btn_ResultClose.onClick.AddListener(OnClickClose);

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        private void OnClickTurnPage()
        {
            var btnObj = EventSystem.current.currentSelectedGameObject;
            if (btnObj == null)
            {
                return;
            }
            if (btnObj.name == "Button_Left")
            {
                RefreshPageNum(false);
            }
            
            if(btnObj.name == "Button_Right")
            {
                RefreshPageNum(true);
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

        private void OnClickInfoConfirm()
        {
            input_PasswordA.text = input_PasswordB.text = null;
            GoodsBuyConfirmPanel.SetActive(true);
        }

        private void OnClickPayConfirm()
        {
            GoodsBuyResultPanel.SetActive(true);
            image_ResultGoodsIcon.sprite = goodsIcon;
        }

        private void RefreshPageNum(bool right)
        {
            if (right && image_PageArr[image_PageArr.Length - 1].sprite == sprite_CurrentPage)
            {
                return;
            }
            if (!right && image_PageArr[0].sprite == sprite_CurrentPage)
            {
                return;
            }

            for (int i = 0; i < image_PageArr.Length; i++)
            {
                if (image_PageArr[i].sprite == sprite_CurrentPage)
                {
                    if (!right)
                    {
                        grid_Equip.localPosition += new Vector3(1215, 0, 0);
                        grid_Prop.localPosition += new Vector3(1215, 0, 0);
                        image_PageArr[i - 1].sprite = sprite_CurrentPage;
                        image_PageArr[i].sprite = sprite_NotCurrentPage;
                        return;
                    }
                    else
                    {
                        grid_Equip.localPosition -= new Vector3(1215, 0, 0);
                        grid_Prop.localPosition -= new Vector3(1215, 0, 0);
                        image_PageArr[i + 1].sprite = sprite_CurrentPage;
                        image_PageArr[i].sprite = sprite_NotCurrentPage;
                        return;
                    }
                }
            }

        }

        public void GetCurrentGoodsInfo(Sprite bg, Sprite icon)
        {
            goodsIcon = icon;

            image_IconBg.sprite = bg;
            image_Icon.sprite = icon;
            GoodsBuyInfoPanel.SetActive(true);
        }

        public void OnPasswordASelected()
        {
            input_PasswordA.image.sprite = input_SpriteOn;
            input_PasswordB.image.sprite = input_SpriteOff;
        }
        public void OnPasswordBSelected()
        {
            input_PasswordA.image.sprite = input_SpriteOff;
            input_PasswordB.image.sprite = input_SpriteOn;
        }

    }
}

