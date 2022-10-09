using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    /// <summary>
    /// �˴��������������ײ(ֻ���������ײ �����������������)
    /// </summary>
    public class FireCollider : MonoBehaviour
    {
        readonly private static int s_groundLayerMask = 6;       // �����(����)
        readonly private static int s_playerLayerMask = 11;      // ��Ҳ�(����)
        private bool _wait = false;
        private EnemyLogic _enemyLogic;
        private PlayerLogic _playerLogic;
        private BlackDragonFireLogic _parentLogic;

        //����ӿڷ����ȡ��ײ��λ��
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
            //    Debug.Log("���е���");
            //}

            if (other.layer == s_playerLayerMask)
            {
                //����˺�
              //  _playerLogic.ApplyDamage(_enemyLogic.RightHand, 10000);
            }
        }

        /// <summary>
        /// ������ɻ���(��������Ƶ��̫��)Ŀǰ0.1��һ��
        /// </summary>
        /// <returns></returns>
        IEnumerator Stop()
        {
            //ÿ��һ������һ��·��
            yield return new WaitForSecondsRealtime(0f);
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70015),
            typeof(BlackDragonFireHitGroundLogic));
            yield return new WaitForSecondsRealtime(0f);
            _wait = false;
        }
    }
}

