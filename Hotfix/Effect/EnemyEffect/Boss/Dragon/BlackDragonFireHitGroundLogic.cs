using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    public class BlackDragonFireHitGroundLogic : EffectLogic
    {
        readonly private static string s_childName = "Print";
        readonly private static int s_layerMask = 1 << 6;       // 地面层(过滤)
        private BlackDragonFireLogic _fireLogic;
        private ParticleSystem[] _particleSystems;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            //获取黑龙火焰逻辑
            _fireLogic = FindObjectOfType<BlackDragonFireLogic>();
            //获取黑龙火焰特效中,拥有碰撞检测的核心特效
            _particleSystems = GetComponentsInChildren<ParticleSystem>();
            AddScriptToChild(s_childName);

            //将特效放到地面上
            //Ray downRay = new Ray(_colliderScirpt.pos, Vector3.down);
            //RaycastHit hit;
            //if (Physics.Raycast(downRay, out hit, 20, s_layerMask))
            //{
            //    Debug.DrawLine(downRay.origin, hit.point);
            //}
            if(_fireLogic != null)
            {
                transform.position = _fireLogic.pos;
            }
            
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (CheckAllParticleSystemIsStop())
            {
                GameEntry.Entity.HideEntity(this);
            }
        }

        /// <summary>
        /// 所有特效结束后隐藏此实体
        /// </summary>
        /// <returns></returns>
        private bool CheckAllParticleSystemIsStop()
        {
            //检查特效是否结束
            foreach(var obj in _particleSystems)
            {
                if (!obj.isStopped)
                    return false;
            }
            return true;
        }

        private GameObject AddScriptToChild(in string childName)
        {
            if (_particleSystems != null)
            {
                foreach (var particleSystem in _particleSystems)
                {
                    if (particleSystem.name == childName)
                    {
                        particleSystem.gameObject.AddComponent<GroundFireCollider>();
                        return particleSystem.gameObject;
                    }
                }
            }

            return null;
        }

    }
}
