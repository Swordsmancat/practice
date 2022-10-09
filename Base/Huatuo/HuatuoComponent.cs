using UnityGameFramework.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using GameFramework.Resource;
using GameFramework;
using System.IO;

namespace Farm
{
   public class HuatuoComponent:GameFrameworkComponent
    {
        [SerializeField]
        private bool m_HuatuoMode;

        public bool HuatuoMode
        {
            get
            {
                return m_HuatuoMode;
            }
        }
    }
}
