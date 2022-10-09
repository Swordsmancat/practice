using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class UISelectHero : UGuiForm
    {
        [Title("SelectHero")]
        [SerializeField]
        private Button[] btn_array;

        [Title("Foucs")]
        [SerializeField]
        private GameObject foucs;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitAddListener();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        void InitAddListener()
        {
            for(int i = 0;i < btn_array.Length;i++)
            {
                btn_array[i].onClick.AddListener(OnClickSelectHero);
            }
        }

        void FcousSelect(GameObject obj)
        {
            foucs.transform.SetParent(obj.transform);
            foucs.transform.SetLocalPositionY(0);
            foucs.transform.SetLocalPositionX(0);
            foucs.SetActive(true);
        }

        private void OnClickSelectHero()
        {
            var obj = EventSystem.current.currentSelectedGameObject;
            FcousSelect(obj);
        }

    }

}
