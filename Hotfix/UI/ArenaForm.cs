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
    public class ArenaForm : UGuiForm
    {
        [Title("HeroButton"),BoxGroup("Heroes")]
        [SerializeField]
        private Button[] btn_array;

        [BoxGroup("Right_Panel")]
        [SerializeField]
        private Button btn_rank,btn_TV,btn_team;

        [Title("PlayerButton"), BoxGroup("Button")]
        [SerializeField]
        private Button btn_play;

        [Title("UpLevelButton"), BoxGroup("Heroes")]
        [SerializeField]
        private Button[] btn_upLevel;



        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitListener();
            UnlockHero(4);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        private void OnClickRankButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.UIRanking,this);
        }
        
        private void OnClickSelectHero()
        {
            var obj = EventSystem.current.currentSelectedGameObject;
            GameEntry.UI.OpenUIForm(UIFormId.UISelectHero, this);
        }

        private void OnClickPlayButton()
        {
            throw new NotImplementedException();
        }

        private void OnClickUpLevel()
        {
            throw new NotImplementedException();
        }

        private void OnClickTeamButton()
        {
            throw new NotImplementedException();
        }

        private void OnClickTVButton()
        {
            throw new NotImplementedException();
        }

        private void UnlockHero(in int number)
        {
            for(int i = 0;i < number;i++)
            {
                btn_array[i].enabled = true;
            }
        }

        private void InitListener()
        {
            btn_rank.onClick.AddListener(OnClickRankButton);
            btn_play.onClick.AddListener(OnClickPlayButton);
            btn_TV.onClick.AddListener(OnClickTVButton);
            btn_team.onClick.AddListener(OnClickTeamButton);
            for (int i = 0; i < btn_array.Length; i++)
            {
                btn_array[i].onClick.AddListener(OnClickSelectHero);
            }
            for (int i = 0; i < btn_upLevel.Length; i++)
            {
                btn_upLevel[i].onClick.AddListener(OnClickUpLevel);
            }
        }


    }
}
