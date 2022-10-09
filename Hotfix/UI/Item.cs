using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using Sirenix.OdinInspector;

namespace Farm.Hotfix
{
    public class Item : UGuiForm
    {
        public static Item Instance;
        [SerializeField]
        private Text text_Count;

        [SerializeField]
        private GameObject obj_Nofify;

        [SerializeField]
        private Image img_Icon;

        [SerializeField]
        private Image img_Frame;

        [SerializeField]
        private Image img_Bg;

        [SerializeField]
        private GameObject obj_Focus;

        [SerializeField]
        private Button btn_Item;

        [SerializeField]
        private List<AllFrameList> allFrameList = new List<AllFrameList>();

        [SerializeField]
        private List<AllBgColor> allBgColor = new List<AllBgColor>();

        [SerializeField]
        private bool isEquip = false;
        public bool IsEquip { get => isEquip;}

        UIEquipmentItemDetailInfo m_EquipInfo = UIEquipmentItemDetailInfo.Instance;
        EquipmentForm m_EquipmentForm = EquipmentForm.Instance;

        private GameObject m_EquipmentForm1;
        protected override void OnInit(object userData)
        {
            Instance = this;
            base.OnInit(userData);

            m_EquipmentForm1 = this.gameObject.transform.parent.transform.parent.transform.parent.transform.parent.gameObject;
            
            btn_Item.onClick.AddListener(OnClickItem);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        public void OnClickItem()
        {
            Log.Info(m_EquipmentForm1.name);
            m_EquipmentForm1.GetComponent<EquipmentForm>().EquipmentDetailIsShow(true);
            m_EquipInfo.SetItem(img_Bg.color, img_Frame.sprite, img_Icon.sprite);
            m_EquipInfo.SetItemName(img_Bg.color, "∏÷÷∆∂‹≈∆");
            
        }


        public void ControlFoucsing(bool isFocus)
        {
            isEquip = isFocus;
            obj_Focus.SetActive(isFocus);
        }

        public void SetTextCount(int count)
        {
            if (count < 2)
            {
                text_Count.gameObject.SetActive(false);
            }
            text_Count.gameObject.SetActive(true);
            text_Count.text = count.ToString();
        }

        public void SetIcon(Sprite sp)
        {
            img_Icon.sprite = sp;
        }

        public void SetFrame(Sprite sp)
        {
            img_Frame.sprite = sp;
        }

        public void SetNofify(bool isNofify)
        {
            obj_Nofify.SetActive(isNofify);
        }

        public void SetBGColor(float r, float g, float b)
        {
            img_Bg.color = new Color(r, g, b);
        }
    }

    [Serializable]
    public class AllFrameList
    {
        [SerializeField,TableColumnWidth(130, Resizable = false)][GUIColor(247 / 255f, 220 /255f, 111 / 255f,1f)]
        private Sprite sp_Yellow;
        [SerializeField]
        [GUIColor(1f, 165 / 255f, 0, 1f)]
        private Sprite sp_Goal;
        [SerializeField]
        [GUIColor(1f, 0, 0, 1f)]
        private Sprite sp_Red;
        [SerializeField]
        [GUIColor(123 / 255f, 104 / 255f, 238 / 255f, 1f)]
        private Sprite sp_Purple;
        [SerializeField]
        [GUIColor(0, 0, 1f, 1f)]
        private Sprite sp_Blue;
        [SerializeField]
        [GUIColor(0, 128 / 255f, 0, 1f)]
        private Sprite sp_Green;

    };

    [Serializable]
    public class AllBgColor
    {
        [SerializeField, TableColumnWidth(130, Resizable = false)]
        [GUIColor(247 / 255f, 220 / 255f, 111 / 255f, 1f)]
        private Color v3_Yellow;
        [SerializeField]
        [GUIColor(1f, 165 / 255f, 0, 1f)]
        private Color v3_Goal;
        [SerializeField]
        [GUIColor(1f, 0, 0, 1f)]
        private Color v3_Red;
        [SerializeField]
        [GUIColor(123 / 255f, 104 / 255f, 238 / 255f, 1f)]
        private Color v3_Purple;
        [SerializeField]
        [GUIColor(0, 0, 1f, 1f)]
        private Color v3_Blue;
        [SerializeField]
        [GUIColor(0, 128 / 255f, 0, 1f)]
        private Color v3_Green;
    };

}
