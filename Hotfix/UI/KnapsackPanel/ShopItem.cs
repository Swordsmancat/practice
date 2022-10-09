using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Farm.Hotfix
{
    public class ShopItem : UGuiForm
    {
        [SerializeField]
        private Image bg;

        [SerializeField]
        private Image icon;

        private void Start()
        {
            transform.GetComponent<Button>().onClick.AddListener(OnClickShopItem);
        }

        public Sprite GetSelfSprite()
        {
            var iconBgSprite = transform.GetComponent<Image>().sprite;
            return iconBgSprite;
        }

        private void OnClickShopItem()
        {
            Sprite m_bg = GetSelfSprite();
            Sprite m_icon = icon.sprite;
            ShopForm.Instance.GetCurrentGoodsInfo(m_bg, m_icon);
        }
        //protected override void OnInit(object userData)
        //{
        //    base.OnInit(userData);
        //}

        //protected override void OnOpen(object userData)
        //{
        //    base.OnOpen(userData);
        //}
    }
}

