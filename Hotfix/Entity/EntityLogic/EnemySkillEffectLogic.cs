using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    //���˼�����Ч�߼�
    //ÿ�����˼���Ч��Ӧ��������������õ��������͵��˶���
    public class EnemySkillEffectLogic : EffectLogic
    {
        private readonly static string s_startPoint = "StartPoint";
        private readonly static string s_playerTag = "Player";
        //�ṩ����
        protected GameObject Enemy = null;
        protected GameObject Player = null;
        protected GameObject EnemyWeapon = null;
        protected GameObject EffectStartPoint = null;

        //����ӿ�
        public GameObject PlayerObject => Player;
        public GameObject EnemyObject => Enemy;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);          
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }
}

