using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    public class Old_HobgoblinFeipuLogic : EnemySkillEffectLogic
    {
        private Old_HobgoblinLogic _owner;
        private readonly static string feipu = "feipu";


        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            _owner = Enemy.GetComponent<Old_HobgoblinLogic>();
            EffectStartPoint = GetEffectStartPoint(Enemy, feipu);
            transform.position = EffectStartPoint.transform.position;
            _owner.IsEffectAttack = false;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

    }
}

