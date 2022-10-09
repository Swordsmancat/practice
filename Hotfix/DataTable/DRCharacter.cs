﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2022-09-23 16:19:52.343
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
    /// 战机表。
    /// </summary>
    public class DRCharacter : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取战机编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取推进器编号。
        /// </summary>
        public int ThrusterId
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
        /// 获取武器编号2。
        /// </summary>
        public int WeaponId2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取装甲编号0。
        /// </summary>
        public int ArmorId0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取装甲编号1。
        /// </summary>
        public int ArmorId1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取装甲编号2。
        /// </summary>
        public int ArmorId2
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
        /// 获取移动声音编号。
        /// </summary>
        public int MoveSoundId0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取移动声音编号。
        /// </summary>
        public int MoveSoundId1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取翻滚声音编号。
        /// </summary>
        public int DodgeSoundId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取装备武器声音编号。
        /// </summary>
        public int EquipWeaponSoundId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取被击倒声音编号。
        /// </summary>
        public int KnockDownSoundId
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
            ThrusterId = int.Parse(columnStrings[index++]);
            WeaponId0 = int.Parse(columnStrings[index++]);
            WeaponId1 = int.Parse(columnStrings[index++]);
            WeaponId2 = int.Parse(columnStrings[index++]);
            ArmorId0 = int.Parse(columnStrings[index++]);
            ArmorId1 = int.Parse(columnStrings[index++]);
            ArmorId2 = int.Parse(columnStrings[index++]);
            DeadEffectId = int.Parse(columnStrings[index++]);
            DeadSoundId = int.Parse(columnStrings[index++]);
            MoveSoundId0 = int.Parse(columnStrings[index++]);
            MoveSoundId1 = int.Parse(columnStrings[index++]);
            DodgeSoundId = int.Parse(columnStrings[index++]);
            EquipWeaponSoundId = int.Parse(columnStrings[index++]);
            KnockDownSoundId = int.Parse(columnStrings[index++]);

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
                    ThrusterId = binaryReader.Read7BitEncodedInt32();
                    WeaponId0 = binaryReader.Read7BitEncodedInt32();
                    WeaponId1 = binaryReader.Read7BitEncodedInt32();
                    WeaponId2 = binaryReader.Read7BitEncodedInt32();
                    ArmorId0 = binaryReader.Read7BitEncodedInt32();
                    ArmorId1 = binaryReader.Read7BitEncodedInt32();
                    ArmorId2 = binaryReader.Read7BitEncodedInt32();
                    DeadEffectId = binaryReader.Read7BitEncodedInt32();
                    DeadSoundId = binaryReader.Read7BitEncodedInt32();
                    MoveSoundId0 = binaryReader.Read7BitEncodedInt32();
                    MoveSoundId1 = binaryReader.Read7BitEncodedInt32();
                    DodgeSoundId = binaryReader.Read7BitEncodedInt32();
                    EquipWeaponSoundId = binaryReader.Read7BitEncodedInt32();
                    KnockDownSoundId = binaryReader.Read7BitEncodedInt32();
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

        private KeyValuePair<int, int>[] m_ArmorId = null;

        public int ArmorIdCount
        {
            get
            {
                return m_ArmorId.Length;
            }
        }

        public int GetArmorId(int id)
        {
            foreach (KeyValuePair<int, int> i in m_ArmorId)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetArmorId with invalid id '{0}'.", id));
        }

        public int GetArmorIdAt(int index)
        {
            if (index < 0 || index >= m_ArmorId.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetArmorIdAt with invalid index '{0}'.", index));
            }

            return m_ArmorId[index].Value;
        }

        private KeyValuePair<int, int>[] m_MoveSoundId = null;

        public int MoveSoundIdCount
        {
            get
            {
                return m_MoveSoundId.Length;
            }
        }

        public int GetMoveSoundId(int id)
        {
            foreach (KeyValuePair<int, int> i in m_MoveSoundId)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetMoveSoundId with invalid id '{0}'.", id));
        }

        public int GetMoveSoundIdAt(int index)
        {
            if (index < 0 || index >= m_MoveSoundId.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetMoveSoundIdAt with invalid index '{0}'.", index));
            }

            return m_MoveSoundId[index].Value;
        }

        private void GeneratePropertyArray()
        {
            m_WeaponId = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(0, WeaponId0),
                new KeyValuePair<int, int>(1, WeaponId1),
                new KeyValuePair<int, int>(2, WeaponId2),
            };

            m_ArmorId = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(0, ArmorId0),
                new KeyValuePair<int, int>(1, ArmorId1),
                new KeyValuePair<int, int>(2, ArmorId2),
            };

            m_MoveSoundId = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(0, MoveSoundId0),
                new KeyValuePair<int, int>(1, MoveSoundId1),
            };
        }
    }
}
