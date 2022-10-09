using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Farm
{
    public class SelectLanguageForm : MonoBehaviour
    {
        [Title("按钮")]
        [SerializeField]
        private Button[] btn_array;


        //前一个选择的按钮
        private Button selectButton;

        private void Start()
        {
            for(int i = 0;i < btn_array.Length; i++)
            {
                btn_array[i].onClick.AddListener(OnClickSelectFlag);
            }
            selectButton = FindSelectButton();
        }

        private void OnClickSelectFlag()
        {
            var obj = EventSystem.current.currentSelectedGameObject;
            if(obj.GetComponent<Button>() != selectButton)
            {
                obj.gameObject.transform.Find("Check").gameObject.SetActive(true);
                selectButton.gameObject.transform.Find("Check").gameObject.SetActive(false);
                selectButton = obj.GetComponent<Button>();
            }
        }

        private Button FindSelectButton()
        {
            for(int i = 0;i < btn_array.Length; i++)
            {
                if(btn_array[i].gameObject.transform.Find("Check").gameObject.activeSelf == true)
                {
                    return btn_array[i];
                }
            }
            return null;
        }
    }
}

