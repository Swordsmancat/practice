using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    /// <summary>
    /// 此处处理黑龙火焰碰撞(只处理玩家碰撞 不处理火焰地面的生成)
    /// </summary>
    public class FireCollider : MonoBehaviour
    {
        readonly private static int s_groundLayerMask = 6;       // 地面层(过滤)
        readonly private static int s_playerLayerMask = 11;      // 玩家层(过滤)
        private bool _wait = false;
        private EnemyLogic _enemyLogic;
        private PlayerLogic _playerLogic;
        private BlackDragonFireLogic _parentLogic;

        //对外接口方便获取碰撞点位置
        //public Vector3 pos;

        private void Start()
        {
            _parentLogic = GetComponentInParent<BlackDragonFireLogic>();
            _enemyLogic = _parentLogic.EnemyObject.GetComponent<EnemyLogic>();
            _playerLogic = _parentLogic.PlayerObject.GetComponent<PlayerLogic>();
        }

        private void OnParticleCollision(GameObject other)
        {
            //if (other.layer == s_groundLayerMask)
            //{
            //    if (!_wait)
            //    {
            //        _wait = true;
            //        StartCoroutine("Stop");
            //    }
            //    Debug.Log("击中地面");
            //}

            if (other.layer == s_playerLayerMask)
            {
                //造成伤害
              //  _playerLogic.ApplyDamage(_enemyLogic.RightHand, 10000);
            }
        }

        /// <summary>
        /// 间隔生成火焰(否则生成频率太高)目前0.1秒一个
        /// </summary>
        /// <returns></returns>
        IEnumerator Stop()
        {
            //每隔一秒生成一个路径
            yield return new WaitForSecondsRealtime(0f);
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70015),
            typeof(BlackDragonFireHitGroundLogic));
            yield return new WaitForSecondsRealtime(0f);
            _wait = false;
        }
    }
}

