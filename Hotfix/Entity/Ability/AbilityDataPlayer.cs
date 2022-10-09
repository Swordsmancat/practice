//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/6/28/周二 17:59:45
//------------------------------------------------------------
using GameFramework;
using UnityEngine;
using System;

namespace Farm.Hotfix
{
    [Serializable]
    public class AbilityDataPlayer :AbilityData
    {
        [SerializeField]
        private int m_Level = 1;

        [SerializeField]
        private int m_HPBase = 100;

        [SerializeField]
        private int m_MPBase = 100;

        [SerializeField]
        private int m_ArmorBase = 10;

        [SerializeField]
        private int m_AttackBase = 10;

        [SerializeField]
        private int m_Physical =10;

        [SerializeField]
        private int m_Power =10;

        [SerializeField]
        private int m_Agility =10;

        [SerializeField]
        private int m_Devout = 10;

        public override void OnInit()
        {
            base.OnInit();
            Load();
            CalculationData();
        }

        private void CalculationData()
        {
            //TODO: 计算人物属性值;

        }

        public override void Load()
        {
            base.Load();
            if (!GameEntry.Setting.HasSetting(Constant.Setting.PlayerLevel))
            {
                return;
            }
            m_Level = GameEntry.Setting.GetInt(Constant.Setting.PlayerLevel);
            m_Physical = GameEntry.Setting.GetInt(Constant.Setting.PlayerPhysical);
            m_Power = GameEntry.Setting.GetInt(Constant.Setting.PlayerPower);
            m_Agility = GameEntry.Setting.GetInt(Constant.Setting.PlayerAgility);
            m_Devout = GameEntry.Setting.GetInt(Constant.Setting.PlayerDevout);
        }

        public override void Save()
        {
            base.Save();
            GameEntry.Setting.SetInt(Constant.Setting.PlayerLevel, m_Level);
            GameEntry.Setting.SetInt(Constant.Setting.PlayerPhysical, m_Physical);
            GameEntry.Setting.SetInt(Constant.Setting.PlayerPower, m_Power);
            GameEntry.Setting.SetInt(Constant.Setting.PlayerAgility, m_Agility);
            GameEntry.Setting.SetInt(Constant.Setting.PlayerDevout, m_Devout);
            GameEntry.Setting.Save();
        }

        /// <summary>
        /// 等级:每升一级可选择一项属性进行加点。
        /// </summary>
        public int Level
        {
            get
            {
                return m_Level;
            }
        }

        /// <summary>
        /// 基础血量
        /// </summary>
        public int HPBase
        {
            get
            {
                return m_HPBase;
            }
        }

        /// <summary>
        /// 基础精力
        /// </summary>
        public int MPBase
        {
            get
            {
                return m_MPBase;
            }
        }

        public int ArmorBase
        {
            get
            {
                return m_ArmorBase;
            }
        }

        public int AttackBase
        {
            get
            {
                return m_AttackBase;
            }
        }

        /// <summary>
        /// 体格:增加HP，降低受伤出硬直的几率。
        /// </summary>
        public int Physical
        {
            get
            {
                return m_Physical;
            }
        }

        /// <summary>
        /// 力量：增加攻击力，提升致命一级的几率。
        /// </summary>
        public int Power
        {
            get
            {
                return m_Power;
            }
        }

        /// <summary>
        /// 敏捷：增加精力值，减少翻滚和滑步的消耗。
        /// </summary>
        public int Agility
        {
            get
            {
                return m_Agility;
            }
        }

        /// <summary>
        /// 虔诚：需要一定点数来使用部分武器。
        /// </summary>
        public int Devout
        {
            get
            {
                return m_Devout;
            }
        }

    }
}
