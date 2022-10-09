using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Farm.Hotfix
{
    public class KnapsackItem : UGuiForm
{

        [SerializeField]
        private Image image_Icon;

        [SerializeField]
        private Text text_Count;

        [SerializeField]
        private GameObject obj_Check;

        private void Start()
        {
            transform.GetComponent<Button>().onClick.AddListener(OnClickItem);
        }
        //protected override void OnInit(object userData)
        //{
        //    base.OnInit(userData);
        //}

        //protected override void OnOpen(object userData)
        //{
        //    base.OnOpen(userData);
        //}

        public void SetIcon(Sprite sp)
        {

        }

        public void SetText(string name)
        {
            text_Count.text = name;
        }

        public void IsCheck(bool check)
        {
            obj_Check.SetActive(check);
        }

        public Sprite GetSelfSprite()
        {
            var iconBgSprite = transform.GetComponent<Image>().sprite;
            return iconBgSprite;
        }

        private void OnClickItem()
        {
            Sprite m_bg = GetSelfSprite();
            Sprite m_icon = image_Icon.sprite;
            KnapsackForm.Instance.GetCurrentGoodsInfo(m_bg, m_icon);
        }
    }

}
