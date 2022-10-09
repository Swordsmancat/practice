using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    /// <summary>
    /// ÁúµÄÑÌÎíÌØÐ§
    /// </summary>
    public class BlackDragonSmokeLogic : EnemySkillEffectLogic
    {
        private DragonLogic _owner;
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            _owner = Enemy.GetComponent<DragonLogic>();

            if(_owner.IsEffectAttack)
            {
                transform.position = EnemyWeapon.transform.position;
                _owner.IsEffectAttack = false;
            }
            else
            {
                transform.position = Enemy.transform.position;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

    }
}

