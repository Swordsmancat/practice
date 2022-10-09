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
        [Title("玩家信息")]
        [SerializeField]
        private GameObject Userinfo;

        [Title("列表父对象")]
        [SerializeField]
        private GameObject parentObj;

        [Title("排名列表")]
        [SerializeField]
        private List<GameObject> rankList;

        [BoxGroup("TabMenu"), Title("按钮")]
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
                Log.Info("所有子物体名称:" + child.name);
                rankList.Add(child.gameObject);
            }
            foreach (GameObject apart in rankList)
            {
                Log.Info("物体名字:" + apart.name);
            }
        }

        void InitDictionary()
        {
            //用户名
            keyValuePairs.Add(001, "Text_Name");
            //分数
            keyValuePairs.Add(002, "Text_Score");
            //排名
            keyValuePairs.Add(003, "Text_Rank");
            //旗帜
            keyValuePairs.Add(004, "Flag");
        }

        /// <summary>
        /// 修改列表中对象的信息
        /// </summary>
        /// <param name="ojb">游戏对象</param>
        /// <param name="name">名字</param>
        /// <param name="score">分数</param>
        /// <param name="rank">排行</param>
        /// <param name="flag">旗帜</param>
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
        /// 得到游戏对象的子对象
        /// </summary>
        /// <param name="obj">父级对象</param>
        /// <param name="objName">子对象名</param>
        /// <returns>返回子对象</returns>
        GameObject GetObject(GameObject obj,string objName)
        {
            return obj.transform.Find(objName).gameObject;
        }

        //修改按钮子对象激活状态
        void ModifyButtonActive(bool oneButton,bool threeButton)
        {
            GetObject(onevs.gameObject, "TabFocusLine").SetActive(oneButton);
            GetObject(threevs.gameObject, "TabFocusLine").SetActive(threeButton);
        }
    }
}
