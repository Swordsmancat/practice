using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    public class BlackDragonFireHitGroundLogic : EffectLogic
    {
        readonly private static string s_childName = "Print";
        readonly private static int s_layerMask = 1 << 6;       // �����(����)
        private BlackDragonFireLogic _fireLogic;
        private ParticleSystem[] _particleSystems;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            //��ȡ���������߼�
            _fireLogic = FindObjectOfType<BlackDragonFireLogic>();
            //��ȡ����������Ч��,ӵ����ײ���ĺ�����Ч
            _particleSystems = GetComponentsInChildren<ParticleSystem>();
            AddScriptToChild(s_childName);

            //����Ч�ŵ�������
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
        /// ������Ч���������ش�ʵ��
        /// </summary>
        /// <returns></returns>
        private bool CheckAllParticleSystemIsStop()
        {
            //�����Ч�Ƿ����
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
