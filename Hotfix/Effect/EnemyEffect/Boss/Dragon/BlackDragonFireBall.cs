using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    /// <summary>
    /// 黑龙火球特效
    /// </summary>
    public class BlackDragonFireBall : EnemySkillEffectLogic
    {
        private readonly static float s_HideTime = 5;
        private float _hideTimer;
        private bool _isCanFollow = false;
        private ParticleSystem[] _particleSystems = null;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            transform.position = EffectStartPoint.transform.position;
            transform.localRotation = Enemy.transform.localRotation;
            
            Vector3 dir = Player.transform.position - transform.position;
            _hideTimer = 0;

            float angle = Vector3.Angle(dir, transform.forward);
            if (angle < 90)
            {
                _isCanFollow = true;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            _hideTimer += elapseSeconds;
            if(_hideTimer >= s_HideTime)
            {
                _isCanFollow = false;
                GameEntry.Entity.HideEntity(this);
            }
            else
            {
                if (_isCanFollow)
                {
                    //只设置一次
                    _isCanFollow = false;
                    transform.LookAt(Player.transform);
                }
                    
            }

        }

    }

}

