//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/6/29/周三 14:54:36
//------------------------------------------------------------
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Farm.Hotfix
{
    [CreateAssetMenu(fileName ="New Skill",menuName ="SkillSystem/Create Skillable")]
    public class Skill:ScriptableObject
    {
        [LabelText("技能名称"),HideLabel]
        public string SkillName;

        [LabelText("技能CD"), HideLabel]
        public float CD;

        [LabelText("持续时间"), HideLabel]
        public float Duration;

        [LabelText("是否能被打断"), HideLabel]
        public bool IsCanBreak;

        [LabelText("耗血"), HideLabel]
        public float NeedHPValue;

        [LabelText("耗蓝"), HideLabel]
        public float NeedMPValue;

        [LabelText("伤害"), HideLabel]
        public int Damage;

        [LabelText("伤害频率"), HideLabel]
        public float DamageFrequency;

        [LabelText("触发类型"), HideLabel,EnumToggleButtons]
        public SkillType TypeEnum;

        [LabelText("半径"),HideLabel,ShowIf("TypeEnum",SkillType.Circular)]
        public float CircularRadius;

        [LabelText("距离"), HideLabel, ShowIf("TypeEnum", SkillType.Circular)]
        public float CircularDistance;

        [LabelText("半径"), HideLabel, ShowIf("TypeEnum", SkillType.Sector)]
        public float SectorRadius;

        [LabelText("角度"), HideLabel, ShowIf("TypeEnum", SkillType.Sector)]
        public float SectorAngle;

        [LabelText("长宽高"), HideLabel, ShowIf("TypeEnum", SkillType.Rectangle)]
        public Vector3 RectangleHalfExtents;

        [LabelText("距离"), HideLabel, ShowIf("TypeEnum", SkillType.Rectangle)]
        public float RectangleDistance;

        [LabelText("技能图标"), HideLabel]
        public Sprite Sprite;

        [LabelText("技能音效名称"), HideLabel,InfoBox("播放'Assets/GameMain/Skills/Sounds/'中所属音效")]
        public string SoundName;

        [LabelText("技能动画名称"), HideLabel,InfoBox("播放角色动画机'Skill'层中所属动画")]
        public string AnimationClipName;

        [LabelText("技能特效名称"), HideLabel,InfoBox("播放'Assets/GameMain/Skills/Effect/'中所属特效")]
        public string EffectName;

        [LabelText("Buff列表"),HideLabel]
        public Buff Buffs;
    }
}
