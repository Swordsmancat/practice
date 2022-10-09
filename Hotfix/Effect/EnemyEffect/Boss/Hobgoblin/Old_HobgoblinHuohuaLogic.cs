using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    public class Old_HobgoblinHuohuaLogic : EnemySkillEffectLogic
    {
        private Old_HobgoblinLogic _owner;
        private readonly static string huohua = "huohua";
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            _owner = Enemy.GetComponent<Old_HobgoblinLogic>();
            EffectStartPoint = GetEffectStartPoint(Enemy, huohua);
            transform.position = EffectStartPoint.transform.position;
            transform.rotation = EffectStartPoint.transform.rotation;
            _owner.IsEffectAttack = false;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

    }
}

