using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    /// <summary>
    /// ����������(��س�����������)
    /// </summary>
    public class FireBallHitWindCollider : MonoBehaviour
    {
        readonly private static int s_playerLayerMask = 11;

        private DragonLogic _enemyLogic;
        private PlayerLogic _playerLogic;
        private float _prevTimePlayerHP;

        private void Start()
        {
            BlackDragonFireBall component = GetComponentInParent<BlackDragonFireBall>();
            _enemyLogic = component.EnemyObject.GetComponent<EnemyLogic>() as DragonLogic;
            _playerLogic = component.PlayerObject.GetComponent<PlayerLogic>();
            _prevTimePlayerHP = _playerLogic.PlayerData.HP;
        }

        private void OnParticleCollision(GameObject other)
        {
            //Debug.Log(other);
            if (other.layer == s_playerLayerMask)
            {
                Debug.Log(other.name + "������������");
                //��������
                _enemyLogic.IsThump = true;
              //  _playerLogic.ApplyDamage(_enemyLogic.RightHand, 10000);
                if (_prevTimePlayerHP != _playerLogic.PlayerData.HP)
                {
                    _prevTimePlayerHP = _playerLogic.PlayerData.HP;
                    _enemyLogic.FireBallCount = 0;
                }
            }

        }
    }

}
