//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2022-09-23 16:19:53.208
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    /// <summary>
    /// 普通敌人表。
    /// </summary>
    public class DREnemy : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取敌人编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取最大生命。
        /// </summary>
        public int MaxHP
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取最大精力值。
        /// </summary>
        public int MaxMP
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取武器编号0。
        /// </summary>
        public int WeaponId0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取武器编号1。
        /// </summary>
        public int WeaponId1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取视野距离。
        /// </summary>
        public float SeekRange
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取视野范围。
        /// </summary>
        public float SeekAngle
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取攻击范围。
        /// </summary>
        public float AttackRange
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取攻速。
        /// </summary>
        public float AttackSpeed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取走路移动速度。
        /// </summary>
        public float MoveSpeedWalk
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取跑动移动速度。
        /// </summary>
        public float MoveSpeedRun
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取死亡特效编号。
        /// </summary>
        public int DeadEffectId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取死亡声音编号。
        /// </summary>
        public int DeadSoundId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取被攻击声音编号。
        /// </summary>
        public int ByAttackSoundId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取血液效果编号。
        /// </summary>
        public int BloodEffctId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取霸体被攻击声音编号。
        /// </summary>
        public int StoicHurtSoundId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取霸体血液效果编号。
        /// </summary>
        public int StoicHurtEffectId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取行走声音编号。
        /// </summary>
        public int WalkSoundId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取动作声音编号。
        /// </summary>
        public int MotionSoundId0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取动作声音编号。
        /// </summary>
        public int MotionSoundId1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取动作声音编号。
        /// </summary>
        public int MotionSoundId2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取动作声音编号。
        /// </summary>
        public int MotionSoundId3
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取吼叫声音编号。
        /// </summary>
        public int ShoutSoundId
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            MaxHP = int.Parse(columnStrings[index++]);
            MaxMP = int.Parse(columnStrings[index++]);
            WeaponId0 = int.Parse(columnStrings[index++]);
            WeaponId1 = int.Parse(columnStrings[index++]);
            SeekRange = float.Parse(columnStrings[index++]);
            SeekAngle = float.Parse(columnStrings[index++]);
            AttackRange = float.Parse(columnStrings[index++]);
            AttackSpeed = float.Parse(columnStrings[index++]);
            MoveSpeedWalk = float.Parse(columnStrings[index++]);
            MoveSpeedRun = float.Parse(columnStrings[index++]);
            DeadEffectId = int.Parse(columnStrings[index++]);
            DeadSoundId = int.Parse(columnStrings[index++]);
            ByAttackSoundId = int.Parse(columnStrings[index++]);
            BloodEffctId = int.Parse(columnStrings[index++]);
            StoicHurtSoundId = int.Parse(columnStrings[index++]);
            StoicHurtEffectId = int.Parse(columnStrings[index++]);
            WalkSoundId = int.Parse(columnStrings[index++]);
            MotionSoundId0 = int.Parse(columnStrings[index++]);
            MotionSoundId1 = int.Parse(columnStrings[index++]);
            MotionSoundId2 = int.Parse(columnStrings[index++]);
            MotionSoundId3 = int.Parse(columnStrings[index++]);
            ShoutSoundId = int.Parse(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    MaxHP = binaryReader.Read7BitEncodedInt32();
                    MaxMP = binaryReader.Read7BitEncodedInt32();
                    WeaponId0 = binaryReader.Read7BitEncodedInt32();
                    WeaponId1 = binaryReader.Read7BitEncodedInt32();
                    SeekRange = binaryReader.ReadSingle();
                    SeekAngle = binaryReader.ReadSingle();
                    AttackRange = binaryReader.ReadSingle();
                    AttackSpeed = binaryReader.ReadSingle();
                    MoveSpeedWalk = binaryReader.ReadSingle();
                    MoveSpeedRun = binaryReader.ReadSingle();
                    DeadEffectId = binaryReader.Read7BitEncodedInt32();
                    DeadSoundId = binaryReader.Read7BitEncodedInt32();
                    ByAttackSoundId = binaryReader.Read7BitEncodedInt32();
                    BloodEffctId = binaryReader.Read7BitEncodedInt32();
                    StoicHurtSoundId = binaryReader.Read7BitEncodedInt32();
                    StoicHurtEffectId = binaryReader.Read7BitEncodedInt32();
                    WalkSoundId = binaryReader.Read7BitEncodedInt32();
                    MotionSoundId0 = binaryReader.Read7BitEncodedInt32();
                    MotionSoundId1 = binaryReader.Read7BitEncodedInt32();
                    MotionSoundId2 = binaryReader.Read7BitEncodedInt32();
                    MotionSoundId3 = binaryReader.Read7BitEncodedInt32();
                    ShoutSoundId = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private KeyValuePair<int, int>[] m_WeaponId = null;

        public int WeaponIdCount
        {
            get
            {
                return m_WeaponId.Length;
            }
        }

        public int GetWeaponId(int id)
        {
            foreach (KeyValuePair<int, int> i in m_WeaponId)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetWeaponId with invalid id '{0}'.", id));
        }

        public int GetWeaponIdAt(int index)
        {
            if (index < 0 || index >= m_WeaponId.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetWeaponIdAt with invalid index '{0}'.", index));
            }

            return m_WeaponId[index].Value;
        }

        private KeyValuePair<int, int>[] m_MotionSoundId = null;

        public int MotionSoundIdCount
        {
            get
            {
                return m_MotionSoundId.Length;
            }
        }

        public int GetMotionSoundId(int id)
        {
            foreach (KeyValuePair<int, int> i in m_MotionSoundId)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetMotionSoundId with invalid id '{0}'.", id));
        }

        public int GetMotionSoundIdAt(int index)
        {
            if (index < 0 || index >= m_MotionSoundId.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetMotionSoundIdAt with invalid index '{0}'.", index));
            }

            return m_MotionSoundId[index].Value;
        }

        private void GeneratePropertyArray()
        {
            m_WeaponId = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(0, WeaponId0),
                new KeyValuePair<int, int>(1, WeaponId1),
            };

            m_MotionSoundId = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(0, MotionSoundId0),
                new KeyValuePair<int, int>(1, MotionSoundId1),
                new KeyValuePair<int, int>(2, MotionSoundId2),
                new KeyValuePair<int, int>(3, MotionSoundId3),
            };
        }
    }
}
