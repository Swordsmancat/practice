using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    /// <summary>
    /// 处理黑龙火焰地面的碰撞
    /// </summary>
    public class GroundFireCollider : MonoBehaviour
    {
        readonly private static int s_playerLayerMask = 11;      // 玩家层(过滤)
        private PlayerLogic _playerLogic;

        private void Update()
        {
            Debug.DrawRay(transform.position, transform.up);
            Ray ray = new Ray(transform.position, Vector3.up);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit,10))
            {
                if(hit.collider.gameObject.layer == s_playerLayerMask)
                {
                    Debug.Log(hit.collider.gameObject.name);
                    _playerLogic = hit.collider.GetComponent<PlayerLogic>();
                  //  _playerLogic.ApplyDamage(null, 10000);
                }
            }

        }
    }
}
