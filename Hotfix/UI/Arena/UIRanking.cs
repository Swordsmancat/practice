using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using LitJson;
using System;

namespace Farm.Hotfix
{
    public class UIRanking : UGuiForm
    {
        [Title("�����Ϣ")]
        [SerializeField]
        private GameObject Userinfo;

        [Title("�б�����")]
        [SerializeField]
        private GameObject parentObj;

        [Title("�����б�")]
        [SerializeField]
        private List<GameObject> rankList;

        [BoxGroup("TabMenu"), Title("��ť")]
        [SerializeField]
        private Button onevs, threevs;

        
        private SortedDictionary<int, string> keyValuePairs = 
            new SortedDictionary<int, string>();



        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitList();
            InitDictionary();
            threevs.onClick.AddListener(OnClickThreeVsButton);
            onevs.onClick.AddListener(OnClickOneVsButton);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        private void OnClickOneVsButton()
        {
            ModifyInfo(Userinfo, "hello", "30", null, null);
            ModifyButtonActive(true, false);
        }

        private void OnClickThreeVsButton()
        {
            ModifyInfo(Userinfo, "world", "5555", null, null);
            ModifyButtonActive(false, true);
        }

        void InitList()
        {
            foreach (Transform child in parentObj.transform)
            {
                Log.Info("��������������:" + child.name);
                rankList.Add(child.gameObject);
            }
            foreach (GameObject apart in rankList)
            {
                Log.Info("��������:" + apart.name);
            }
        }

        void InitDictionary()
        {
            //�û���
            keyValuePairs.Add(001, "Text_Name");
            //����
            keyValuePairs.Add(002, "Text_Score");
            //����
            keyValuePairs.Add(003, "Text_Rank");
            //����
            keyValuePairs.Add(004, "Flag");
        }

        /// <summary>
        /// �޸��б��ж������Ϣ
        /// </summary>
        /// <param name="ojb">��Ϸ����</param>
        /// <param name="name">����</param>
        /// <param name="score">����</param>
        /// <param name="rank">����</param>
        /// <param name="flag">����</param>
        void ModifyInfo(in GameObject obj,in string name,
            in string score,in string rank,in Image flag)
        {
            ModifyName(GetObject(obj, keyValuePairs[001]), name);
            ModifyScore(GetObject(obj, keyValuePairs[002]), score);
            ModifyRank(GetObject(obj, keyValuePairs[003]), rank);
            ModifyFlag(GetObject(obj, keyValuePairs[004]), flag);
        }

        void ModifyName(GameObject obj,in string name)
        {
            ModifyText(obj, name);
        }

        void ModifyFlag(GameObject obj,in Image flag)
        {

        }

        void ModifyScore(GameObject obj,in string score)
        {
            ModifyText(obj, score);
        }

        void ModifyRank(GameObject obj,in string rank)
        {

        }

        void ModifyText(GameObject obj,in string text)
        {
            obj.GetComponent<Text>().text = text;
        }

        void ModifyImage(GameObject obj,in Image image)
        {
            obj.GetComponent<Image>().sprite = image.sprite;
        }

        /// <summary>
        /// �õ���Ϸ������Ӷ���
        /// </summary>
        /// <param name="obj">��������</param>
        /// <param name="objName">�Ӷ�����</param>
        /// <returns>�����Ӷ���</returns>
        GameObject GetObject(GameObject obj,string objName)
        {
            return obj.transform.Find(objName).gameObject;
        }

        //�޸İ�ť�Ӷ��󼤻�״̬
        void ModifyButtonActive(bool oneButton,bool threeButton)
        {
            GetObject(onevs.gameObject, "TabFocusLine").SetActive(oneButton);
            GetObject(threevs.gameObject, "TabFocusLine").SetActive(threeButton);
        }
    }
}
