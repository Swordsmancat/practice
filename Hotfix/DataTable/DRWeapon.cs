//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2022-09-23 16:19:52.512
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
    /// 武器表。
    /// </summary>
    public class DRWeapon : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取武器编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取攻击力。
        /// </summary>
        public int Attack
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取攻击间隔。
        /// </summary>
        public float AttackInterval
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否为盾牌。
        /// </summary>
        public int IsShield
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取武器击中物体特效编号。
        /// </summary>
        public int WeaponHitEffectId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取武器击中声音列表。
        /// </summary>
        public List<int> WeaponHitSoundList
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取武器类别。
        /// </summary>
        public int WeaponState
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
            Attack = int.Parse(columnStrings[index++]);
            AttackInterval = float.Parse(columnStrings[index++]);
            IsShield = int.Parse(columnStrings[index++]);
            WeaponHitEffectId = int.Parse(columnStrings[index++]);
            WeaponHitSoundList = DataTableExtension.ParseInt32List(columnStrings[index++]);
            WeaponState = int.Parse(columnStrings[index++]);
            index++;
            index++;
            index++;

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
                    Attack = binaryReader.Read7BitEncodedInt32();
                    AttackInterval = binaryReader.ReadSingle();
                    IsShield = binaryReader.Read7BitEncodedInt32();
                    WeaponHitEffectId = binaryReader.Read7BitEncodedInt32();
					WeaponHitSoundList = binaryReader.ReadInt32List();
                    WeaponState = binaryReader.Read7BitEncodedInt32();
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
