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
        [Title("��������")]
        [SerializeField]
        private string accessUrl;
        
        [Title("��ť��")]
        [SerializeField]
        private Button[] btn_array;

        //�������󷽱�����UI״̬
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
                //��������
                case "Setting_Language_Button":
                    ShowUI("SelectLanguageForm");
                    break;

                //�����ά��
                case "InviteButton":
                    ShowUI("InviteCodeForm");
                    break;

                //����
                case "AccessButton":
                    Application.OpenURL(accessUrl);
                    break;

                //�ͷ�����
                case "Customer_Service_Button":
                    ShowUI("ContactCustomerServiceForm");
                    break;

                //��������
                case "AboutUs_Button":
                    break;

                //��������
                case "ChangeName_Button":
                    ShowUI("ChangeNameForm");
                    break;

                //��������
                case "ChangePassword_Button":
                    break;

                //ͬ���Ʒ���
                case "SyncCloudService_Button":
                    break;

                //�˳���Ϸ
                case "ExitGame_Button":
                    break;

                //�˳�����¼����
                case "ExitGoLoginInterface_Button":
                    break;

                //�������Թرհ�ť
                case "Close_Button_Language":
                    HideUI("SelectLanguageForm");
                    break;

                //��ϵ�ͷ��رհ�ť
                case "Close_Button_Service":
                    HideUI("ContactCustomerServiceForm");
                    break;
                
                //��ά��رհ�ť
                case "Close_Button_Code":
                    HideUI("InviteCodeForm");
                    break;
                
                //ȷ�ϰ�ť
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
