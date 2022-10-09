//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2022-09-23 16:19:53.204
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
    /// 装甲表。
    /// </summary>
    public class DRArmor : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取装甲编号。
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
        /// 获取防御力。
        /// </summary>
        public int Defense
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取击中声音。
        /// </summary>
        public int HurtSoundId
        {
            get;
            private set;
        }
        public int HurtSoundId2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取击中效果。
        /// </summary>
        public int HurtEffectId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取霸体击中声音。
        /// </summary>
        public int StoicHurtSoundId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取霸体击中效果。
        /// </summary>
        public int StoicHurtEffectId
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
            Defense = int.Parse(columnStrings[index++]);
            HurtSoundId = int.Parse(columnStrings[index++]);
            HurtSoundId2 = int.Parse(columnStrings[index++]);
            HurtEffectId = int.Parse(columnStrings[index++]);
            StoicHurtSoundId = int.Parse(columnStrings[index++]);
            StoicHurtEffectId = int.Parse(columnStrings[index++]);

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
                    Defense = binaryReader.Read7BitEncodedInt32();
                    HurtSoundId = binaryReader.Read7BitEncodedInt32();
                    HurtSoundId2 = binaryReader.Read7BitEncodedInt32();
                    HurtEffectId = binaryReader.Read7BitEncodedInt32();
                    StoicHurtSoundId = binaryReader.Read7BitEncodedInt32();
                    StoicHurtEffectId = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
