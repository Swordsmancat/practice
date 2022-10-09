using System.Collections;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{


    public static class FindTools
    {
        public static Transform FindFunc(Transform parent, string targetName)
        {
            Transform target = parent.Find(targetName);
            if (target != null)
            {
                return target;
            }
            for (int i = 0; i < parent.childCount; i++)
            {
                target = FindFunc(parent.GetChild(i), targetName);
                if (target != null)
                {
                    return target;
                }
            }
            return target;
        }

        public static T FindFunc<T>(Transform parent, string targetName) where T : Component
        {
            Transform target = parent.Find(targetName);
            if (target != null)
            {
                return target.GetComponent<T>();
            }
            for (int i = 0; i < parent.childCount; i++)
            {
                target = FindFunc(parent.GetChild(i), targetName);
                if (target != null)
                {
                    return target.GetComponent<T>();
                }
            }
            return target.GetComponent<T>();
        }


    }
}

