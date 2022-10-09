//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/7/5/周二 10:09:02
//------------------------------------------------------------
using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace Farm.Hotfix
{
    public class SkillItemObject :ObjectBase
    {
        public static SkillItemObject Create(object target)
        {
            SkillItemObject skillItemObject = ReferencePool.Acquire<SkillItemObject>();
            skillItemObject.Initialize(target);
            return skillItemObject;
        }

        protected override void Release(bool isShutdown)
        {
            SkillItem skillItem =(SkillItem)Target;
            if(skillItem == null)
            {
                return;
            }
            Object.Destroy(skillItem.gameObject);
        }
    }
}
