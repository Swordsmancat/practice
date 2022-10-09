using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class SelectRoleForm : UGuiForm
    {
        [SerializeField]
        private Button btn_Close;

        [SerializeField]
        private Image image_Character;

        [SerializeField]
        private Image image_CharacterType;

        [SerializeField]
        private Text text_CharacterType;

        [SerializeField]
        private Text text_UserName;

        [SerializeField]
        private Text text_CharacterInfo;

        [SerializeField]
        private Button btn_Select;

        [SerializeField]
        private Button[] btn_ArrCharacters;

        [Header("角色五维")]
        // 耐力、敏捷、力量、防御、魔法
        [SerializeField]
        private Slider[] slider_ArrStats;
        [SerializeField]
        private Text[] text_ArrStats;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            for (int i = 0; i < btn_ArrCharacters.Length; i++)
            {
                btn_ArrCharacters[i].onClick.AddListener(OnClickArrCharacters);
            }
            btn_Select.onClick.AddListener(OnClickSelect);
            btn_Close.onClick.AddListener(OnClickClose);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        private void OnClickArrCharacters()
        {
            var obj = EventSystem.current.currentSelectedGameObject;
            FocusingEffect(obj.transform.parent.gameObject);

            ChangeTextInfo(obj.name);

        }

        private void OnClickSelect()
        {
            Log.Info("Selected role.");
        }
        private void OnClickClose()
        {
            Log.Info("OnClickClose");
        }
        private void FocusingEffect(GameObject obj)
        {
            for (int i = 0; i < btn_ArrCharacters.Length; i++)
            {
                btn_ArrCharacters[i].gameObject.transform.parent.Find("RoleButton_Focus").gameObject.SetActive(false);
            }
            obj.transform.Find("RoleButton_Focus").gameObject.SetActive(true);
        }

        private void ChangeTextInfo(string name)
        {
            text_CharacterType.text = name;
            text_CharacterInfo.text = name + "Info";

            for (int i = 0; i < 5; i++)
            {
                slider_ArrStats[i].value = Utility.Random.GetRandom(0, 100);
                text_ArrStats[i].text = slider_ArrStats[i].value.ToString();
            }
            

        }




    }
}

