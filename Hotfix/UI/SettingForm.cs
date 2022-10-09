using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
namespace Farm.Hotfix
{
    public partial class SettingForm : UGuiForm
    {
        [Title("访问链接")]
        [SerializeField]
        private string accessUrl;
        
        [Title("按钮组")]
        [SerializeField]
        private Button[] btn_array;

        //父级对象方便设置UI状态
        private GameObject parentObj;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            parentObj = gameObject.transform.Find("Settings").gameObject;
            InitListener();
        }
        
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        private void InitListener()
        {
            for (int i = 0;i < btn_array.Length;i++)
            {
                btn_array[i].onClick.AddListener(OnClickArrayButton);
            }
        }

        private void OnClickArrayButton()
        {
            var obj = EventSystem.current.currentSelectedGameObject;
            DoButton(obj);
        }

        
        private void DoButton(GameObject obj)
        {
            
            switch (obj.name)
            {
                //设置语言
                case "Setting_Language_Button":
                    ShowUI("SelectLanguageForm");
                    break;

                //邀请二维码
                case "InviteButton":
                    ShowUI("InviteCodeForm");
                    break;

                //访问
                case "AccessButton":
                    Application.OpenURL(accessUrl);
                    break;

                //客服服务
                case "Customer_Service_Button":
                    ShowUI("ContactCustomerServiceForm");
                    break;

                //关于我们
                case "AboutUs_Button":
                    break;

                //更改名字
                case "ChangeName_Button":
                    ShowUI("ChangeNameForm");
                    break;

                //更改密码
                case "ChangePassword_Button":
                    break;

                //同步云服务
                case "SyncCloudService_Button":
                    break;

                //退出游戏
                case "ExitGame_Button":
                    break;

                //退出到登录界面
                case "ExitGoLoginInterface_Button":
                    break;

                //设置语言关闭按钮
                case "Close_Button_Language":
                    HideUI("SelectLanguageForm");
                    break;

                //联系客服关闭按钮
                case "Close_Button_Service":
                    HideUI("ContactCustomerServiceForm");
                    break;
                
                //二维码关闭按钮
                case "Close_Button_Code":
                    HideUI("InviteCodeForm");
                    break;
                
                //确认按钮
                case "Ok_Button":
                    HideUI("ChangeNameForm");
                    break;

                default:
                    break;
            }
        }

        private void ShowUI(string UIname)
        {
            parentObj.transform.Find(UIname).gameObject.SetActive(true);
        }

        private void HideUI(string UIname)
        {
            parentObj.transform.Find(UIname).gameObject.SetActive(false);
        }
    }
}
