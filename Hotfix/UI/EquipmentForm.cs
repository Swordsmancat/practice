using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using UnityEngine.EventSystems;


namespace Farm.Hotfix
{

    public class EquipmentForm : UGuiForm
    {
        public static EquipmentForm Instance;
        #region 六个装备槽(左:鞋子、手套、铠甲 右:武器、盾牌、护腕)
        [TitleGroup("Equipment Slot(六个装备槽)")]
        [HorizontalGroup("Equipment Slot(六个装备槽)/Split")]
        [VerticalGroup("Equipment Slot(六个装备槽)/Split/Left")]
        [BoxGroup("Equipment Slot(六个装备槽)/Split/Left/鞋子")][SerializeField]
        private Image[] img_Shoes;
        [BoxGroup("Equipment Slot(六个装备槽)/Split/Left/手套")][SerializeField]
        private Image[] img_Hands;
        [BoxGroup("Equipment Slot(六个装备槽)/Split/Left/铠甲")][SerializeField]
        private Image[] img_Body;

        [VerticalGroup("Equipment Slot(六个装备槽)/Split/Right")]
        [BoxGroup("Equipment Slot(六个装备槽)/Split/Right/武器")][SerializeField]
        private Image[] img_Arms;
        [BoxGroup("Equipment Slot(六个装备槽)/Split/Right/盾牌")][SerializeField]
        private Image[] img_Shield;
        [BoxGroup("Equipment Slot(六个装备槽)/Split/Right/护腕")][SerializeField]
        private Image[] img_WristGuard;
        [PropertySpace]
        #endregion

        #region 角色属性(五维和元素值)
        [TableList, SerializeField]
        private List<RolePropertiesItem> Properties = new List<RolePropertiesItem>()
        {
            new RolePropertiesItem(),
            new RolePropertiesItem(),
            new RolePropertiesItem(),
            new RolePropertiesItem(),
            new RolePropertiesItem(),
        };

        [Serializable]
        public class RolePropertiesItem
        {
            /// <summary>
            /// title仅作为标题使用，无实际意义
            /// </summary>
            [TableColumnWidth(130, Resizable = false)]
            public string title;

            [TableColumnWidth(200,Resizable = false), SerializeField]
            private Slider slider_Dimension;

            [TableColumnWidth(50),SerializeField]
            private Text text_Dimension;

            [SerializeField]
            private Text text_Element;

        }
        [PropertySpace]
        #endregion

        [Header("导航按钮")]
        [SerializeField]
        private Button[] btn_Menu;

        [SerializeField]
        private GameObject Popup_ItemDetail;

        private void Awake()
        {
            Instance = this;
        }
        protected override void OnInit(object userData)
        {
            
            base.OnInit(userData);

            for (int i = 0; i < btn_Menu.Length; i++)
            {
                btn_Menu[i].onClick.AddListener(OnClickMenuBtn);
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        private void OnClickMenuBtn()
        {
            var btnObj = EventSystem.current.currentSelectedGameObject.transform;
            if (btnObj == null)
            {
                return;
            }
            MenuFocusing(btnObj);
        }

        private void MenuFocusing(Transform trans)
        {
            for (int i = 0; i < btn_Menu.Length; i++)
            {
                btn_Menu[i].transform.Find("Focus").gameObject.SetActive(false);
                btn_Menu[i].transform.Find("IconFocus").gameObject.SetActive(false);
            }
            trans.Find("Focus").gameObject.SetActive(true);
            trans.Find("IconFocus").gameObject.SetActive(true);
        }

        public void EquipmentDetailIsShow(bool isShow)
        {
            Popup_ItemDetail.SetActive(isShow);
        }
    }
}

