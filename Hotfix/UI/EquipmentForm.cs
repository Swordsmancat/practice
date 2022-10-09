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
        #region ����װ����(��:Ь�ӡ����ס����� ��:���������ơ�����)
        [TitleGroup("Equipment Slot(����װ����)")]
        [HorizontalGroup("Equipment Slot(����װ����)/Split")]
        [VerticalGroup("Equipment Slot(����װ����)/Split/Left")]
        [BoxGroup("Equipment Slot(����װ����)/Split/Left/Ь��")][SerializeField]
        private Image[] img_Shoes;
        [BoxGroup("Equipment Slot(����װ����)/Split/Left/����")][SerializeField]
        private Image[] img_Hands;
        [BoxGroup("Equipment Slot(����װ����)/Split/Left/����")][SerializeField]
        private Image[] img_Body;

        [VerticalGroup("Equipment Slot(����װ����)/Split/Right")]
        [BoxGroup("Equipment Slot(����װ����)/Split/Right/����")][SerializeField]
        private Image[] img_Arms;
        [BoxGroup("Equipment Slot(����װ����)/Split/Right/����")][SerializeField]
        private Image[] img_Shield;
        [BoxGroup("Equipment Slot(����װ����)/Split/Right/����")][SerializeField]
        private Image[] img_WristGuard;
        [PropertySpace]
        #endregion

        #region ��ɫ����(��ά��Ԫ��ֵ)
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
            /// title����Ϊ����ʹ�ã���ʵ������
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

        [Header("������ť")]
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

