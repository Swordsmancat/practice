using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    //敌人技能特效逻辑
    //每个敌人技能效果应该派生该类来获得敌人武器和敌人对象
    public class EnemySkillEffectLogic : EffectLogic
    {
        private readonly static string s_startPoint = "StartPoint";
        private readonly static string s_playerTag = "Player";
        //提供对象
        protected GameObject Enemy = null;
        protected GameObject Player = null;
        protected GameObject EnemyWeapon = null;
        protected GameObject EffectStartPoint = null;

        //对外接口
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

