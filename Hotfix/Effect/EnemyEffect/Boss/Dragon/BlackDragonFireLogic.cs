using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    public class BlackDragonFireLogic : EnemySkillEffectLogic
    {
        private readonly static string s_childName = "FireParticlesCore";
        private readonly static int s_waitTime = 5;
        readonly private static int s_layerMask = 1 << 6;       // 地面层(过滤)
        private ParticleSystem[] _particleSystems = null;
        private DragonLogic _owner;
        private float _exitTimer;
        private bool _wait;
        private bool _endFire = false;

        public GameObject CoreFireEffect;
        public Vector3 pos;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            _owner = Enemy.GetComponent<DragonLogic>();
            _particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
            CoreFireEffect = AddScriptToChild(s_childName);
            EnableAllEffect();
            Debug.Log("喷火喷火");
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            transform.SetPositionAndRotation(EffectStartPoint.transform.position, EffectStartPoint.transform.rotation);
            //Debug.DrawRay(transform.position, transform.forward * 10, Color.black);
            //Debug.DrawRay(EffectStartPoint.transform.position, EffectStartPoint.transform.forward * 10,Color.red);

            if (!_endFire)
            {
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 30, s_layerMask))
                {
                    Debug.DrawLine(ray.origin, hit.point);
                    CreateFireGround(hit.point);
                    pos = hit.point;
                }

            }


            if (!_owner.IsFire)
            {
                _exitTimer += elapseSeconds;
                if (_exitTimer >= s_waitTime)
                    GameEntry.Entity.HideEntity(this);
                UnEnableAllEffect();
            }

            if(_owner.IsDead)
            {
                if (_exitTimer >= s_waitTime)
                    GameEntry.Entity.HideEntity(this);
                UnEnableAllEffect();
            }
        }

        private void EnableAllEffect()
        {
            if(_particleSystems != null)
            {
                foreach(var particleSystem in _particleSystems)
                {
                    var emission = particleSystem.emission;
                    emission.enabled = true;
                }
            }
        }

        private void UnEnableAllEffect()
        {
            if (_particleSystems != null)
            {
                foreach (var particleSystem in _particleSystems)
                {
                    var emission = particleSystem.emission;
                    emission.enabled = false;
                }
            }
            _endFire = true;
        }

        private GameObject AddScriptToChild(in string childName)
        {
            if (_particleSystems != null)
            {
                foreach (var particleSystem in _particleSystems)
                {
                    if(particleSystem.name == childName)
                    {
                        particleSystem.gameObject.AddComponent<FireCollider>();
                        return particleSystem.gameObject;
                    }
                }
            }

            return null;
        }

        private void CreateFireGround(Vector3 hitPoint)
        {
            if (!_wait)
            {
                _wait = true;
                StartCoroutine("WaitCreate", hitPoint);
            }
            //pos = hitPoint;
            //GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70015),
            //    typeof(BlackDragonFireHitGroundLogic));
        }

        IEnumerator WaitCreate(Vector3 hitPoint)
        {
            //每隔一秒生成一个路径
            yield return new WaitForSecondsRealtime(0.1f);
            pos = hitPoint;
            //Debug.Log(pos);
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70015),
            typeof(BlackDragonFireHitGroundLogic));

            yield return new WaitForSecondsRealtime(0.1f);
            _wait = false;
        }

    }
}
