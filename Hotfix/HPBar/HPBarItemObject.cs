using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;


namespace Farm.Hotfix
{
  public  class HPBarItemObject:ObjectBase
    {
        public static HPBarItemObject Create(object target)
        {
            HPBarItemObject hPBarItemObject = ReferencePool.Acquire<HPBarItemObject>();
            hPBarItemObject.Initialize(target);
            return hPBarItemObject;
        }

        protected override void Release(bool isShutdown)
        {
            HPBarItem hPBarItem = (HPBarItem)Target;
            if(hPBarItem == null)
            {
                return;
            }

            Object.Destroy(hPBarItem.gameObject);
        }
    }
}
