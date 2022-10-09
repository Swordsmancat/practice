using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class UIEquipmentItemDetailInfo : UGuiForm
    {
        public static UIEquipmentItemDetailInfo Instance;
        [SerializeField]
        private Text text_ItemName;

        [SerializeField]
        private Image img_ItemFrame;

        [SerializeField]
        private Image img_ItemBg;

        [SerializeField]
        private Image img_ItemIcon;

        [SerializeField]
        private GameObject obj_AttackDamage;

        [SerializeField]
        private GameObject obj_Health;

        [SerializeField]
        private Button btn_Fuse;

        [SerializeField]
        private Button btn_Equip;

        [SerializeField]
        private Button btn_ScreenDimmed;

        [SerializeField]
        private Item m_item = Item.Instance;


        protected override void OnInit(object userData)
        {
            Instance = this;
            base.OnInit(userData);

            IsEquipCurrentItem(m_item.IsEquip);
            btn_Fuse.onClick.AddListener(OnClickFuse);
            btn_Equip.onClick.AddListener(OnClickEquip);
            btn_ScreenDimmed.onClick.AddListener(OnClickScreenDimmed);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        public void SetItemName(Color color,string name)
        {
            text_ItemName.color = color;
            text_ItemName.text = name;
        }
        public void SetAttackDamageValue(int value)
        {
            if (value == 0)
            {
                obj_AttackDamage.SetActive(false);
                return;
            }
            Text attackValue = obj_AttackDamage.transform.Find("Text_Value").GetComponent<Text>();
            attackValue.text = "+" + value.ToString();

        }
        public void SetHealthValue(int value)
        {
            if (value == 0)
            {
                obj_Health.SetActive(false);
                return;
            }
            Text HealthValue = obj_Health.transform.Find("Text_Value").GetComponent<Text>();
            HealthValue.text = "+" + value.ToString();
        }
        public void SetItem(Color color,Sprite frame,Sprite itemIcon)
        {
            Log.Info(color);
            img_ItemBg.color = color;
            img_ItemFrame.sprite = frame;
            img_ItemIcon.sprite = itemIcon;

        }
        public void IsShow(bool isShow)
        {
            this.gameObject.SetActive(isShow);
        }

        public void OnClickFuse()
        {
            m_item.ControlFoucsing(false);
            IsShow(false);
        }
        public void OnClickEquip()
        {
            m_item.ControlFoucsing(true);
            IsShow(false);
        }
        public void OnClickScreenDimmed()
        {
            IsShow(false);
        }

        private void IsEquipCurrentItem(bool isEquip)
        {
            if (isEquip)
            {
                btn_Equip.gameObject.SetActive(false);
                btn_Fuse.gameObject.SetActive(true);
                return;
            }
            btn_Equip.gameObject.SetActive(false);
            btn_Fuse.gameObject.SetActive(true);
        }
        
    }

}
