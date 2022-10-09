using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Sirenix.OdinInspector;

namespace Farm.Hotfix
{
    /// <summary>
    /// 武器轨迹类提供显示轨迹和隐藏轨迹公开方法
    /// </summary>
    public class WeaponTrail : MonoBehaviour
    {

        [SerializeField]
        private List<GameObject> TrailList;


        public void ShowTrail()
        {
            for (int i = 0; i < TrailList.Count; i++)
            {
                TrailList[i].gameObject.SetActive(true);
            }
        }
        public void HideTrail()
        {
            for (int i = 0; i < TrailList.Count; i++)
            {
                TrailList[i].gameObject.SetActive(false);
            }
        }
    }
}
