//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/6/29/周三 15:24:01
//------------------------------------------------------------
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace Farm.Hotfix
{
    [Serializable]
    public class Buff
    {
        [LabelText("触发类型"), HideLabel, EnumToggleButtons]
        public BuffType BuffTypeEnum;

        //[LabelText("击退"), HideLabel, ShowIf("BuffTypeEnum", BuffType.RePluse)]
        //public bool RePluse;

        //[LabelText("击飞"), HideLabel, ShowIf("BuffTypeEnum", BuffType.Striketofly)]
        //public bool Striketofly;

        //[LabelText("击倒"), HideLabel, ShowIf("BuffTypeEnum", BuffType.KnockDown)]
        //public bool KnockDown;

        [LabelText("控制触发时间"), InfoBox("基于动画时间"),HideLabel,EnableIf("@this.BuffTypeEnum!=BuffType.None")]
        public float TriggerTime;

        [LabelText("霸体"),HideLabel]
        public bool SuperArmor;

        [LabelText("失衡"), HideLabel]
        public bool Unbalance;//固定时间

        [LabelText("击晕"),HideLabel]
        public bool Stun;//固定时间

        [LabelText("治疗"), HideLabel]
        public float Treatment;

        [LabelText("恢复精力"), HideLabel]
        public float Energy;
          
    }
}
